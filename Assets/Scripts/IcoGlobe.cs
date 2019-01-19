using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcoGlobe : MonoBehaviour {
    public GameObject icoSphere;
    public Color defaultColor;
    public Color hoverColor;
    public Color selectedColor;

    Mesh mesh;
    Color[] colors;

    // Use this for initialization
    void Start () {
        mesh = icoSphere.GetComponent<MeshFilter>().mesh;
        InitColors();
        this.colors = (Color[])mesh.colors.Clone();
    }
    
    void InitColors()
    {        
        Color[] colors = new Color[mesh.vertices.Length];

        for (int x=0;x<colors.Length;x++)
        {
            colors[x] = defaultColor;
        }

        mesh.colors = colors;
    }

    public void Hover(int triangleIndex)
    {
        Color[] colors = (Color[])this.colors.Clone();
        int[] triangles = mesh.triangles;

        colors[mesh.triangles[triangleIndex * 3 + 0]] = 
            colors[mesh.triangles[triangleIndex * 3 + 1]] = 
            colors[mesh.triangles[triangleIndex * 3 + 2]] = hoverColor;
        
        mesh.colors = colors;
        colors = null;
    }

    public void Select(int triangleIndex)
    {
        Color[] colors = (Color[])this.colors.Clone();
        int[] triangles = mesh.triangles;

        colors[mesh.triangles[triangleIndex * 3 + 0]] =
            colors[mesh.triangles[triangleIndex * 3 + 1]] =
            colors[mesh.triangles[triangleIndex * 3 + 2]] = selectedColor;

        mesh.colors = colors;
        this.colors = (Color[])mesh.colors.Clone();
        colors = null;
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}
