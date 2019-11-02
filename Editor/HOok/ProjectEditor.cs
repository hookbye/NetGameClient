using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectEditor:Editor {

	[MenuItem("HOok/build")]
    static void Build()
    {
         string[] scenes =
        {
            "Assets/Scence/DemoScene.unity",
            //"Assets/ActScene.unity",
         };
        BuildPipeline.BuildPlayer(scenes,"NetGame.apk",BuildTarget.Android,BuildOptions.None);
    }
}
