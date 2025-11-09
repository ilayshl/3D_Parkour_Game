using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Range(1f, 50f)] public float sensitivityX, sensitivityY = 15f;

    [SerializeField] private CameraAnchor playerCameraAnchor;
    [SerializeField] private PlayerMovementOrientation playerOrientation;

    private float _xRotation;
    private float _yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HandleCameraMovement(Vector2 lookInput)
    {
        GetInput(lookInput.x, lookInput.y);
        RotateCamera();
    }

    private void GetInput(float xInput, float yInput)
    {
        //Getting input (new system)
        float mouseX = xInput * sensitivityX * 0.005f;
        float mouseY = yInput * sensitivityY * 0.005f;

        //Moving the rotation values
        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    }

    private void RotateModel()
    {
        //playerModel.transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void RotateCamera()
    {
        Vector2 direction = new Vector2(_xRotation, _yRotation);
        playerCameraAnchor.LookTowards(Quaternion.Euler(direction.x, direction.y, 0));
        playerOrientation.LookTowards(Quaternion.Euler(0, _yRotation, 0));
    }

}
