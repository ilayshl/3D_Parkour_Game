using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovementStateReceiver
{
    public Vector2 currentVelocity;

    private bool _readyToJump = true;

    private Vector3 moveDirection;
    private Coroutine _checkInput;
    private Coroutine _move;

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

    public void OnMovementStateChanged(MovementState newState)
    {
        if (newState == MovementState.Walking)
        {
            _playerData.rb.linearDamping = _playerData.GroundDrag;
            _checkInput = StartCoroutine(nameof(CheckInput));
            _move = StartCoroutine(nameof(Move));
        }

    }

    private IEnumerator CheckInput()
    {
        while (_playerManager.State == MovementState.Walking)
        {
            GetInput();
            currentVelocity = new Vector2(_playerData.rb.linearVelocity.x, _playerData.rb.linearVelocity.z);
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

    private void LimitSpeed()
    {
        if (currentVelocity.magnitude > _playerData.MoveSpeed)
        {
            Vector2 limitVelocity = currentVelocity.normalized * _playerData.MoveSpeed;
            _playerData.rb.linearVelocity = new Vector3(limitVelocity.x, _playerData.rb.linearVelocity.y, limitVelocity.y);
        }
    }

    private void Jump()
    {
        _playerManager.ChangeMovementState();
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
            moveDirection = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
            Vector3 forceToAdd = moveDirection.normalized * _playerData.MoveSpeed * 10f;
            _playerData.rb.AddForce(forceToAdd, ForceMode.Force);

            yield return new WaitForFixedUpdate();
        }

        _move = null;
    }
}


