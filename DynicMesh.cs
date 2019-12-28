using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynicMesh : MonoBehaviour {

    protected bool isDirty = true;
    protected Mesh mesh;
    protected MeshRenderer meshRender;
    protected MeshFilter meshFilter;

    protected List<Vector3> vertices = new List<Vector3>();
    protected List<Vector2> uvs = new List<Vector2>();
    protected List<int> triangles = new List<int>();
	// Use this for initialization
	void Start () {
		if(mesh == null)
        {
            mesh = new Mesh();
        }
        if (meshRender == null)
        {
            meshRender = gameObject.AddComponent<MeshRenderer>();
        }
        if(meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        List<Vector3> vecs = new List<Vector3>();
        vecs.Add(new Vector3(10, 0, 0));
        vecs.Add(new Vector3(0, 0, 10));
        vecs.Add(new Vector3(10, 0, 10));
        AddTriangle(vecs);

        List<Vector3> vecs2 = new List<Vector3>();
        vecs2.Add(new Vector3(0, 0, 0));
        vecs2.Add(new Vector3(5, 0, 0));
        vecs2.Add(new Vector3(0, 0, 5));
        AddTriangle(vecs2);
    }
	
	// Update is called once per frame
	void Update () {
		if(isDirty)
        {
            isDirty = false;
            if (vertices.Count == 0 || triangles.Count == 0 || uvs.Count == 0)
                return;
            mesh.Clear();

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles,0);
            mesh.SetUVs(1,uvs);
            meshFilter.sharedMesh = mesh;

            mesh.RecalculateNormals();
        }
	}

    public void AddTriangle(List<Vector3> points)
    {
        int beginIdx = triangles.Count;
        bool isReverse = beginIdx / 3 % 2 == 1;
        vertices.Add(points[0]);
        vertices.Add(points[1]);
        vertices.Add(points[2]);
        if (isReverse)
        {
            triangles.Add(beginIdx + 0);
            triangles.Add(beginIdx + 2);
            triangles.Add(beginIdx + 1);
        }
        else
        {
            triangles.Add(beginIdx + 0);
            triangles.Add(beginIdx + 1);
            triangles.Add(beginIdx + 2);
        }
        
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        isDirty = true;

    }

}
