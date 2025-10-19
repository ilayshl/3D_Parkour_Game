/* using System.Collections;
using UnityEngine;

public class PlayerAirborne : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private float airMovementMult = 0.3f;
    [SerializeField] private Transform movementOrientation;
    private PlayerData _playerData;

    private Coroutine groundedCheck;
    private Coroutine moveAirborne;

    void Awake()
    {
        _playerData = GetComponent<PlayerData>();
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

    private void OnMovementStateChanged(MovementState newState)
    {
        if (newState == MovementState.Airborne)
        {
            _playerData.rb.linearDamping = _playerData.AirDrag;
            groundedCheck = StartCoroutine(nameof(CheckForGround));
            moveAirborne = StartCoroutine(nameof(Move));
        }
    }

    public void CheckForGround()
    {
        while (_playerManager.State == MovementState.Airborne)
        {
            if (Physics.CheckSphere(transform.position, 0.1f, _playerData.GroundLayer))
            {
                _playerManager.ChangeMovementState(MovementState.Walking);
            }
        }
        groundedCheck = null;
    }

    public void Move()
    {
        while (_playerManager.State == MovementState.Airborne)
        {
            Vector3 currentInput = movementOrientation.forward * Input.GetAxisRaw("Vertical") + movementOrientation.right * Input.GetAxisRaw("Horizontal");
            Vector3 forceToAdd = currentInput.normalized * _playerData.MoveSpeed * 10f;
            _playerData.rb.AddForce(forceToAdd * airMovementMult, ForceMode.Force);
        }
        moveAirborne = null;
    }
} */