using System.Collections;
using UnityEngine;

//The brain of the player. FACADE TO ALL PLAYER BEHAVIOUR SCRIPTS
public class PlayerManager : MonoBehaviour
{
    public PlayerState CurrentState => _stateMachine.CurrentState;
    private InputReader InputReader;
    public Vector2 LookInput { get => InputReader.LookInput; }
    public Vector3 CurrentVelocity { get => _rb.linearVelocity; }
    public PlayerCamera LookCamera => lookCamera;
    public bool IsGrounded => _groundSensor.CheckForGround();
    [SerializeField] private PlayerSwingData swingData;
    [SerializeField] private IAbility equippedAbility;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerMovementOrientation movementOrientation;
    [SerializeField] private PlayerCamera lookCamera;
    private Rigidbody _rb;
    private StateMachine _stateMachine;
    private PlayerMovementManager _movementManager;
    private GroundSensor _groundSensor;
    private PlayerStateFactory _factory;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        InitializeInputs();
        InitializeMovementManager();
        InitializeMovementLogic();
        InitializeAnyTransitionStates();
    }

    private void Start()
    {
        _stateMachine.Start();
        InitializeSwingPoint();
    }

    private void Update()
    {
        LookCamera.HandleCameraMovement(LookInput);
        GetInput();
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    void OnDestroy()
    {
        InputReader.Disable();
        _movementManager.SwingUpdateEvent -= HandleHitPrediction;
    }

    private void InitializeMovementManager()
    {
        _movementManager = new(_rb);
        _movementManager.InitializeMovement(movementOrientation, _rb);
        _movementManager.InitializeSwing(swingData, _rb);
        _movementManager.InitializeAbility(equippedAbility);
        _movementManager.SwingUpdateEvent += HandleHitPrediction;
    }

    private void InitializeMovementLogic()
    {
        _stateMachine = new(this);
        _factory = _stateMachine.Factory;
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

    private void InitializeSwingPoint()
    {
        //StartCoroutine(nameof(CheckForSwingPoints));
    }

    private void GetInput()
    {
        _movementManager.MoveInput = InputReader.MovementInput;
    }

    private void HandleHitPrediction()
    {
        
    }
    
    
    /* private IEnumerator CheckForSwingPoints()
    {
        while (!_playerSwing.IsSwinging)
        {
            _playerSwing.CheckForSwingPoints();
            yield return null;
        }
    } */
}
