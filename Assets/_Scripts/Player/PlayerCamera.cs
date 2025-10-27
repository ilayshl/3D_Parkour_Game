using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Range(1f, 100f)] public float sensitivityX, sensitivityY;

    [SerializeField] private Transform playerOrientation, playerLook;
    [SerializeField] private InputReader inputReader;

    private float _xRotation;
    private float _yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInput();
        RotateCamera();
    }

    private void GetInput()
    {
        //Getting input (new system)
        float mouseX = inputReader.LookInput.x * sensitivityX * 0.01f;
        float mouseY = inputReader.LookInput.y * sensitivityY * 0.01f;

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
        playerLook.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        playerOrientation.transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

}
