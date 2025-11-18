using UnityEngine;

/// <summary>
/// The brain of the player, acts as a single entry point starting script.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    //For debug canvas
    public PlayerState CurrentState => _stateMachine.CurrentState;
    public Vector3 CurrentVelocity { get => _rb.linearVelocity; }
    [SerializeField] private Weapon weapon;
    [SerializeField] private PlayerSwingData swingData;
    [SerializeField] private IAbility equippedAbility;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerMovementOrientation movementOrientation;
    [SerializeField] private PlayerCamera lookCamera;
    private InputReader _input;
    private Rigidbody _rb;
    private PlayerControllerFacade _controllerFacade;
    private StateMachine _stateMachine;
    private PlayerStateFactory _factory;
    private PlayerShootLogic _shoot;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        InitializeInputs();
        InitializeMovementFacade();
        InitializeStatesLogic();
        InitializeAnyTransitionStates();
        InitializeWeapon();
    }

    private void Start()
    {
        _stateMachine.Start();
    }

    private void Update()
    {
        lookCamera.HandleCameraMovement(_input.LookInput);
        _stateMachine.Update();
        _controllerFacade.Update();
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
        _controllerFacade = new(_rb, _input);
        _controllerFacade.InitializeMovement(movementOrientation, _rb, groundLayer);
        _controllerFacade.InitializeSwing(swingData, _rb);
        _controllerFacade.InitializeAbility(equippedAbility);
    }

    private void InitializeStatesLogic()
    {
        _stateMachine = new(_controllerFacade);
        _factory = _stateMachine.Factory;
    }

    private void InitializeInputs()
    {
        _input = new();
        _input.Initiate();
    }

    private void InitializeAnyTransitionStates()
    {
        _stateMachine.AddAnyTransition(_factory.Swing(), () => _input.IsSwinging && _controllerFacade.CheckSwing());
        //_stateMachine.AddAnyTransition(_factory.Dash(), () => _input.IsUsingAbility);
    }

    private void InitializeWeapon()
    {
        _shoot = new(weapon, lookCamera.transform);
    }

    void OnEnable()
    {
        _input.ShootEvent += CheckShootRemoveThisLater;
    }

    void OnDisable()
    {
        _input.ShootEvent -= CheckShootRemoveThisLater;
    }

    private void CheckShootRemoveThisLater()
    {
        _shoot.Shoot();
    }
}
