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

            TextureToNormal(texture, destinationPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RunGenerationProcessSingle()
    {
        string destinationPath = AssetDatabase.GetAssetPath(texture);

        Debug.Log($"Normal Map Generator: Starting Normal Map Generation...");
        Debug.Log($"Normal Map Generator: Destination Path: {destinationPath}");
        Debug.Log($"Normal Map Generator: Strength: {normalMapStrength}");

        TextureToNormal(texture, destinationPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void TextureToNormal(Texture2D texture, string destinationPath)
    {
        if (texture == null) return;
        TextureImporter textureImporter = AssetImporter.GetAtPath(destinationPath) as TextureImporter;
        if (textureImporter == null) return;
        if (!textureImporter.isReadable)
        {
            textureImporter.isReadable = true;
            textureImporter.SaveAndReimport();
        }

        //generate normal map
        Texture2D normalTexture = GenerateNormalMap(texture);
        Debug.Log($"Normal Map Generator: Encoding...");

        byte[] bytes = normalTexture.EncodeToPNG();
        if (bytes == null || bytes.Length == 0)
        {
            Debug.LogError($"Normal Map Generator: Failed to read bytes...");
            return;
        }
        normalTexture.name = texture.name + "_Normal";
        destinationPath = Path.GetDirectoryName(destinationPath);
        destinationPath = Path.Combine(destinationPath, normalTexture.name);

        File.WriteAllBytes(destinationPath, bytes);

    }

    private Texture2D GenerateNormalMap(Texture2D texture)
    {
        Texture2D normalTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);

        Color[] pixelsColor = texture.GetPixels();
        Color[] newPixelColor = new Color[pixelsColor.Length];

        for (int i = 0; i < pixelsColor.Length; ++i) 
        {
            float grayScale = pixelsColor[i].grayscale;
            newPixelColor[i] = new Color(grayScale, grayScale, grayScale);
        }

        for (int y = 0; y < texture.width; ++y)
        {
            for (int x = 0; x < texture.height; ++x)
            {
                //float x
                //texture.pix
            }
        }
        return normalTexture;
    }
}