using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class SceneManagerPro : EditorWindow
{
    private Vector2 _scrollPos;
    private string _searchText = "";
    private float _contentHeight;
    private const float MaxWindowHeight = 600f;

    [MenuItem("Tools/Scene Manager Pro")]
    public static void ShowWindow() => GetWindow<SceneManagerPro>("Scene Manager").Show();

    private void OnGUI()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        string[] allSceneGuids = AssetDatabase.FindAssets("t:Scene");
        SceneAsset startScene = EditorSceneManager.playModeStartScene;
        int loadedSceneCount = EditorSceneManager.sceneCount;

        // --- Toolbar ---
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        _searchText = EditorGUILayout.TextField(_searchText, EditorStyles.toolbarSearchField);
        
        if (GUILayout.Button("Save All", EditorStyles.toolbarButton))
            EditorSceneManager.SaveOpenScenes();

        if (GUILayout.Button("Build Settings", EditorStyles.toolbarButton))
            EditorApplication.ExecuteMenuItem("File/Build Settings...");

        if (GUILayout.Button("Clear Lock", EditorStyles.toolbarButton))
            EditorSceneManager.playModeStartScene = null;
        EditorGUILayout.EndHorizontal();

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        Rect totalRect = EditorGUILayout.BeginVertical();

        // --- SECTION 1: IN BUILD ---
        EditorGUILayout.LabelField($"Scenes in Build ({buildScenes.Length})", EditorStyles.boldLabel);
        for (int i = 0; i < buildScenes.Length; i++)
            DrawBuildItem(buildScenes[i], i, startScene, loadedSceneCount);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // --- SECTION 2: NOT IN BUILD ---
        EditorGUILayout.LabelField("Project Scenes (Not in Build)", EditorStyles.boldLabel);
        for (int i = 0; i < allSceneGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allSceneGuids[i]);
            string name = Path.GetFileNameWithoutExtension(path);
            if (!IsInBuild(path, buildScenes) && name.ToLower().Contains(_searchText.ToLower()))
                DrawProjectItem(path, name);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        // --- Auto-Size ---
        if (Event.current.type == EventType.Repaint)
        {
            float newHeight = Mathf.Min(totalRect.height + 45, MaxWindowHeight);
            if (!Mathf.Approximately(_contentHeight, newHeight))
            {
                _contentHeight = newHeight;
                minSize = maxSize = new Vector2(450, _contentHeight);
            }
        }
    }

    private void DrawBuildItem(EditorBuildSettingsScene buildScene, int index, SceneAsset startScene, int loadedCount)
    {
        string name = Path.GetFileNameWithoutExtension(buildScene.path);
        if (!name.ToLower().Contains(_searchText.ToLower())) return;

        var runtimeScene = EditorSceneManager.GetSceneByPath(buildScene.path);
        bool isLoaded = runtimeScene.isLoaded;
        bool isStartScene = startScene != null && AssetDatabase.GetAssetPath(startScene) == buildScene.path;
        string displayName = isLoaded && runtimeScene.isDirty ? name + "*" : name;

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        
        // 1. Start Lock (Yellow Star)
        GUI.color = isStartScene ? Color.yellow : Color.white;
        if (GUILayout.Button(isStartScene ? "★" : "☆", GUILayout.Width(25)))
            EditorSceneManager.playModeStartScene = isStartScene ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path);
        GUI.color = Color.white;

        // 2. Open/Ping Scene
        // Cyan = Active/Loaded, White = Closed
        if (isLoaded) GUI.color = Color.cyan;
        if (GUILayout.Button(displayName, EditorStyles.label, GUILayout.ExpandWidth(true)))
        {
            if (isLoaded) 
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path));
            else if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Single);
        }
        GUI.color = Color.white;

        // 3. Additive Load (+) / Unload (-)
        if (!isLoaded)
        {
            if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(18)))
                EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Additive);
        }
        else
        {
            GUI.enabled = loadedCount > 1;
            if (GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(18)))
            {
                if (EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] { runtimeScene }))
                    EditorSceneManager.CloseScene(runtimeScene, true);
            }
            GUI.enabled = true;
        }

        // 4. Move/Remove
        if (GUILayout.Button("▲", GUILayout.Width(22))) Move(index, -1);
        if (GUILayout.Button("▼", GUILayout.Width(22))) Move(index, 1);
        if (GUILayout.Button("X", GUILayout.Width(22))) RemoveFromBuild(index);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawProjectItem(string path, string name)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(name, EditorStyles.label, GUILayout.ExpandWidth(true)))
        {
             if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        }
        if (GUILayout.Button("Add", GUILayout.Width(50))) AddToBuild(path);
        EditorGUILayout.EndHorizontal();
    }

    private void Move(int index, int direction)
    {
        if (index + direction < 0 || index + direction >= EditorBuildSettings.scenes.Length) return;
        var scenes = EditorBuildSettings.scenes;
        var temp = scenes[index];
        scenes[index] = scenes[index + direction];
        scenes[index + direction] = temp;
        EditorBuildSettings.scenes = scenes;
    }

    private bool IsInBuild(string path, EditorBuildSettingsScene[] scenes)
    {
        for (int i = 0; i < scenes.Length; i++)
            if (scenes[i].path == path) return true;
        return false;
    }

    private void AddToBuild(string path)
    {
        var old = EditorBuildSettings.scenes;
        var newArray = new EditorBuildSettingsScene[old.Length + 1];
        System.Array.Copy(old, newArray, old.Length);
        newArray[old.Length] = new EditorBuildSettingsScene(path, true);
        EditorBuildSettings.scenes = newArray;
    }

    private void RemoveFromBuild(int index)
    {
        var old = EditorBuildSettings.scenes;
        var newArray = new EditorBuildSettingsScene[old.Length - 1];
        for (int i = 0, j = 0; i < old.Length; i++)
            if (i != index) newArray[j++] = old[i];
        EditorBuildSettings.scenes = newArray;
    }
}