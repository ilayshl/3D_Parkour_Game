using UnityEngine;

public class PlayerMovement
{
    private const float MOVE_SPEED = 10;
    public float Damp = 0.5f;
    public float MoveSpeedLimitMult = 1; //Normally 1
    public float MoveSpeedMult = 1; //Normally 1
    private Transform _orientation;
    private Rigidbody _rb;



    public PlayerMovement(Transform movementOrientation, Rigidbody rb)
    {
        _orientation = movementOrientation;
        _rb = rb;
    }

    public void Move(Vector2 moveInput)
    {
        /* Vector2 moveDirection = _orientation.forward * moveInput.y + _orientation.right * moveInput.x;
        Vector3 forceToAdd = moveDirection.normalized * MOVE_SPEED * 750 * Time.fixedDeltaTime;
        //forceToAdd = forceToAdd * Damp * MoveSpeedMult;
        _rb.AddForce(forceToAdd, ForceMode.Force);
        Debug.Log($"Moving by {forceToAdd}"); */
        
            Vector3 currentInput = _orientation.forward * moveInput.y + _orientation.right * moveInput.x;
            Vector3 forceToAdd = currentInput.normalized * MOVE_SPEED * 10f;
            _rb.AddForce(forceToAdd * Damp, ForceMode.Force);
    }
    /*  private const float AIRBORNE_MOVEMENT_MULT = 0.3f;

     [SerializeField] private Transform movementOrientation;
     [SerializeField] private float groundDrag = 0.5f;
     [SerializeField] private float airDrag = 0.1f;

     private bool _readyToJump = true;

     private Vector3 moveDirection;
     private Coroutine _checkInput,_move, _groundCheck;

     private bool _isGrounded;
     private float _speedLimitMult = 1;

     private PlayerData _playerData;
     private IMovementManager _playerManager;

     void Awake()
     {
         _playerData = GetComponent<PlayerData>();
         _playerManager = GetComponent<IMovementManager>();
     }
     private void OnEnable()
     {
         _playerManager.OnMovementStateChanged += OnMovementStateChanged;
     }

     private void OnDisable()
     {
         _playerManager.OnMovementStateChanged -= OnMovementStateChanged;
     }

     void Start()
     {
         _playerManager.ChangeMovementState();
     }

     void Update()
     {
         CurrentVelocity = new Vector2(_playerData.rb.linearVelocity.x, _playerData.rb.linearVelocity.z);
     }

     public void OnMovementStateChanged(MovementState newState)
     {
         if (newState == MovementState.Walking)
         {
             _playerData.rb.linearDamping = _playerData.GroundDrag;
             _move = StartCoroutine(nameof(Move));
             _groundCheck = StartCoroutine(nameof(CheckForGround));
             _checkInput = StartCoroutine(nameof(CheckInput));
         }
     }

     private IEnumerator CheckInput()
     {
         while (_playerManager.State == MovementState.Walking)
         {
             GetInput();
             LimitSpeed();
             yield return null;
         }

         _checkInput = null;
     }

     private void GetInput()
     {
         if (Input.GetKey(_playerData.JumpInput) && _readyToJump)
         {
             Jump();
             Invoke(nameof(ResetJump), _playerData.JumpCooldown);
         }
     }

     private IEnumerator CheckForGround()
     {
         yield return new WaitForSeconds(_playerData.JumpCooldown);
         while (_playerManager.State == MovementState.Walking)
         {
             if (!Physics.CheckSphere(transform.position, 0.1f, _playerData.GroundLayer))
             {
                 _playerManager.ChangeMovementState();
             }
             yield return null;
         }
         _groundCheck = null;
     }

     private void LimitSpeed()
     {
         if (CurrentVelocity.magnitude > LimitedSpeedValue())
         {
             Vector2 limitVelocity = CurrentVelocity.normalized * _playerData.MoveSpeed * _speedLimitMult;
             _playerData.rb.linearVelocity = new Vector3(limitVelocity.x, _playerData.rb.linearVelocity.y, limitVelocity.y);
         }
     }

     private float LimitedSpeedValue()
     {
         float limitedSpeed = _playerData.MoveSpeed;
         if (_isGrounded)
         {
             return limitedSpeed;
         }
         else
         {
             return limitedSpeed * 4;
         }
     }

     private void Jump()
     {
         _playerData.rb.linearVelocity = new Vector3(_playerData.rb.linearVelocity.x, 0, _playerData.rb.linearVelocity.z);
         _playerData.rb.AddForce(transform.up * _playerData.JumpForce, ForceMode.Impulse);
         _readyToJump = false;
     }

     private void ResetJump()
     {
         _readyToJump = true;
     }

      */
}