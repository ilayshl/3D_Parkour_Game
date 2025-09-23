using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovementStateReceiver
{
    [Header("Movement")]
    public Vector2 currentVelocity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;

    [Header("Jump")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool _readyToJump = true;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    private bool _isGrounded;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 moveDirection;
    private Coroutine _checkInput;
    private Coroutine _move;

    private Rigidbody _rb;
    private IMovementManager _playerManager;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerManager = GetComponent<IMovementManager>();
    }

    void OnEnable()
    {
        _playerManager.OnMovementStateChanged += OnMovementStateChanged;
    }

    void OnDisable()
    {
        _playerManager.OnMovementStateChanged -= OnMovementStateChanged;
    }

    void Start()
    {
        _playerManager.ChangeMovementState();
    }

    public void OnMovementStateChanged(MovementState newState)
    {
        if (newState == MovementState.Walking)
        {
            Debug.Log("Recieved state as Walking");
            _checkInput = StartCoroutine(nameof(CheckInput));
            _move = StartCoroutine(nameof(Move));
        }

    }

    private IEnumerator CheckInput()
    {
        Debug.Log("CheckInput of PlayerMovement");
        while (_playerManager.State == MovementState.Walking)
        {
            GroundCheck();
            GetInput();
            currentVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
            LimitSpeed();
            yield return null;
        }

        _checkInput = null;
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && _isGrounded && _readyToJump)
        {
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundLayer);
        _rb.linearDamping = _isGrounded ? groundDrag : 0;
    }

    private void LimitSpeed()
    {
        if (currentVelocity.magnitude > moveSpeed)
        {
            Vector2 limitVelocity = currentVelocity.normalized * moveSpeed;
            _rb.linearVelocity = new Vector3(limitVelocity.x, _rb.linearVelocity.y, limitVelocity.y);
        }
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        _readyToJump = false;
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private IEnumerator Move()
    {
        Debug.Log("Move of PlayerMovement");
        while (_playerManager.State == MovementState.Walking)
        {
            moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
            Vector3 forceToAdd = moveDirection.normalized * moveSpeed * 10f;
            _rb.AddForce(_isGrounded ? forceToAdd : forceToAdd * airMultiplier, ForceMode.Force);

            yield return new WaitForFixedUpdate();
        }

        _move = null;
    }
}


