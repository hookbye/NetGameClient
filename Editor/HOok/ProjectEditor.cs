using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectEditor:Editor {

	[MenuItem("HOok/buildAnroid")]
    static void BuildAndroid()
    {
         string[] scenes =
        {
            "Assets/Scence/DemoScene.unity",
            //"Assets/ActScene.unity",
         };
        BuildPipeline.BuildPlayer(scenes,"NetGame.apk",BuildTarget.Android,BuildOptions.None);
    }
    [MenuItem("HOok/buildIOS")]
    static void BuildIOS()
    {
        string[] scenes =
       {
            "Assets/Scence/DemoScene.unity",
            //"Assets/ActScene.unity",
         };
        BuildPipeline.BuildPlayer(scenes, "NetGame", BuildTarget.iOS, BuildOptions.None);

    }
}
