using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrianManager : MonoBehaviour {
    public TerrainData terrainData;
    private float[,] heightsBackups;

	// Use this for initialization
	void Start () {
        ModifyTerrainDataHeight(terrainData);
        //StartCoroutine(Disable());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Terrain CreateTerrain()
    {
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 513;
        terrainData.baseMapResolution = 513;
        terrainData.size = new Vector3(50, 50, 50);
        terrainData.alphamapResolution = 512;
        terrainData.SetDetailResolution(32, 8);
        AssetDatabase.CreateAsset(terrainData, "Assets/Terrian_ModifyHeight.asset");
        AssetDatabase.SaveAssets();
        GameObject obj = Terrain.CreateTerrainGameObject(terrainData);
        return obj.GetComponent<Terrain>();
    }

    public void ModifyTerrainDataHeight(TerrainData terrainData)
    {
        int width = terrainData.heightmapWidth;
        int height = terrainData.heightmapHeight;
        
        float[,] array = new float[width, height];
        //Debug.Log("terrian w " + width + " h " + height);
        
        width = Mathf.Min(100, width);
        height = Mathf.Min(20, height);
        for (int i = 0;i<width;i++)
        {
            for(int j = 0;j< height; j++)
            {
                float f1 = i;
                float f2 = width;
                float f3 = j;
                float f4 = height;
                float baseV = (f1 / f2 + f3 / f4) / 2 * 1;
                //Debug.Log("baseV"+ baseV);
                array[i, j] = 0;// baseV*baseV/10;
            }
        }
        heightsBackups = terrainData.GetHeights(0, 0, width, height);
        terrainData.SetHeights(0, 0, array);
    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Recover");
        terrainData.SetHeights(0, 0, heightsBackups);
    }
}
