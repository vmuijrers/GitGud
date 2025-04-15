using UnityEngine;
using UnityEditor;
using System.IO;

public static class FavoriteAssetUtility
{
    [MenuItem("Assets/Add to Favorites", true)]
    private static bool ValidateAddToFavorites()
    {
        return Selection.activeObject != null;
    }

    [MenuItem("Assets/Add to Favorites")]
    private static void AddToFavorites()
    {
        Object selected = Selection.activeObject;
        string favoritesFolder = "Assets/Favorites";

        if (!AssetDatabase.IsValidFolder(favoritesFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Favorites");
        }

        string name = selected.name.Replace(" ", "_");
        string path = $"{favoritesFolder}/Fav_{name}.asset";

        FavoriteAsset fav = ScriptableObject.CreateInstance<FavoriteAsset>();
        fav.target = selected;

        AssetDatabase.CreateAsset(fav, AssetDatabase.GenerateUniqueAssetPath(path));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = fav;
    }
}

