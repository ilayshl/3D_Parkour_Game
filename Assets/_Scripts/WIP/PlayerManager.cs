using System.Collections;
using Unity.VisualScripting;
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
    public RopeHandler RopeHandler => ropeHandler;
    public PlayerCamera LookCamera => lookCamera;
    public bool IsGrounded => _groundSensor.CheckForGround();
    public float SpeedLimitMult = 1;
    //[SerializeField] private PlayerSwingData swingData; //Couldn't set a ScriptableObject in the SerializeField section
    //[SerializeField] private float movementSpeed = 10f; //Should it be here?
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerMovementOrientation movementOrientation;
    [SerializeField] private PlayerCamera lookCamera;
    [SerializeField] private RopeHandler ropeHandler; //To remove from here in the future and move it to PlayerSwingData.
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

    void Start()
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
        Debug.Log($"Instantiated StateMachine {_stateMachine.ToString()} and Factory {_factory.ToString()}");
        _playerSwing = new(this, new PlayerSwingData(), Rigidbody);
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
        _stateMachine.AddAnyTransition(new StatePredicate(_factory.Swing(), () => InputReader.IsSwinging));
    }

    private void GetInput()
    {
        MoveInput = InputReader.MovementInput;
        LookInput = InputReader.LookInput;
    }

    public void SetMovementData(PlayerMovementData data)
    {
        _playerMovement.MoveSpeedLimitMult = data.SpeedLimitMult;
        Rigidbody.linearDamping = data.Damp;
        _playerMovement.MoveSpeedMult = data.MoveSpeedMult;
        Debug.Log($"Move Speed Limit Mult = {data.SpeedLimitMult}\nDamping = {data.Damp}\nMove Speed Mult = {data.MoveSpeedMult}");
    }

    public void HandleMove()
    {
        _playerMovement.Move(MoveInput);
        _playerMovement.LimitSpeed();
    }

    public void HandleSwingMove()
    {
        _playerSwing.SwingMove(MoveInput);
    }
    
    public void HandleJump()
    {
        _playerMovement.Jump();
        //Invoke(nameof(_playerMovement.ResetJump), .25f); Unused, meant to prevent double-jumps
    }

    public void HandleSwing()
    {
        if(_playerSwing.CheckSwing())
        {
            
        }
    }

    private IEnumerator CheckForSwingPoints()
    {
        while(!_playerSwing.IsSwinging)
        {
            _playerSwing.CheckForSwingPoints();
            yield return null;
        }
    }
}
