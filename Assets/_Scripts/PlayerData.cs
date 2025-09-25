using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode leftSwingInput = KeyCode.Mouse0;
    [SerializeField] private KeyCode rightSwingInput = KeyCode.Mouse1;
    [SerializeField] private KeyCode shortenJointInput = KeyCode.Space;
    [SerializeField] private KeyCode jumpInput = KeyCode.Space;

    public KeyCode LeftSwingInput { get => leftSwingInput; }
    public KeyCode RightSwingInput { get => rightSwingInput; }
    public KeyCode ShortenJointInput { get => shortenJointInput; }
    public KeyCode JumpInput { get => jumpInput; }

    [Header("References")]
    [SerializeField] private LayerMask grappableLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform leftGunTip, rightGunTip, cam, swingPredictionPoint;


    public Transform LeftGunTip { get => leftGunTip; }
    public Transform RightGunTip { get => rightGunTip; }
    public Transform Cam { get => cam; }
    public Transform SwingPredictionPoint { get => swingPredictionPoint; }
    public LayerMask GrappableLayer { get => grappableLayer; }
    public LayerMask GroundLayer { get => groundLayer; }

    [Header("Component References")]
    public IMovementManager playerManager { get; private set; }
    public Rigidbody rb { get; private set; }
    public LineRenderer lr { get; private set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float groundDrag = 5;
    [SerializeField] private float airDrag = 0.01f;
    [SerializeField] private float swingSpeedLimitMult = 4f;

    public float MoveSpeed { get => moveSpeed; }
    public float GroundDrag { get => groundDrag; }
    public float AirDrag { get => airDrag; }
    public float SwingSpeedLimitMult { get => swingSpeedLimitMult; }

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;

    public float JumpForce { get => jumpForce; }
    public float JumpCooldown { get => jumpCooldown; }
    public float AirMultiplier { get => airMultiplier; }

    [Header("Swinging")]
    [SerializeField] private float maxSwingDistance = 25f;
    [SerializeField] private float horizontalThrustForce = 2000;
    [SerializeField] private float forwardThrustForce = 3000;
    [SerializeField] private float extendCableSpeed = 20;
    [SerializeField] private float predictionSphereCastRadius = 3;

    public float MaxSwingDistance { get => maxSwingDistance; }
    public float HorizontalThrustForce { get => horizontalThrustForce; }
    public float ForwardThrustForce { get => forwardThrustForce; }
    public float ExtendCableSpeed { get => extendCableSpeed; }
    public float PredictionSphereCastRadius { get => predictionSphereCastRadius; }

    void Awake()
    {
        playerManager = GetComponent<IMovementManager>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }
}
