using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class NormalMapGeneratorWindow : EditorWindow
{
    private DefaultAsset sourceFolder;
    private DefaultAsset destinationFolder;
    private Texture2D texture;
    private float normalMapStrength = 2.0f;

    [MenuItem("Tools/Pixel Art Normal Map Generator")]
    public static void ShowWindow()
    {
        GetWindow<NormalMapGeneratorWindow>("Normal Map Generator");
    }

    private void OnGUI()
    {

        GUILayout.Label("   Normal Map Generator Settings", EditorStyles.largeLabel);

        EditorGUILayout.Space(10);

        GUILayout.Label("Batch mode:", EditorStyles.boldLabel);

        sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "   Source Folder",
            sourceFolder,
            typeof(DefaultAsset),
            false);

        destinationFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "   Destination Folder",
            destinationFolder,
            typeof(DefaultAsset),
            false);

        EditorGUILayout.Space(10);

        GUILayout.Label("Single mode:", EditorStyles.boldLabel);

        texture = (Texture2D)EditorGUILayout.ObjectField(
            "   Texture2D",
            texture,
            typeof(Texture2D),
            false);

        EditorGUILayout.Space(5);

        normalMapStrength = EditorGUILayout.Slider(
            "Normal Strength",
            normalMapStrength,
            0.1f,
            5.0f);

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Generate Normal Maps"))
        {
            Validate();
        }

        if (texture != null) 
        {
            sourceFolder = null;
            destinationFolder = null;
        }
        if (sourceFolder != null || destinationFolder != null)
        {
            texture = null;
        }
    }

    private void Validate()
    {
        if (sourceFolder == null && destinationFolder == null && texture == null)
        {
            Debug.LogError("Normal Map Generator: You must select a Source Folder or a Texture2D.");
            return;
        }

        if (texture != null)
        {
            RunGenerationProcessSingle();
        }
        else if (sourceFolder != null && destinationFolder != null)
        {
            RunGenerationProcessBatch();
        }
    } 

    private void RunGenerationProcessBatch()
    {
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

    private void RunGenerationProcessSingle()
    {
        string sourcePath = AssetDatabase.GetAssetPath(texture);
        string destinationPath = sourcePath;

        Debug.Log($"Starting Normal Map Generation...");
        Debug.Log($"Source Path: {sourcePath}");
        Debug.Log($"Destination Path: {destinationPath}");
        Debug.Log($"Strength: {normalMapStrength}");

        string newFileName = texture.name + "Normal";

        Debug.Log($"Normal map name: {newFileName}");

        AssetDatabase.Refresh();
    }
}