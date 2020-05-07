using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public Text celllabelPrefab;
    Canvas gridCanvas;
    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
    }
}
