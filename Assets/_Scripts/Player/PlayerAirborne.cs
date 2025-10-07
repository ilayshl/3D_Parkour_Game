using System.Collections;
using UnityEngine;

public class PlayerAirborne : MonoBehaviour
{
    [SerializeField] private Transform movementOrientation;
    private PlayerData _playerData;
    private IMovementManager _playerManager;

    private Coroutine groundedCheck;
    private Coroutine moveAirborne;

    void Awake()
    {
        _playerData = GetComponent<PlayerData>();
        _playerManager = GetComponent<IMovementManager>();
    }

    void OnEnable()
    {
        _playerManager.OnMovementStateChanged += OnMovementStateChange;
    }

    void OnDisable()
    {
        _playerManager.OnMovementStateChanged -= OnMovementStateChange;
    }

    private void OnMovementStateChange(MovementState newState)
    {
        if (newState == MovementState.Airborne)
        {
            _playerData.rb.linearDamping = _playerData.AirDrag;
            groundedCheck = StartCoroutine(nameof(CheckForGround));
            moveAirborne = StartCoroutine(nameof(MoveAirborne));
        }
    }

    private IEnumerator CheckForGround()
    {
        yield return new WaitForSeconds(_playerData.JumpCooldown);
        while (_playerManager.State == MovementState.Airborne)
        {
            if (Physics.CheckSphere(transform.position, 0.1f, _playerData.GroundLayer))
            {
                _playerManager.ChangeMovementState(MovementState.Walking);
            }
            yield return null;
        }

        groundedCheck = null;
    }

    private IEnumerator MoveAirborne()
    {
        while (_playerManager.State == MovementState.Airborne)
        {
            Vector3 currentInput = movementOrientation.forward * Input.GetAxisRaw("Vertical") + movementOrientation.right * Input.GetAxisRaw("Horizontal");
            Vector3 forceToAdd = currentInput.normalized * _playerData.MoveSpeed * 10f;
            _playerData.rb.AddForce(forceToAdd * _playerData.AirMultiplier, ForceMode.Force);
            yield return new WaitForFixedUpdate();
        }
        moveAirborne = null;
    }
}
