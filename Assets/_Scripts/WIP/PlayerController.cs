using UnityEngine;

//The brain of the player. FACADE TO ALL PLAYER BEHAVIOUR SCRIPTS
public class PlayerController : MonoBehaviour
{
    public InputReader InputReader { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Vector3 CurrentVelocity { get; private set; }
    public bool IsGrounded { get => _groundSensor.CheckForGround(); }
    public float SpeedLimitMult = 1;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform movementOrientation;
    [SerializeField] private PlayerCamera playerCamera;

    private PlayerSwing _playerSwing;
    private PlayerMovement _playerMovement;
    private GroundSensor _groundSensor;

    void Awake()
    {
        InitializeInputs();
        _groundSensor = new GroundSensor(transform, groundLayer);
        InputReader = GetComponent<InputReader>();
        Rigidbody = GetComponent<Rigidbody>();
        _playerMovement = new PlayerMovement(movementOrientation, Rigidbody);
        _playerSwing = GetComponent<PlayerSwing>();
    }

    private void Update()
    {
        CurrentVelocity = Rigidbody.linearVelocity;
        playerCamera.HandleCameraMovement(LookInput);
        GetInput();
        _playerSwing.CheckForSwingPoints();

    }

    private void InitializeInputs()
    {
        //Inputs logic goes here
    }

    private void GetInput()
    {
        MoveInput = InputReader.MovementInput;
        LookInput = InputReader.LookInput;
    }

    public void SetMoveData(PlayerMovementData data)
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


}
