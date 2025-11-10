using System.Collections;
using UnityEngine;

//The brain of the player. FACADE TO ALL PLAYER BEHAVIOUR SCRIPTS
public class PlayerManager : MonoBehaviour, IPlayerManager
{
    public PlayerState CurrentState => _stateMachine.CurrentState;
    public InputReader InputReader { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Vector3 CurrentVelocity { get; private set; }
    public PlayerCamera LookCamera => lookCamera;
    public bool IsGrounded => _groundSensor.CheckForGround();
    [SerializeField] private PlayerSwingData swingData;
    //[SerializeField] private float movementSpeed = 10f; //Should it be here?
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerMovementOrientation movementOrientation;
    [SerializeField] private PlayerCamera lookCamera;
    private StateMachine _stateMachine;
    private PlayerSwing _playerSwing;
    private PlayerMovement _playerMovement;
    private GroundSensor _groundSensor;
    private PlayerStateFactory _factory;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        InitializeInputs();
        InitializeMovementLogic();
        InitializeAnyTransitionStates();
    }

    private void Start()
    {
        _stateMachine.Start();
    }

    private void Update()
    {
        CurrentVelocity = Rigidbody.linearVelocity;
        LookCamera.HandleCameraMovement(LookInput);
        GetInput();
        _playerSwing.CheckForSwingPoints();
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    void OnDestroy()
    {
        InputReader.Disable();
    }

    private void InitializeMovementLogic()
    {
        _stateMachine = new(this);
        _factory = _stateMachine.Factory;
        _playerSwing = new(swingData, Rigidbody);
        _playerMovement = new(movementOrientation, Rigidbody);
        _groundSensor = new(transform, groundLayer);
    }

    private void InitializeInputs()
    {
        InputReader = new();
        InputReader.Initiate();
    }

    private void InitializeAnyTransitionStates()
    {
        _stateMachine.AddAnyTransition(new StatePredicate(_factory.Swing(), () => InputReader.IsSwinging && _playerSwing.IsReady));
        _stateMachine.AddAnyTransition(new StatePredicate(_factory.Dash(), () => InputReader.IsUsingAbility));
    }

    private void GetInput()
    {
        MoveInput = InputReader.MovementInput;
        LookInput = InputReader.LookInput;
    }

    
}
