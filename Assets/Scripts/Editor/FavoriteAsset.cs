using UnityEngine;

[CreateAssetMenu(menuName = "Favorites/Favorite Asset")]
public class FavoriteAsset : ScriptableObject
{
    public Object target;
    public bool selectTargetOnClick = true;
}