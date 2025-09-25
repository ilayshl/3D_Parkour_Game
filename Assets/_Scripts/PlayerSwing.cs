using System.Collections;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    public Vector3 _swingPoint;
    private SpringJoint _joint;
    private Vector3 _currentGrapplePosition; //For line animation
    private RaycastHit _predictionHit;

    private Coroutine _odmMovement, _drawRope, _checkForSwingPoints;

    private PlayerData _playerData;
    private IMovementManager _playerManager;

    void Awake()
    {
        _playerData = GetComponent<PlayerData>();
        _playerManager = GetComponent<IMovementManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_playerData.LeftSwingInput)) CheckSwingLeft();
        if (Input.GetKeyUp(_playerData.LeftSwingInput)) StopSwingLeft();

    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void CheckSwingLeft()
    {
        if (_predictionHit.point != Vector3.zero)
        {
            StartSwingLeft();
        }
    }

    private void StartSwingLeft()
    {
        _swingPoint = _predictionHit.point;
        _joint = this.gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _swingPoint;

        float distanceFromPoint = Vector3.Distance(transform.position, _swingPoint);

        _joint.maxDistance = distanceFromPoint * 0.8f;
        _joint.minDistance = distanceFromPoint * 0.2f;

        _joint.spring = 4.5f;
        _joint.damper = 7f;
        _joint.massScale = 4.5f;

        _playerData.lr.positionCount = 2;
        _currentGrapplePosition = _playerData.LeftGunTip.position;

        _playerManager.ChangeMovementState(MovementState.Swinging);
        _odmMovement = StartCoroutine(nameof(OdmMovement));
        _drawRope = StartCoroutine(nameof(DrawRope));
        _playerData.rb.linearDamping = _playerData.AirDrag;
    }

    private void StopSwingLeft()
    {
        _playerData.lr.positionCount = 0;
        Destroy(_joint);
        _playerData.playerManager.ChangeMovementState();
        _checkForSwingPoints = StartCoroutine(nameof(CheckForSwingPoints));
        _playerData.SwingPredictionPoint.gameObject.SetActive(true);
    }

    private IEnumerator DrawRope()
    {
        while (_playerManager.State == MovementState.Swinging)
        {
            _currentGrapplePosition = Vector3.Lerp(_currentGrapplePosition, _swingPoint, Time.deltaTime * 9);
            _playerData.lr.SetPosition(0, _playerData.LeftGunTip.position);
            _playerData.lr.SetPosition(1, _currentGrapplePosition);
            yield return null;
        }
        _drawRope = null;
    }

    private IEnumerator OdmMovement()
    {
        while (_playerManager.State == MovementState.Swinging)
        {
            //Sideways movement
            Vector3 moveDirection = transform.right * Input.GetAxisRaw("Horizontal") * _playerData.HorizontalThrustForce;
            _playerData.rb.AddForce(moveDirection.normalized);

            //Lenghtening the joint
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                float extendedDistanceFromPoint = Vector3.Distance(transform.position, _swingPoint) + _playerData.ExtendCableSpeed;
                _joint.maxDistance = extendedDistanceFromPoint * 0.8f;
                _joint.minDistance = extendedDistanceFromPoint * 0.2f;
            }

            //Shortening the joint
            if (Input.GetKey(_playerData.ShortenJointInput))
            {
                Vector3 directionToPoint = _swingPoint - transform.position;
                _playerData.rb.AddForce(directionToPoint.normalized * _playerData.ForwardThrustForce * Time.deltaTime);

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
        Vector2 currentVelocity = new Vector2(_playerData.rb.linearVelocity.x, _playerData.rb.linearVelocity.z);
        if (currentVelocity.magnitude > _playerData.MoveSpeed * _playerData.SwingSpeedLimitMult)
        {
            Vector2 limitVelocity = currentVelocity.normalized * _playerData.MoveSpeed * _playerData.SwingSpeedLimitMult;
            _playerData.rb.linearVelocity = new Vector3(limitVelocity.x, _playerData.rb.linearVelocity.y, limitVelocity.y);
        }
    }

    private IEnumerator CheckForSwingPoints()
    {
        while (_playerManager.State != MovementState.Swinging)
        {
            RaycastHit raycastHit;
            Physics.Raycast(_playerData.Cam.position, _playerData.Cam.forward,
                    out raycastHit, _playerData.MaxSwingDistance, _playerData.GrappableLayer);

            RaycastHit sphereCastHit;
            Physics.SphereCast(_playerData.Cam.position, _playerData.PredictionSphereCastRadius, _playerData.Cam.forward,
                    out sphereCastHit, _playerData.MaxSwingDistance, _playerData.GrappableLayer);

           /*  Vector3 hitPosition;
            if (raycastHit.point != Vector3.zero)
            {
                hitPosition = raycastHit.point;
            }
            else if (sphereCastHit.point != Vector3.zero)
            {
                hitPosition = sphereCastHit.point;
            }
            else
            {
                hitPosition = Vector3.zero;
            } */
            _predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
            _playerData.SwingPredictionPoint.position = _predictionHit.point;
            _playerData.SwingPredictionPoint.gameObject.SetActive(_predictionHit.point != Vector3.zero);
            yield return null;
        }
        _checkForSwingPoints = null;
        _playerData.SwingPredictionPoint.gameObject.SetActive(false);
    }
    }
