using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Color color;

    public void ChangeColor()
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
