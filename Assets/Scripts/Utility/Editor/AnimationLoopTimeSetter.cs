using UnityEngine;
using UnityEditor;

public class AnimationLoopTimeSetterEditor : EditorWindow
{
    [MenuItem("Tools/Set Animation Loop Time")]
    public static void ShowWindow()
    {
        GetWindow<AnimationLoopTimeSetterEditor>("Animation Loop Time Setter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Loop Time for FBX Animations", EditorStyles.boldLabel);

        if (GUILayout.Button("Select FBX File"))
        {
            string initialDirectory = Application.dataPath;
            string fbxPath = EditorUtility.OpenFilePanel("Select FBX File", initialDirectory, "fbx");
            
            if (!string.IsNullOrEmpty(fbxPath))
            {
                // Convert absolute path to Unity relative asset path
                string relativePath = ConvertToRelativeAssetPath(fbxPath);
                
                SetAnimationLoopTime(relativePath);
            }
        }
    }

    private void SetAnimationLoopTime(string relativePath)
    {
        // Load the ModelImporter for the FBX file
        ModelImporter importer = AssetImporter.GetAtPath(relativePath) as ModelImporter;

        if (importer == null)
        {
            Debug.LogError($"Could not load ModelImporter for {relativePath}");
            return;
        }

        // Ensure import settings are set to extract clips
        importer.importAnimation = true;

        // Check if there are default clips
        if (importer.defaultClipAnimations == null || importer.defaultClipAnimations.Length == 0)
        {
            Debug.LogWarning($"No default animation clips found in the FBX");
            return;
        }

        // Create new clip animations array
        ModelImporterClipAnimation[] newClipAnimations = new ModelImporterClipAnimation[importer.defaultClipAnimations.Length];

        // Set loop time for all clips
        for (int i = 0; i < importer.defaultClipAnimations.Length; i++)
        {
            ModelImporterClipAnimation originalClip = importer.defaultClipAnimations[i];
            ModelImporterClipAnimation newClip = new ModelImporterClipAnimation();

            // Copy all properties from the original clip
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
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Could not copy property {prop.Name}: {ex.Message}");
                }
            }

            // Explicitly set loop time to true
            newClip.loopTime = true;

            newClipAnimations[i] = newClip;
        }

        // Apply the new clip animations
        importer.clipAnimations = newClipAnimations;

        try
        {
            // Save the changes
            importer.SaveAndReimport();
            Debug.Log($"Successfully set loop time for all clips in {relativePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error setting loop time for {relativePath}: {ex.Message}");
        }
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
