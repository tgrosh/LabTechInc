using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class DataGlobe : OVRGrabbable
{
    public Material PointMaterial;
    public Gradient Colors;
    public GameObject Earth;
    public GameObject PointPrefab;
    public VertGroup vertGroupPrefab;
    public DataLoader loader;
    public float ValueScaleMultiplier = 1;
    public bool isReady = false;
    public GameObject EarthVertGroupParent;
    public float updateInterval = 1.0f;
    public bool isGrabbing = false;
    public float resumeDelay = 2f;
    
    private AutoMoveAndRotate autoMover;
    float currentTime = 0f;
    List<Vector3> meshVertices = new List<Vector3>(65000);
    List<int> meshIndices = new List<int>(117000);
    List<Color> meshColors = new List<Color>(65000);
    int vertIndex = 0;
    private float resumeTime;

    new public void Start()
    {
        autoMover = GetComponent<AutoMoveAndRotate>();
    }
    
    public void Update()
    {
        if (currentTime >= updateInterval)
        {
            UpdateMeshColors();
            currentTime = 0;
        }
        currentTime += Time.deltaTime;

        if (m_grabbedBy != null)
        {
            resumeTime = 0f;
            autoMover.enabled = false;        
        }
        else
        {
            if (resumeTime >= resumeDelay)
            {
                autoMover.enabled = true;
                resumeTime = 0f;
            }
            else
            {
                resumeTime += Time.deltaTime;
            }
        }

    }

    public void CreateWorldMeshes(World world)
    {
        GameObject pointObject = Instantiate(PointPrefab); //instantiate one point prefab
        Vector3[] verts = pointObject.GetComponent<MeshFilter>().mesh.vertices; //get its mesh verts
        int[] indices = pointObject.GetComponent<MeshFilter>().mesh.triangles; //get its triangles
                
        for (int y = 0; y < 180; y++)
        {
            for (int x = 0; x < 360; x++)
            {
                InfectionPoint point = world.infectionPoints[y][x];
                if (point != null)
                {
                    point.vertIndex = vertIndex;
                    vertIndex += verts.Length;
                    AppendPointVertices(pointObject, verts, indices, point);
                    if (meshVertices.Count + verts.Length > 65000) //once we get past 65k, cut an object
                    {
                        CreateObject();
                    }
                }                
            }
        }

        CreateObject();
        Destroy(pointObject);
        isReady = true;
    }
        
    private void AppendPointVertices(GameObject pointObject, Vector3[] verts, int[] indices, InfectionPoint infectionPoint)
    {
        Color valueColor = Colors.Evaluate(infectionPoint.Infection);
        Vector3 pos = GetGlobePosition(infectionPoint.x, infectionPoint.y);
        int prevVertCount = meshVertices.Count;
        
        pointObject.transform.parent = Earth.transform;
        pointObject.transform.position = pos;
        pointObject.transform.localScale = new Vector3(1, 1, Mathf.Max(0.001f, infectionPoint.population * ValueScaleMultiplier));
        pointObject.transform.LookAt(pos * 2);

        for (int k = 0; k < verts.Length; k++)
        {
            meshVertices.Add(pointObject.transform.TransformPoint(verts[k]));
            meshColors.Add(valueColor);
        }

        for (int k = 0; k < indices.Length; k++)
        {
            meshIndices.Add(prevVertCount + indices[k]);
        }
    }

    public Vector3 GetGlobePosition(float x, float y)
    {
        Vector3 pos;

        pos.x = 0.5f * Mathf.Cos((x) * Mathf.Deg2Rad) * Mathf.Cos(y * Mathf.Deg2Rad);
        pos.y = 0.5f * Mathf.Sin(y * Mathf.Deg2Rad);
        pos.z = 0.5f * Mathf.Sin((x) * Mathf.Deg2Rad) * Mathf.Cos(y * Mathf.Deg2Rad);

        return pos;
    }
    
    public void UpdateMeshColors()
    {
        foreach (Transform t in EarthVertGroupParent.transform)
        {
            Mesh mesh = t.gameObject.GetComponent<MeshFilter>().mesh;
            mesh.colors = t.gameObject.GetComponent<VertGroup>().colors;
        }        
    }
    
    public void UpdateInfectionPointColor(InfectionPoint point)
    {
        if (point == null) return;

        Color newColor = Colors.Evaluate(point.Infection);
        int objectIndex = (point.vertIndex / 65000); //which object is it in
        VertGroup vertGroup = EarthVertGroupParent.transform.GetChild(objectIndex).GetComponent<VertGroup>();
        GameObject obj = EarthVertGroupParent.transform.GetChild(objectIndex).gameObject; //get the object it is in
        int indexStart = point.vertIndex % 65000;

        Color[] colors = vertGroup.colors;
        for (int i=indexStart; i<indexStart + 20; i++)
        {
            colors[i] = newColor;
        }
        vertGroup.colors = colors;
    }
        
    private void CreateObject()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = meshIndices.ToArray();
        mesh.colors = meshColors.ToArray();

        VertGroup vertGroup = Instantiate(vertGroupPrefab);
        vertGroup.colors = meshColors.ToArray();

        GameObject obj = vertGroup.gameObject;
        obj.name = "VertGroup_" + vertIndex;
        obj.transform.parent = EarthVertGroupParent.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        obj.AddComponent<MeshFilter>().mesh = mesh;
        obj.AddComponent<MeshRenderer>().material = PointMaterial;
        obj.transform.parent = EarthVertGroupParent.transform;
        
        meshVertices.Clear();
        meshIndices.Clear();
        meshColors.Clear();
    }

    /// <summary>
    /// Notifies the object that it has been grabbed.
    /// </summary>
    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;
    }

    /// <summary>
    /// Notifies the object that it has been released.
    /// </summary>
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}
