using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    public bool IsReady { get => _predictionHit.point != Vector3.zero; }
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
    [SerializeField] private ParticleSystem cheeseBitsParticle;
    private Vector3 _swingPoint;
    private SpringJoint _joint;
    private RaycastHit _predictionHit;
    private RopeHandler _activeRope;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        hitPredictionHandler.SetActive(true);
    }

    void Update()
    {
        /* if (Input.GetKeyDown(swingInput)) CheckSwing();
        if (Input.GetKeyUp(swingInput)) StopSwing(); */
    }

    public void StartSwing(Vector3 position)
    {
        _predictionHit.point = position;
    }

    private void CheckSwing()
    {
        if (IsReady)
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

        handRotation.SetTarget(_swingPoint);
        _rb.linearDamping = swingDrag;

        cheeseBitsParticle.Play();
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
            handRotation.ResetTarget();
            _activeRope.CutRope(_rb.linearVelocity);
            Destroy(_joint);
            hitPredictionHandler.SetActive(true);
    }

    public void SwingMove(Vector2 moveInput)
    {
            //Sideways movement
            Vector3 moveDirection = lookDirection.transform.right * Input.GetAxisRaw("Horizontal") * horizontalThrustForce;
            _rb.AddForce(moveDirection.normalized);

            //Lengthening the joint
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                float extendedDistanceFromPoint = Vector3.Distance(transform.position, _swingPoint) + extendRopeSpeed;
                _joint.maxDistance = extendedDistanceFromPoint * 0.8f;
                _joint.minDistance = extendedDistanceFromPoint * 0.2f;
            }

            //Shortening the joint
            /* if (Input.GetKey(shortenRopeInput))
            {
                Vector3 directionToPoint = _swingPoint - transform.position;
                _rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

                float distanceFromPoint = Vector3.Distance(transform.position, _swingPoint);

                _joint.maxDistance = distanceFromPoint * 0.8f;
                _joint.minDistance = distanceFromPoint * 0.25f;
            } */
    }

    public void CheckForSwingPoints()
    {
       if(_swingPoint == Vector3.zero)
        {
            _predictionHit = hitPredictionHandler.ShootRaycast(maxDistance, hitLayer);
        }
        hitPredictionHandler.SetActive(false);
    }
}