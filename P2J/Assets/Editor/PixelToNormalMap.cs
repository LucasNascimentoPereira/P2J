using UnityEditor;
using UnityEngine;
using System.IO;

public class NormalMapGeneratorWindow : EditorWindow
{
    private DefaultAsset sourceFolder;
    private DefaultAsset destinationFolder;
    private float normalMapStrength = 2.0f;

    [MenuItem("Tools/Pixel Art Normal Map Generator")]
    public static void ShowWindow()
    {
        GetWindow<NormalMapGeneratorWindow>("Normal Map Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Normal Map Generator Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space(10);

        sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Source Folder",
            sourceFolder,
            typeof(DefaultAsset),
            false);

        destinationFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Destination Folder",
            destinationFolder,
            typeof(DefaultAsset),
            false);

        EditorGUILayout.Space(5);

        normalMapStrength = EditorGUILayout.Slider(
            "Normal Strength",
            normalMapStrength,
            0.1f,
            10.0f);

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Generate Normal Maps"))
        {
            RunGenerationProcess();
        }
    }

    private void RunGenerationProcess()
    {
        if (sourceFolder == null)
        {
            Debug.LogError("Normal Map Generator: You must select a Source Folder.");
            return;
        }

        if (destinationFolder == null)
        {
            Debug.LogError("Normal Map Generator: You must select a Destination Folder.");
            return;
        }
        string sourcePath = AssetDatabase.GetAssetPath(sourceFolder);
        string destinationPath = AssetDatabase.GetAssetPath(destinationFolder);

        Debug.Log($"Starting Normal Map Generation...");
        Debug.Log($"Source Path: {sourcePath}");
        Debug.Log($"Destination Path: {destinationPath}");
        Debug.Log($"Strength: {normalMapStrength}");

        string[] allFileGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { sourcePath });
        Debug.Log($"Found {allFileGUIDs.Length} textures in the source folder.");


        AssetDatabase.Refresh();
    }
}