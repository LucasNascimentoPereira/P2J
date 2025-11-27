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
        if (sourceFolder != null && destinationFolder == null)
        {
            Debug.LogError("Normal Map Generator: You must select a Destination Folder.");
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

        Debug.Log($"Normal Map Generator: Starting Normal Map Generation...");
        Debug.Log($"Normal Map Generator: Source Path: {sourcePath}");
        Debug.Log($"Normal Map Generator: Destination Path: {destinationPath}");
        Debug.Log($"Normal Map Generator: Strength: {normalMapStrength}");

        string[] allFileGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { sourcePath });
        Debug.Log($"Found {allFileGUIDs.Length} textures in the source folder.");

        foreach (string fileGUID in allFileGUIDs)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(fileGUID);

            if (texture == null) continue;

            Texture2D normalTexture = TextureToNormal(texture);

            if (normalTexture == null)
            {
                Debug.LogError($"Normal Map Generator: Failed to generate normal map, make sure import settings are correct!");
                continue;
            }

            if (!WriteToFolder(normalTexture, destinationPath))
            {
                Debug.Log($"Normal Map Generator: Failed to write to folder, make sure unity has write permissions!");
                continue;
            }

            Debug.Log($"Normal Map Generator: Successfully generated and wrote normal map.");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RunGenerationProcessSingle()
    {
        string sourcePath = AssetDatabase.GetAssetPath(texture);
        string destinationPath = sourcePath;

        Debug.Log($"Normal Map Generator: Starting Normal Map Generation...");
        Debug.Log($"Normal Map Generator: Source Path: {sourcePath}");
        Debug.Log($"Normal Map Generator: Destination Path: {destinationPath}");
        Debug.Log($"Normal Map Generator: Strength: {normalMapStrength}");

        Texture2D normalTexture = TextureToNormal(texture);

        if (normalTexture == null)
        {
            Debug.LogError($"Normal Map Generator: Failed to generate normal map, make sure import settings are correct!");
            return;
        }

        Debug.Log($"Normal Map Generator: Normal Map generated writing to {destinationFolder}");

        if (!WriteToFolder(normalTexture, destinationPath))
        {
            Debug.Log($"Normal Map Generator: Failed to write to folder, make sure unity has write permissions!");
        }
        else 
        {
            Debug.Log($"Normal Map Generator: Successfully generated and wrote normal map.");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private Texture2D TextureToNormal(Texture2D texture)
    {
        if (texture == null) return null;
        
        //Texture2D normalTexture = null;
        //normalTexture.name = texture.name + "_Normal";
        texture.name = texture.name + "_Normal";
        return texture;
    }

    private bool WriteToFolder(Texture2D normalTexture, string destinationPath)
    {
        byte[] bytes = normalTexture.EncodeToPNG();
        if (bytes == null) return false;
        File.WriteAllBytes(destinationPath, bytes);
        return false;
    }
}