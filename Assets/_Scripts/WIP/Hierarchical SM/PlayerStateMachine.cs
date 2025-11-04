using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState CurrentState { get => _currentState; }
    [SerializeField] private PlayerData data;

    private PlayerController _playerController;
    private PlayerBaseState _currentState;
    private PlayerStateFactory _stateFactory;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _stateFactory = new PlayerStateFactory(this, _playerController);
    }

    private void Start()
    {
        ChangeState(_stateFactory.Airborne());
    }

    private void Update()
    {
        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }
}
