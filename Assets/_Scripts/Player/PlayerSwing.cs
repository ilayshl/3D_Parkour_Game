using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    [SerializeField] private Transform lookDirection;
    [SerializeField] private KeyCode swingInput;
    [SerializeField] private HitPrediction predictionHitObject;
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3 _swingPoint;
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
        if (Input.GetKeyDown(swingInput)) CheckSwing();
        if (Input.GetKeyUp(swingInput)) StopSwing();

    }

    private void CheckSwing()
    {
        if (_predictionHit.point != Vector3.zero)
        {
            StartSwing();
        }
    }

    private void StartSwing()
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

        lineRenderer.positionCount = 2;
        _currentGrapplePosition = lookDirection.position;

        _playerManager.ChangeMovementState(MovementState.Swinging);
        _odmMovement = StartCoroutine(nameof(OdmMovement));
        _drawRope = StartCoroutine(nameof(DrawRope));
        _playerData.rb.linearDamping = _playerData.AirDrag;
    }

    private void StopSwing()
    {
        lineRenderer.positionCount = 0;
        Destroy(_joint);
        _playerData.playerManager.ChangeMovementState();
        _checkForSwingPoints = StartCoroutine(nameof(CheckForSwingPoints));
        _playerData.SwingPredictionPoint.gameObject.SetActive(true);
    }

    private IEnumerator DrawRope()
    {
        while (_playerManager.State == MovementState.Swinging)
        {
            float distance = Vector3.Distance(lookDirection.position, _swingPoint);
            if (_currentGrapplePosition != _swingPoint)
            {
                _currentGrapplePosition = Vector3.MoveTowards(_currentGrapplePosition, _swingPoint, Time.deltaTime * distance * 15);
            }
            lineRenderer.SetPosition(0, lookDirection.position);
            lineRenderer.SetPosition(1, _currentGrapplePosition);
            yield return null;
        }
        _drawRope = null;
    }

    private IEnumerator OdmMovement()
    {
        while (_playerManager.State == MovementState.Swinging)
        {
            //Sideways movement
            Vector3 moveDirection = _playerData.Cam.transform.right * Input.GetAxisRaw("Horizontal") * _playerData.HorizontalThrustForce;
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
            Physics.SphereCast(_playerData.Cam.position, _playerData.PredictionSphereCastRadius, lookDirection.forward,
                    out sphereCastHit, _playerData.MaxSwingDistance, _playerData.GrappableLayer);

            _predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
            predictionHitObject.SetPosition(_playerData.Cam.position, _predictionHit.point, _playerData.MaxSwingDistance);
            yield return null;
        }
        _checkForSwingPoints = null;
        predictionHitObject.gameObject.SetActive(false);
    }

    }
