using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FavoritesBrowser : EditorWindow
{
    private Vector2 scroll;
    private List<FavoriteAsset> favoriteAssets = new();

    [MenuItem("Window/Favorites Browser")]
    public static void ShowWindow()
    {
        GetWindow<FavoritesBrowser>("Favorites");
    }

    private void OnEnable()
    {
        LoadFavorites();
    }

    private void OnGUI()
    {
        DrawDropZone();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        int columnCount = Mathf.FloorToInt(position.width / 100f);
        int index = 0;

        EditorGUILayout.BeginVertical();
        while (index < favoriteAssets.Count)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < columnCount && index < favoriteAssets.Count; i++, index++)
            {
                DrawFavoriteThumbnail(favoriteAssets[index]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void DrawDropZone()
    {
        GUILayout.Label("Drag Assets Here", EditorStyles.boldLabel);
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop assets or folders here");

        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && dropArea.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (Object obj in DragAndDrop.objectReferences)
                {
                    AddAssetToFavorites(obj);
                }
                LoadFavorites(); // refresh list
            }
            evt.Use();
        }
    }

    private void DrawFavoriteThumbnail(FavoriteAsset fav)
    {
        Object target = fav.target;
        EditorGUILayout.BeginVertical(GUILayout.Width(90));
        if (target != null)
        {
            Texture2D preview = AssetPreview.GetAssetPreview(target) ?? AssetPreview.GetMiniThumbnail(target);
            if (GUILayout.Button(preview, GUILayout.Width(64), GUILayout.Height(64)))
            {
                Selection.activeObject = target;
                EditorGUIUtility.PingObject(target);
            }

            GUILayout.Label(target.name, EditorStyles.miniLabel);
        }
        else
        {
            GUILayout.Label("Missing Reference");
        }

        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            string path = AssetDatabase.GetAssetPath(fav);
            AssetDatabase.DeleteAsset(path);
            LoadFavorites();
        }

        EditorGUILayout.EndVertical();
    }

    private void AddAssetToFavorites(Object obj)
    {
        if (obj == null) return;

        string favoritesFolder = "Assets/Favorites";
        if (!AssetDatabase.IsValidFolder(favoritesFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Favorites");
        }

        string assetName = obj.name.Replace(" ", "_");
        string path = AssetDatabase.GenerateUniqueAssetPath($"{favoritesFolder}/Fav_{assetName}.asset");

        FavoriteAsset fav = ScriptableObject.CreateInstance<FavoriteAsset>();
        fav.target = obj;

        AssetDatabase.CreateAsset(fav, path);
        AssetDatabase.SaveAssets();
    }

    private void LoadFavorites()
    {
        favoriteAssets.Clear();
        string[] guids = AssetDatabase.FindAssets("t:FavoriteAsset", new[] { "Assets/Favorites" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            FavoriteAsset fav = AssetDatabase.LoadAssetAtPath<FavoriteAsset>(path);
            if (fav != null)
            {
                favoriteAssets.Add(fav);
            }
        }
    }
}