using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGlobe : MonoBehaviour {
    public Material PointMaterial;
    public Gradient Colors;
    public GameObject Earth;
    public GameObject PointPrefab;
    public DataLoader loader;
    public float ValueScaleMultiplier = 1;
    GameObject[] seriesObjects;
    int currentSeries = 0;
    WorldData world;

    public void Start()
    {
        world = loader.LoadJSON("WorldData");
        CreateMeshes();
    }

    public void CreateMeshes()
    {
        CountryData[] countries = world.Countries;

        seriesObjects = new GameObject[countries.Length];
        GameObject p = Instantiate<GameObject>(PointPrefab); //instantiate one point prefab
        Vector3[] verts = p.GetComponent<MeshFilter>().mesh.vertices; //get its mesh verts
        int[] indices = p.GetComponent<MeshFilter>().mesh.triangles; //get its triangles

        List<Vector3> meshVertices = new List<Vector3>(65000);
        List<int> meshIndices = new List<int>(117000);
        List<Color> meshColors = new List<Color>(65000);

        //for each country in country data
        for (int i = 0; i < countries.Length; i++)
        {
            //create a country object to store the meshes on
            GameObject countryObject = new GameObject(countries[i].Name);
            countryObject.transform.parent = Earth.transform;
            seriesObjects[i] = countryObject;

            CountryData countryData = countries[i];
            foreach (CountryPoint point in countryData.Points)
            {
                AppendPointVertices(p, verts, indices, point.x, point.y, point.population, meshVertices, meshIndices, meshColors);
                if (meshVertices.Count + verts.Length > 65000) //once we get past 65k, cut an object
                {
                    CreateObject(meshVertices, meshIndices, meshColors, countryObject);
                }
            }
            CreateObject(meshVertices, meshIndices, meshColors, countryObject);
        }
        Destroy(p);
    }

    private void AppendPointVertices(GameObject p, Vector3[] verts, int[] indices, float lng, float lat, float value, List<Vector3> meshVertices, List<int> meshIndices, List<Color> meshColors)
    {
        Color valueColor = Colors.Evaluate(value);
        Vector3 pos;
        pos.x = 0.5f * Mathf.Cos((lng) * Mathf.Deg2Rad) * Mathf.Cos(lat * Mathf.Deg2Rad);
        pos.y = 0.5f * Mathf.Sin(lat * Mathf.Deg2Rad);
        pos.z = 0.5f * Mathf.Sin((lng) * Mathf.Deg2Rad) * Mathf.Cos(lat * Mathf.Deg2Rad);
        p.transform.parent = Earth.transform;
        p.transform.position = pos;
        p.transform.localScale = new Vector3(1, 1, Mathf.Max(0.001f, value * ValueScaleMultiplier));
        p.transform.LookAt(pos * 2);

        int prevVertCount = meshVertices.Count;

        for (int k = 0; k < verts.Length; k++)
        {
            meshVertices.Add(p.transform.TransformPoint(verts[k]));
            meshColors.Add(valueColor);
        }
        for (int k = 0; k < indices.Length; k++)
        {
            meshIndices.Add(prevVertCount + indices[k]);
        }
    }

    private void CreateObject(List<Vector3> meshVertices, List<int> meshIndices, List<Color> meshColors, GameObject seriesObj)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = meshIndices.ToArray();
        mesh.colors = meshColors.ToArray();

        GameObject obj = new GameObject();
        obj.transform.parent = Earth.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        obj.AddComponent<MeshFilter>().mesh = mesh;
        obj.AddComponent<MeshRenderer>().material = PointMaterial;
        obj.transform.parent = seriesObj.transform;

        meshVertices.Clear();
        meshIndices.Clear();
        meshColors.Clear();
    }
}
