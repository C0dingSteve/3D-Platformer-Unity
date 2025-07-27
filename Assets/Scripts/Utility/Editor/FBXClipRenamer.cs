using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public static class EditorUtilityExtensions
{
    public static string[] OpenFilePanelMultiSelect(string title, string directory, string extension)
    {
        // Workaround since Unity's OpenFilePanel doesn't natively support multi-select
        string result = EditorUtility.OpenFilePanel(title, directory, extension);
        
        // If no file is selected, return an empty array
        if (string.IsNullOrEmpty(result))
        {
            return new string[0];
        }
        
        // If a single file is selected, return it in an array
        return new[] { result };
    }
}

public class FBXClipRenamer : EditorWindow
{
    private List<string> fbxFilePaths = new List<string>();

    [MenuItem("Tools/FBX Clip Renamer")]
    public static void ShowWindow()
    {
        GetWindow<FBXClipRenamer>("FBX Clip Renamer");
    }

    private void OnGUI()
    {
        GUILayout.Label("FBX Clip Renamer", EditorStyles.boldLabel);

        // File path selection
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add FBX Files", GUILayout.Width(150)))
        {
            string initialDirectory = Application.dataPath;
            string[] selectedPaths = EditorUtilityExtensions.OpenFilePanelMultiSelect("Select FBX Files", initialDirectory, "fbx");

            foreach (string path in selectedPaths)
            {
                if (!string.IsNullOrEmpty(path) && !fbxFilePaths.Contains(path))
                {
                    fbxFilePaths.Add(path);
                }
            }
        }

        // Folder selection button
        if (GUILayout.Button("Add Folder", GUILayout.Width(150)))
        {
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder with FBX Files", Application.dataPath, "");

            if (!string.IsNullOrEmpty(folderPath))
            {
                string[] fbxFiles = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);

                foreach (string file in fbxFiles)
                {
                    if (!fbxFilePaths.Contains(file))
                    {
                        fbxFilePaths.Add(file);
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        // Display selected files
        GUILayout.Label("Selected Files:", EditorStyles.boldLabel);
        if (fbxFilePaths.Count == 0)
        {
            GUILayout.Label("No files selected", EditorStyles.miniLabel);
        }
        else
        {
            // Scrollable list of selected files
            using (var scrollView = new EditorGUILayout.ScrollViewScope(Vector2.zero))
            {
                for (int i = fbxFilePaths.Count - 1; i >= 0; i--)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(Path.GetFileName(fbxFilePaths[i]));
                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        fbxFilePaths.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        // Rename button
        GUI.enabled = fbxFilePaths.Count > 0;
        if (GUILayout.Button("Rename Clips"))
        {
            RenameClips();
        }
        GUI.enabled = true;

        // Clear all button
        if (GUILayout.Button("Clear All Files"))
        {
            fbxFilePaths.Clear();
        }

        // Additional info
        EditorGUILayout.HelpBox($"Total Files Selected: {fbxFilePaths.Count}", MessageType.Info);
    }

    private void RenameClips()
    {
        int successCount = 0;
        int failureCount = 0;

        // Create a copy of the list to avoid modification during iteration
        List<string> processingPaths = new List<string>(fbxFilePaths);

        foreach (string fbxFilePath in processingPaths)
        {
            if (string.IsNullOrEmpty(fbxFilePath))
            {
                Debug.LogError("Skipping empty file path.");
                failureCount++;
                continue;
            }

            // Get the filename without extension
            string fileName = Path.GetFileNameWithoutExtension(fbxFilePath);
            string relativePath = ConvertToRelativeAssetPath(fbxFilePath);

            // Load the asset
            ModelImporter importer = AssetImporter.GetAtPath(relativePath) as ModelImporter;

            if (importer == null)
            {
                Debug.LogError($"Could not load the FBX file in the project: {fileName}");
                failureCount++;
                continue;
            }

            // Ensure import settings are set to extract clips
            importer.importAnimation = true;

            // Check if there are default clips
            if (importer.defaultClipAnimations == null || importer.defaultClipAnimations.Length == 0)
            {
                Debug.LogWarning($"No default animation clips found in the FBX: {fileName}");
                failureCount++;
                continue;
            }

            // Create new clip animations array
            ModelImporterClipAnimation[] newClipAnimations = new ModelImporterClipAnimation[importer.defaultClipAnimations.Length];

            // Rename clips
            for (int i = 0; i < importer.defaultClipAnimations.Length; i++)
            {
                ModelImporterClipAnimation originalClip = importer.defaultClipAnimations[i];
                ModelImporterClipAnimation newClip = new ModelImporterClipAnimation();

                // Use reflection to copy all properties
                foreach (var prop in typeof(ModelImporterClipAnimation).GetProperties())
                {
                    try 
                    {
                        if (prop.CanWrite)
                        {
                            var value = prop.GetValue(originalClip);
                            prop.SetValue(newClip, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Could not copy property {prop.Name}: {ex.Message}");
                    }
                }

                // Explicitly set the name with the filename
                newClip.name = fileName;

                newClipAnimations[i] = newClip;
            }

            // Apply the new clip animations
            importer.clipAnimations = newClipAnimations;

            try
            {
                // Save the changes
                importer.SaveAndReimport();
                successCount++;
                Debug.Log($"Successfully renamed clips for {fileName}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error renaming clips for {fileName}: {ex.Message}");
                failureCount++;
            }
        }

        // Remove processed files from the original list
        fbxFilePaths.Clear();

        // Show summary
        EditorUtility.DisplayDialog("Rename Clips",
            $"Rename process completed.\nSuccessful: {successCount}\nFailed: {failureCount}",
            "OK");
    }

    // Helper method to convert absolute path to Unity relative asset path
    private string ConvertToRelativeAssetPath(string absolutePath)
    {
        string assetsPath = Application.dataPath;
        if (absolutePath.StartsWith(assetsPath))
        {
            return "Assets" + absolutePath.Substring(assetsPath.Length);
        }
        return absolutePath;
    }
}


