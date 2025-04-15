using UnityEditor;
using UnityEngine;
using System.IO;

public class FavoritesWindow : EditorWindow
{
    private ResourceCollection collection;
    private Vector2 scroll;

    [MenuItem("Window/Favorites")]
    public static void ShowWindow()
    {
        GetWindow<FavoritesWindow>("Favorites");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Favorite Assets", EditorStyles.boldLabel);
        collection = EditorGUILayout.ObjectField(collection, typeof(ResourceCollection), false) as ResourceCollection;
        EditorGUILayout.Space();

        if(collection != null && collection.favorites != null)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);

            for (int i = 0; i < collection.favorites.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                collection.favorites[i] = EditorGUILayout.ObjectField(collection.favorites[i], typeof(Object), false);

                if (GUILayout.Button("Ping", GUILayout.Width(40)))
                {
                    EditorGUIUtility.PingObject(collection.favorites[i]);
                }

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    collection.favorites.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Drag assets here to add them:");
            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drop here");

            HandleDragAndDrop(dropArea);
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && dropArea.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(draggedObject);

                    if (AssetDatabase.IsValidFolder(path))
                    {
                        // Get all asset paths inside the folder (non-recursive or recursive)
                        string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { path });

                        foreach (string guid in assetGUIDs)
                        {
                            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                            if (asset != null && !collection.favorites.Contains(asset))
                            {
                                collection.favorites.Add(asset);
                            }
                        }
                    }
                    else
                    {
                        // Regular asset
                        if (!collection.favorites.Contains(draggedObject))
                        {
                            collection.favorites.Add(draggedObject);
                        }
                    }
                }
            }

            Event.current.Use();
        }
    }
}
