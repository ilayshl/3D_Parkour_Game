using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    [SerializeField] private Transform playerOrientation, playerLook;

    private float _xRotation;
    private float _yRotation;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

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
