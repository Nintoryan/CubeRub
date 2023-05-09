using CubeRub.LevelGenerator;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeTemplateGenerator))]
public class CubeTemplateGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        
        if(Application.isPlaying) return;
        var ctg = (CubeTemplateGenerator) target;
        if (FindObjectOfType<CubePiece>() == null)
        {
            if (GUILayout.Button("Create"))
            {
                ctg.Create();
            } 
        }
        else
        {
            if (GUILayout.Button("Recreate"))
            {
                ctg.Delete();
                ctg.Create();
            }

            if (GUILayout.Button("Remove"))
            {
                ctg.Delete();
            }
        }
        
    }
}
