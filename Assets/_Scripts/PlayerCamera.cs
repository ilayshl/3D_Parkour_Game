using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    [SerializeField] private Transform playerOrientation;

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
        //Getting input (old system)
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        //Moving the rotation values
        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    }

    private void RotateCamera()
    {
        //Moving the rotation itself
        playerOrientation.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

}
