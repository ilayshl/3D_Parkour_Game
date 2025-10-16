using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode swingInput = KeyCode.Mouse0;
    [SerializeField] private KeyCode shortenRopeInput = KeyCode.Space;
    [Header("Ray Variables")]
    [SerializeField] private int maxDistance = 25;
    [SerializeField] private LayerMask hitLayer;
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float swingSpeedLimitMult = 4f;
    [SerializeField] private float horizontalThrustForce = 2000;
    [SerializeField] private float forwardThrustForce = 3000;
    [SerializeField] private float extendRopeSpeed = 20;
    [SerializeField] private float swingDrag = 0.01f;
    [Header("References")]
    [SerializeField] private Transform lookDirection;
    [SerializeField] private HitPredictionHandler hitPredictionHandler;
    [SerializeField] private RopeHandler ropeHandler;
    [SerializeField] private SwingingHandRotation handRotation;
    private Vector3 _swingPoint;
    private SpringJoint _joint;
    private RaycastHit _predictionHit;
    private RopeHandler _activeRope;

    private Coroutine _odmMovement, _checkForSwingPoints;

    private Rigidbody _rb;
    private IMovementManager _playerManager;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerManager = GetComponent<IMovementManager>();
    }

    void Start()
    {
        _checkForSwingPoints = StartCoroutine(nameof(CheckForSwingPoints));
        hitPredictionHandler.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(swingInput)) CheckSwing();
        if (Input.GetKeyUp(swingInput)) StopSwing();
    }

    private void CheckSwing()
    {
        if (_predictionHit.point != Vector3.zero)
        {
            InitializeSwing();
        }
    }

    private void InitializeSwing()
    {
        _swingPoint = _predictionHit.point;

        InitializeSpringJoint();
        _activeRope = Instantiate(ropeHandler, transform.position, Quaternion.identity);
        _activeRope.Initialize(lookDirection, _predictionHit);

        _playerManager.ChangeMovementState(MovementState.Swinging);
        _odmMovement = StartCoroutine(nameof(OdmMovement));

        handRotation.SetTarget(_swingPoint);
        _rb.linearDamping = swingDrag;
    }

    private void InitializeSpringJoint()
    {
        _joint = this.gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _swingPoint;

        float distanceFromPoint = Vector3.Distance(transform.position, _swingPoint);

        _joint.maxDistance = distanceFromPoint * 0.8f;
        _joint.minDistance = distanceFromPoint * 0.1f;

        _joint.spring = 4.5f;
        _joint.damper = 7f;
        _joint.massScale = 4.5f;
    }

    private void StopSwing()
    {
        if (_playerManager.State == MovementState.Swinging)
        {
            handRotation.ResetTarget();
            _activeRope.CutRope(_rb.linearVelocity);
            _playerManager.ChangeMovementState();
            Destroy(_joint);
            _checkForSwingPoints = StartCoroutine(nameof(CheckForSwingPoints));
            hitPredictionHandler.SetActive(true);
        }
    }

    private IEnumerator OdmMovement()
    {
        while (_playerManager.State == MovementState.Swinging)
        {
            //Sideways movement
            Vector3 moveDirection = lookDirection.transform.right * Input.GetAxisRaw("Horizontal") * horizontalThrustForce;
            _rb.AddForce(moveDirection.normalized);

            //Lenghtening the joint
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                float extendedDistanceFromPoint = Vector3.Distance(transform.position, _swingPoint) + extendRopeSpeed;
                _joint.maxDistance = extendedDistanceFromPoint * 0.8f;
                _joint.minDistance = extendedDistanceFromPoint * 0.2f;
            }

            //Shortening the joint
            if (Input.GetKey(shortenRopeInput))
            {
                Vector3 directionToPoint = _swingPoint - transform.position;
                _rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

                float distanceFromPoint = Vector3.Distance(transform.position, _swingPoint);

                _joint.maxDistance = distanceFromPoint * 0.8f;
                _joint.minDistance = distanceFromPoint * 0.25f;
            }

            LimitSpeed();
            yield return null;
        }
        _odmMovement = null;
    }

    private void LimitSpeed()
    {
        Vector2 currentVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        if (currentVelocity.magnitude > moveSpeed * swingSpeedLimitMult)
        {
            Vector2 limitVelocity = currentVelocity.normalized * moveSpeed * swingSpeedLimitMult;
            _rb.linearVelocity = new Vector3(limitVelocity.x, _rb.linearVelocity.y, limitVelocity.y);
        }
    }

    private IEnumerator CheckForSwingPoints()
    {
        while (_playerManager.State != MovementState.Swinging)
        {
            _predictionHit = hitPredictionHandler.ShootRaycast(maxDistance, hitLayer);
            yield return null;
        }
        _checkForSwingPoints = null;
        hitPredictionHandler.SetActive(false);
    }
}
