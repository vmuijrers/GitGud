using UnityEngine;

public abstract class CustomMonobehaviour : MonoBehaviour, IUpdateable
{
    public abstract void OnUpdate();
}
