using System.Collections;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode leftInput, rightInput, shortenJointInput;

    [Header("References")]
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform leftGunTip, rightGunTip, cam;
    [SerializeField] private LayerMask grappableLayer;

    [Header("Swinging")]
    [SerializeField] private float maxSwingDistance = 25f;
    [SerializeField] private float horizontalThrustForce;
    [SerializeField] private float forwardThrustForce;
    [SerializeField] private float extendCableSpeed;
    private Vector3 _swingPoint;
    private SpringJoint _joint;

    private Vector3 _currentGrapplePosition;
    private Rigidbody _rb;
    private Coroutine odmMovement;
    private IMovementManager _playerManager;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerManager = GetComponent<IMovementManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(leftInput)) StartSwingLeft();
        if (Input.GetKeyUp(leftInput)) StopSwingLeft();

    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwingLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, grappableLayer))
        {
            _swingPoint = hit.point;
            _joint = this.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _swingPoint;

            float distanceFromPoint = Vector3.Distance(transform.position, _swingPoint);

            _joint.maxDistance = distanceFromPoint * 0.8f;
            _joint.minDistance = distanceFromPoint * 0.25f;

            _joint.spring = 4.5f;
            _joint.damper = 7f;
            _joint.massScale = 4.5f;

            lr.positionCount = 2;
            _currentGrapplePosition = leftGunTip.position;

            odmMovement = StartCoroutine(nameof(OdmMovement));
            _playerManager.ChangeMovementState(MovementState.Grappling);
        }
    }

    private void StopSwingLeft()
    {
        lr.positionCount = 0;
        Destroy(_joint);
        _playerManager.ChangeMovementState();
    }

    private void DrawRope()
    {
        if (!_joint) return;

        _currentGrapplePosition = Vector3.Lerp(_currentGrapplePosition, _swingPoint, Time.deltaTime * 8f);
        lr.SetPosition(0, leftGunTip.position);
        lr.SetPosition(1, _swingPoint);
    }

    private IEnumerator OdmMovement()
    {
        while (_joint != null)
        {
            //Sideways movement
            Vector3 moveDirection = transform.right * Input.GetAxisRaw("Horizontal") * horizontalThrustForce;
            _rb.AddForce(moveDirection.normalized);
            
            //Lenghtening the joint
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                float extendedDistanceFromPoint = Vector3.Distance(transform.position, _swingPoint) + extendCableSpeed;
                _joint.maxDistance = extendedDistanceFromPoint * 0.8f;
                _joint.minDistance = extendedDistanceFromPoint * 0.25f;
            }

            //Shortening the joint
            if (Input.GetKey(shortenJointInput))
            {
                Vector3 directionToPoint = _swingPoint - transform.position;
                _rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

                float distanceFromPoint = Vector3.Distance(transform.position, _swingPoint);

                _joint.maxDistance = distanceFromPoint * 0.8f;
                _joint.minDistance = distanceFromPoint * 0.25f;
            }
            yield return null;
        }

        odmMovement = null;
    }
}
