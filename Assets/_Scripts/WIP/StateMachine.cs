using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get => _currentState; }

    private PlayerController _playerController;
    private PlayerState _currentState;
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
        _currentState?.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate(Time.fixedDeltaTime);
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
        Debug.Log("Changed to state "+newState);
    }
}
