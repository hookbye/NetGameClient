using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTest : MonoBehaviour {
    #region Public Attributes
    public Material renderMaterial;
    #endregion
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    #region Unity Messages
    void OnRenderImage(RenderTexture sourceTexture,RenderTexture destTexture)
    {
        if(renderMaterial !=null)
        {
            Graphics.Blit(sourceTexture, destTexture, renderMaterial);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
    #endregion
}
