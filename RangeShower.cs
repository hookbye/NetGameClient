using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class RangeShower : MonoBehaviour {
    public enum ShapeType
    {
        none,
        sector,
        obb,
        ring,
    }
    #region Public Attributes
    public ShapeType m_type = ShapeType.none;
    public float m_degree = 120;
    public float intervalDegree = 10;
    public float m_radius = 5;
    public float m_InnerOff = 2;
    public Material m_circleIndicator;
    public Material m_RectIndicator;
    #endregion
    #region Private Attributes
    Mesh mesh = null;
    MeshFilter meshFilter = null;
    MeshRenderer meshRenderer = null;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    int lastCount;
    #endregion
    // Use this for initialization
    void Start () {
		
	}
#if UNITY_EDITOR
    // Update is called once per frame
    void Update () {
		if(!Application.isPlaying)
        {
            updateMesh(m_type, m_degree, m_radius, m_InnerOff);
        }
	}
#endif
    public void updateMesh(ShapeType shape,float degree,float radius,float innerOff)
    {
        if(shape == ShapeType.obb)
        {
            if(mesh == null)
            {
                mesh = new Mesh();
            }
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            Vector2 uvCenter = new Vector2(0.5f, 0.5f);
            mesh.Clear();
            vertices = new Vector3[4];
            triangles = new int[6];
            uvs = new Vector2[4];
            vertices[0] = Vector3.zero;
            uvs[0] = uvCenter;

            vertices[0] = new Vector3(degree, 0, 0);//1,0
            vertices[1] = new Vector3(degree, 0, radius);//1,1
            vertices[2] = new Vector3(-degree, 0, radius);//-1,1
            vertices[3] = new Vector3(-degree, 0, 0);//-1,0

            triangles[0] = 3;
            triangles[1] = 2;
            triangles[2] = 0;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 1;

            uvs[0] = new Vector2(0, 1);
            uvs[1] = new Vector2(0, 0);
            uvs[2] = new Vector2(1, 0);
            uvs[3] = new Vector2(1, 1);

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            meshFilter.sharedMesh = mesh;
            meshFilter.sharedMesh.name = "CIrcularSectorMesh";
            meshRenderer.sharedMaterial = m_RectIndicator;
            m_degree = degree;
            m_radius = radius;
        }
    }
}
