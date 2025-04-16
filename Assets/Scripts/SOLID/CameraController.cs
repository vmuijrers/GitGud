using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private float angleX, angleY;
    [SerializeField] private float mouseSensitivityY;
    [SerializeField] private float mouseSensitivityX;
    Quaternion startRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float vertMouse = Mouse.current.delta.y.value;
        float horMouse = Mouse.current.delta.x.value;

        angleX += horMouse * Time.deltaTime * mouseSensitivityY;
        angleY += vertMouse * Time.deltaTime * mouseSensitivityX;
        angleY = Mathf.Clamp(angleY, -89, 89);
        transform.rotation = startRotation * Quaternion.Euler(-angleY, angleX, 0);
    }
}
