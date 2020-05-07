using UnityEngine;

public class HexMetrics {
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public static Vector3[] corners =
    {
        new Vector3(0f,0f,outerRadius),
        new Vector3(innerRadius,0f,0.5f*outerRadius),
        new Vector3(innerRadius,0f,-0.5f*outerRadius),
        new Vector3(0f,0f,-outerRadius),
        new Vector3(-innerRadius,0f,-0.5f*outerRadius),
        new Vector3(-innerRadius,0f,0.5f*outerRadius),
    };
    /* shape 0:center 1-6 corner
    * 
    *       *1   
    *    *      *
    * *6           *2
    * *     o0     *
    * *5           *3
    *    *      *
    *       *4
    *         
    * */
}
