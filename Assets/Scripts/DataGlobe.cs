using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGlobe : MonoBehaviour
{
    public Material PointMaterial;
    public Gradient Colors;
    public GameObject Earth;
    public GameObject PointPrefab;
    public DataLoader loader;
    public float ValueScaleMultiplier = 1;
    public bool isReady = false;
    public GameObject EarthVertGroupParent;

    List<Vector3> meshVertices = new List<Vector3>(65000);
    List<int> meshIndices = new List<int>(117000);
    List<Color> meshColors = new List<Color>(65000);
    int vertIndex = 0;
    
    public void Start()
    {
        
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
        //Destroy(pointObject);
        isReady = true;
    }
        
    private void AppendPointVertices(GameObject pointObject, Vector3[] verts, int[] indices, InfectionPoint infectionPoint)
    {
        Color valueColor = Colors.Evaluate(infectionPoint.Infection);
        Vector3 pos;
        int prevVertCount = meshVertices.Count;

        pos.x = 0.5f * Mathf.Cos((infectionPoint.x) * Mathf.Deg2Rad) * Mathf.Cos(infectionPoint.y * Mathf.Deg2Rad);
        pos.y = 0.5f * Mathf.Sin(infectionPoint.y * Mathf.Deg2Rad);
        pos.z = 0.5f * Mathf.Sin((infectionPoint.x) * Mathf.Deg2Rad) * Mathf.Cos(infectionPoint.y * Mathf.Deg2Rad);

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
    
    //public void UpdateInfectionPointColors(World world)
    //{
    //    int globalIndex = 0;
    //    int numVerts = 20;
        
    //    for (int i=2; i<5; i++)
    //    {
    //        GameObject vertGroup = Earth.transform.GetChild(i).gameObject;
    //        Mesh mesh = vertGroup.GetComponent<MeshFilter>().mesh;

    //        for (int index=0; index<mesh.vertices.Length; index+=numVerts)
    //        {
    //            InfectionPoint point = GetInfectionPoint(world, globalIndex); //s world.infectionPoints[globalIndex/numVerts, (globalIndex / 360)/numVerts];

    //            //Color newColor = Colors.Evaluate(point.infection);
    //            //Color[] colors = mesh.colors;
    //            //for (int x = index; x < index + numVerts; x++)
    //            //{
    //            //    colors[x] = newColor;
    //            //}
    //            //mesh.colors = colors;

    //            globalIndex += numVerts;
    //        }
    //    }
    //}
    
    public void UpdateInfectionPointColor(InfectionPoint point)
    {
        if (point == null) return;

        Color newColor = Colors.Evaluate(point.Infection);
        int objectIndex = (point.vertIndex / 65000); //which object is it in
        GameObject vertGroup = EarthVertGroupParent.transform.GetChild(objectIndex).gameObject; //get the object it is in
        Mesh mesh = vertGroup.GetComponent<MeshFilter>().mesh;
        int indexStart = point.vertIndex % 65000;

        Color[] colors = mesh.colors;
        for (int i=indexStart; i<indexStart + 20; i++)
        {
            colors[i] = newColor;
        }
        mesh.colors = colors;
    }
        
    private void CreateObject()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = meshIndices.ToArray();
        mesh.colors = meshColors.ToArray();

        GameObject obj = new GameObject("VertGroup_" + vertIndex);
        obj.tag = "EarthVertGroup";
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
}
