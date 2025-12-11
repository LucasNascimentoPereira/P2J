using UnityEditor;
using UnityEngine;
using System.IO;

public class NormalMapGeneratorWindow : EditorWindow
{
    private DefaultAsset sourceFolder;
    private DefaultAsset destinationFolder;
    private Texture2D texture;
    private float normalMapStrength = 2.0f;
    private bool isSMode = false;

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
            10.0f);

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Generate Bump Maps"))
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
            Debug.LogError("Bump Map Generator: You must select a Source Folder or a Texture2D.");
            return;
        }
        if (sourceFolder != null && destinationFolder == null)
        {
            Debug.LogError("Normal Map Generator: You must select a Destination Folder.");
        }

        if (texture != null)
        {
            isSMode = true;
            RunGenerationProcessSingle();
        }
        else if (sourceFolder != null && destinationFolder != null)
        {
            isSMode = false;
            RunGenerationProcessBatch();
        }
    }

    private void RunGenerationProcessBatch()
    {
        string sourcePath = AssetDatabase.GetAssetPath(sourceFolder);
        string destinationPath = AssetDatabase.GetAssetPath(destinationFolder);
        Debug.Log($"Normal Map Generator: Make sure the original texture has (Alpha as Transparency) ticked off in the import settings");
        Debug.Log($"Normal Map Generator: Starting Normal Map Generation...");
        Debug.Log($"Normal Map Generator: Source Path: {sourcePath}");
        Debug.Log($"Bump Map Generator: Destination Path: {destinationPath}");
        Debug.Log($"Bump Map Generator: Strength: {normalMapStrength}");

        string[] allFileGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { sourcePath });
        Debug.Log($"Found {allFileGUIDs.Length} textures in the source folder.");

        foreach (string fileGUID in allFileGUIDs)
        {
            Texture2D textureLoaded = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(fileGUID));

            if (textureLoaded == null)
            {
                Debug.LogError($"Bump Map Generator: Texture not found or not loaded");
                continue;
            }

            TextureToNormal(textureLoaded, destinationPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RunGenerationProcessSingle()
    {
        string destinationPath = AssetDatabase.GetAssetPath(texture);

        Debug.Log($"Bump Map Generator: Starting Normal Map Generation...");
        Debug.Log($"Bump Map Generator: Destination Path: {destinationPath}");
        Debug.Log($"Bump Map Generator: Strength: {normalMapStrength}");

        
        TextureToNormal(texture, destinationPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void TextureToNormal(Texture2D texture, string destinationPath)
    {
        if (texture == null) return;


        Texture2D normalTexture = GenerateNormalMap(texture);
        Debug.Log($"Bump Map Generator: Encoding...");

        byte[] bytes = normalTexture.EncodeToPNG();
        if (bytes == null || bytes.Length == 0)
        {
            Debug.LogError($"Bump Map Generator: Failed to read bytes...");
            return;
        }
        normalTexture.name = texture.name + "_Normal.PNG";
        if (isSMode)
        {
            destinationPath = Path.GetDirectoryName(destinationPath);
            Debug.Log(destinationPath);
        }
        Debug.Log(destinationPath);
        destinationPath = Path.Combine(destinationPath, normalTexture.name);
        Debug.Log(destinationPath);

        File.WriteAllBytes(destinationPath, bytes);

        AssetDatabase.Refresh();

        TextureImporter textureImporter = AssetImporter.GetAtPath(destinationPath) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError($"Bump Map Generator: Texture does not exist");
            return;
        }
        textureImporter.isReadable = true;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.textureType = TextureImporterType.NormalMap;
        textureImporter.mipmapEnabled = false;
        textureImporter.SaveAndReimport();

    }

    private Texture2D GenerateNormalMap(Texture2D texture)
    {
        Texture2D normalTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);

        for (int y = 0; y < texture.height; ++y)
        {
            for (int x = 0; x < texture.width; ++x)
            {

                Color c = texture.GetPixel(x, y);
                Vector3 norVector = new Vector3(c.r * normalMapStrength, c.g * normalMapStrength, 1.0f * normalMapStrength);
                norVector.Normalize();

                Color newPixel = new Color((norVector.x + 1.0f) / 2.0f, (norVector.y + 1.0f) / 2.0f, (norVector.z + 1.0f) / 2.0f);
                normalTexture.SetPixel(x, y, newPixel);
            }
        }

        for (int y = 0; y < texture.height; ++y)
        {
            for (int x = 0; x < texture.width; ++x)
            {

                float x1 = texture.GetPixel(Mathf.Clamp(x + 1, 0, texture.width - 1), y).grayscale;
                float x2 = texture.GetPixel(Mathf.Clamp(x - 1, 0, texture.width - 1), y).grayscale;
                float y1 = texture.GetPixel(x, Mathf.Clamp(y + 1, 0, texture.height - 1)).grayscale;
                float y2 = texture.GetPixel(x, Mathf.Clamp(y - 1, 0, texture.height - 1)).grayscale;

                Vector3 norVector = new Vector3((x2 - x1) * normalMapStrength, (y2 - y1) * normalMapStrength, 1.0f);


                norVector.Normalize();

                Color newPixel = new Color((norVector.x + 1f) / 2f, (norVector.y + 1f) / 2f, norVector.z);
                normalTexture.SetPixel(x, y, newPixel);
            }
        }
        normalTexture.Apply();
        return normalTexture;
    }
}