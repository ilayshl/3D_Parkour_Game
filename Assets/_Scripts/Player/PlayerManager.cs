using UnityEngine;

/// <summary>
/// The brain of the player, acts as a single entry point starting script.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public PlayerState CurrentState => _stateMachine.CurrentState;
    public Vector2 LookInput { get => _input.LookInput; }
    public Vector3 CurrentVelocity { get => _rb.linearVelocity; }
    public PlayerCamera LookCamera => lookCamera;
    [SerializeField] private PlayerSwingData swingData;
    [SerializeField] private IAbility equippedAbility;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerMovementOrientation movementOrientation;
    [SerializeField] private PlayerCamera lookCamera;
    private InputReader _input;
    private Rigidbody _rb;
    private StateMachine _stateMachine;
    private PlayerControllerFacade _movementFacade;
    private PlayerStateFactory _factory;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        InitializeInputs();
        InitializeMovementFacade();
        InitializeStatesLogic();
        InitializeAnyTransitionStates();
    }

    private void Start()
    {
        _stateMachine.Start();
    }

    private void Update()
    {
        LookCamera.HandleCameraMovement(LookInput);
        _stateMachine.Update();
        _movementFacade.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    void OnDestroy()
    {
        _input.Disable();
    }

    private void InitializeMovementFacade()
    {
        _movementFacade = new(_rb, _input);
        _movementFacade.InitializeMovement(movementOrientation, _rb, groundLayer);
        _movementFacade.InitializeSwing(swingData, _rb);
        _movementFacade.InitializeAbility(equippedAbility);
    }

    private void InitializeStatesLogic()
    {
        _stateMachine = new(_movementFacade);
        _factory = _stateMachine.Factory;
    }

    private void InitializeInputs()
    {
        _input = new();
        _input.Initiate();
    }

    private void InitializeAnyTransitionStates()
    {
        _stateMachine.AddAnyTransition(_factory.Swing(), () => _input.IsSwinging && _movementFacade.CheckSwing());
        _stateMachine.AddAnyTransition(_factory.Dash(), () => _input.IsUsingAbility);
    }
}
