using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FavoriteAsset))]
public class FavoriteAssetEditor : Editor
{
    public void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        FavoriteAsset asset = (FavoriteAsset)target;

        if (asset.target != null)
        {
            EditorGUILayout.ObjectField("Target", asset.target, typeof(Object), false);
            asset.selectTargetOnClick = EditorGUILayout.Toggle("Select Target on Click", asset.selectTargetOnClick);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            Texture2D preview1 = AssetPreview.GetAssetPreview(target) ?? AssetPreview.GetMiniThumbnail(target);
            if (GUILayout.Button(preview1, GUILayout.Width(64), GUILayout.Height(64)))
            {
                EditorGUIUtility.PingObject(target);
            }
            GUILayout.Label("Source");
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            Texture2D preview = AssetPreview.GetAssetPreview(asset.target) ?? AssetPreview.GetMiniThumbnail(asset.target);
            if (GUILayout.Button(preview, GUILayout.Width(64), GUILayout.Height(64)))
            {
                if (asset.selectTargetOnClick)
                {
                    Selection.activeObject = asset.target;
                }
                EditorGUIUtility.PingObject(asset.target);
            }
            GUILayout.Label("Target");

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Open Asset", GUILayout.Width(128), GUILayout.Height(64)))
            {
                AssetDatabase.OpenAsset(asset.target);
            }
            EditorGUILayout.EndHorizontal();


            //if (asset.selectTargetOnClick)
            //{
            //    Object selected = Selection.activeObject;
            //    if (selected != null && selected == target)
            //    {
            //        Selection.activeObject = asset.target;
            //        EditorGUIUtility.PingObject(asset.target);
            //    }
            //}

        }
        else
        {
            EditorGUILayout.HelpBox("No target assigned.", MessageType.Warning);
        }
    }
}

