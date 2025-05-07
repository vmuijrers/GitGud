using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1;

    public void ChangeScale()
    {
        transform.localScale *= scaleMultiplier;
    }
}