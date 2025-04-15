using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
public class FavoritesWindow : EditorWindow
{
    public ResourceCollection resourceCollection;
    private Vector2 scroll;

    private string newCategoryName = "New Category";

    [MenuItem("Window/Favorites")]
    public static void ShowWindow()
    {
        GetWindow<FavoritesWindow>("Favorites");
    }

    private void OnGUI()
    {
        resourceCollection = EditorGUILayout.ObjectField(resourceCollection, typeof(ResourceCollection), false) as ResourceCollection;
        if (resourceCollection == null) { return; }

        scroll = EditorGUILayout.BeginScrollView(scroll);
        List<ResourceList> categories = new List<ResourceList>(resourceCollection.resources);
        foreach (ResourceList category in categories)
        {
            
            EditorGUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            category.isFoldout = EditorGUILayout.Foldout(category.isFoldout, category.name, true);
            GUILayout.FlexibleSpace(); // Pushes the X to the right
            if (GUILayout.Button("X"))
            {
                resourceCollection.resources.Remove(category);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }
            GUILayout.EndHorizontal();
            Rect foldoutRect = GUILayoutUtility.GetLastRect();
            HandleCategoryDrop(category.name, foldoutRect);

            if (category.isFoldout)
            {
                List<Object> favorites = category.favorites;
                for (int i = 0; i < favorites.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    favorites[i] = EditorGUILayout.ObjectField(favorites[i], typeof(Object), false);

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        favorites.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"Drag assets here to add them (adds to '{newCategoryName}'):");
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop here");

        HandleDragAndDrop(dropArea, newCategoryName);

        EditorGUILayout.Space();
        DrawCategoryCreationUI();
    }

    private void DrawCategoryCreationUI()
    {
        EditorGUILayout.BeginHorizontal();
        newCategoryName = EditorGUILayout.TextField(newCategoryName);

        if (GUILayout.Button("Add Category"))
        {
            if(resourceCollection.GetResourceListByCategory(newCategoryName) == null)
            {
                resourceCollection.AddCategory(newCategoryName);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void HandleDragAndDrop(Rect dropArea, string targetCategory)
    {
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && dropArea.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                if(!resourceCollection.resources.Exists(x=>x.name == targetCategory))
                {
                    resourceCollection.AddCategory(targetCategory);
                }

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(draggedObject);
                    
                    if (AssetDatabase.IsValidFolder(path))
                    {
                        string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { path });
                        //ShowFolderContents(draggedObject.GetInstanceID());

                        foreach (string guid in assetGUIDs)
                        {
                            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                            if (asset != null && !resourceCollection.GetResourceListByCategory(targetCategory).favorites.Contains(asset))
                            {
                                resourceCollection.GetResourceListByCategory(targetCategory).favorites.Add(asset);
                            }
                        }
                    }
                    else
                    {
                        if (!resourceCollection.GetResourceListByCategory(targetCategory).favorites.Contains(draggedObject))
                        {
                            resourceCollection.GetResourceListByCategory(targetCategory).favorites.Add(draggedObject);
                        }
                    }
                }
            }

            evt.Use();
        }
    }

    private void HandleCategoryDrop(string category, Rect categoryRect)
    {
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && categoryRect.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                HandleDragAndDrop(categoryRect, category);
            }

            evt.Use();
        }
    }

    /// <summary>
    /// Selects a folder in the project window and shows its content.
    /// Opens a new project window, if none is open yet.
    /// </summary>
    /// <param name="folderInstanceID">The instance of the folder asset to open.</param>
    private static void ShowFolderContents(int folderInstanceID)
    {
        // Find the internal ProjectBrowser class in the editor assembly.
        Assembly editorAssembly = typeof(Editor).Assembly;
        System.Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");

        // This is the internal method, which performs the desired action.
        // Should only be called if the project window is in two column mode.
        MethodInfo showFolderContents = projectBrowserType.GetMethod(
            "ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);

        // Find any open project browser windows.
        Object[] projectBrowserInstances = Resources.FindObjectsOfTypeAll(projectBrowserType);

        if (projectBrowserInstances.Length > 0)
        {
            for (int i = 0; i < projectBrowserInstances.Length; i++)
                ShowFolderContentsInternal(projectBrowserInstances[i], showFolderContents, folderInstanceID);
        }
        else
        {
            EditorWindow projectBrowser = OpenNewProjectBrowser(projectBrowserType);
            ShowFolderContentsInternal(projectBrowser, showFolderContents, folderInstanceID);
        }
    }

    private static void ShowFolderContentsInternal(Object projectBrowser, MethodInfo showFolderContents, int folderInstanceID)
    {
        // Sadly, there is no method to check for the view mode.
        // We can use the serialized object to find the private property.
        SerializedObject serializedObject = new SerializedObject(projectBrowser);
        bool inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;

        if (!inTwoColumnMode)
        {
            // If the browser is not in two column mode, we must set it to show the folder contents.
            MethodInfo setTwoColumns = projectBrowser.GetType().GetMethod(
                "SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
            setTwoColumns.Invoke(projectBrowser, null);
        }

        bool revealAndFrameInFolderTree = true;
        showFolderContents.Invoke(projectBrowser, new object[] { folderInstanceID, revealAndFrameInFolderTree });
    }

    private static EditorWindow OpenNewProjectBrowser(System.Type projectBrowserType)
    {
        EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);
        projectBrowser.Show();

        // Unity does some special initialization logic, which we must call,
        // before we can use the ShowFolderContents method (else we get a NullReferenceException).
        MethodInfo init = projectBrowserType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
        init.Invoke(projectBrowser, null);

        return projectBrowser;
    }
}