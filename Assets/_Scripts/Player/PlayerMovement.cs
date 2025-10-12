using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovementStateReceiver
{
    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField] private Transform movementOrientation;
    [SerializeField] private float airDrag;

    private bool _readyToJump = true;

    private Vector3 moveDirection;
    private Coroutine _checkInput,_move, _groundCheck;

    private bool _isGrounded;
    private float _speedLimitMult = 1;

    private PlayerData _playerData;
    private IMovementManager _playerManager;

    void Awake()
    {
        _playerData = GetComponent<PlayerData>();
        _playerManager = GetComponent<IMovementManager>();
    }

    private void OnEnable()
    {
        _playerManager.OnMovementStateChanged += OnMovementStateChanged;
    }

    void OnDisable()
    {
        _playerManager.OnMovementStateChanged -= OnMovementStateChanged;
    }

    void Start()
    {
        _playerManager.ChangeMovementState(MovementState.Walking);
    }

    void Update()
    {
        CurrentVelocity = new Vector2(_playerData.rb.linearVelocity.x, _playerData.rb.linearVelocity.z);
    }

    public void OnMovementStateChanged(MovementState newState)
    {
        if (newState == MovementState.Walking)
        {
            _playerData.rb.linearDamping = _playerData.GroundDrag;
            _move = StartCoroutine(nameof(Move));
            _groundCheck = StartCoroutine(nameof(CheckForGround));
            _checkInput = StartCoroutine(nameof(CheckInput));
        }

    }

    private IEnumerator CheckInput()
    {
        while (_playerManager.State == MovementState.Walking)
        {
            GetInput();
            LimitSpeed();
            yield return null;
        }

        _checkInput = null;
    }

    private void GetInput()
    {
        if (Input.GetKey(_playerData.JumpInput) && _readyToJump)
        {
            Jump();
            Invoke(nameof(ResetJump), _playerData.JumpCooldown);
        }
    }

    private IEnumerator CheckForGround()
    {
        yield return new WaitForSeconds(_playerData.JumpCooldown);
        while (_playerManager.State == MovementState.Walking)
        {
            if (!Physics.CheckSphere(transform.position, 0.1f, _playerData.GroundLayer))
            {
                _playerManager.ChangeMovementState(MovementState.Airborne);
            }
            yield return null;
        }

        _groundCheck = null;
    }

    private void LimitSpeed()
    {
        if (CurrentVelocity.magnitude > _playerData.MoveSpeed)
        {
            Vector2 limitVelocity = CurrentVelocity.normalized * _playerData.MoveSpeed * _speedLimitMult;
            _playerData.rb.linearVelocity = new Vector3(limitVelocity.x, _playerData.rb.linearVelocity.y, limitVelocity.y);
        }
    }

    private void Jump()
    {
        _playerData.rb.linearVelocity = new Vector3(_playerData.rb.linearVelocity.x, 0, _playerData.rb.linearVelocity.z);
        _playerData.rb.AddForce(transform.up * _playerData.JumpForce, ForceMode.Impulse);
        _readyToJump = false;
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private IEnumerator Move()
    {
        while (_playerManager.State == MovementState.Walking)
        {
            moveDirection = movementOrientation.forward * Input.GetAxisRaw("Vertical") + movementOrientation.right * Input.GetAxisRaw("Horizontal");
            Vector3 forceToAdd = moveDirection.normalized * _playerData.MoveSpeed * 10;
            forceToAdd = _isGrounded ? forceToAdd : forceToAdd * _playerData.GroundDrag;
            _playerData.rb.AddForce(forceToAdd, ForceMode.Force);

            yield return new WaitForFixedUpdate();
        }

        _move = null;
    }
}


