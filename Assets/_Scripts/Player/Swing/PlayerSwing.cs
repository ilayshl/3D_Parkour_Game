using UnityEngine;

public class PlayerSwing
{
    private PlayerManager _player;
    private PlayerSwingData _data;
    private Rigidbody _rb;
    private HitPredictionHandler _hitPredictionHandler;
    private PlayerMovementOrientation _orientation;
    private SwingingHandRotation _handRotation;
    private ParticleSystem _swingParticles;
    private RopeAnchor _ropeAnchor;
    public PlayerSwing(PlayerManager player, PlayerSwingData data, Rigidbody rb)
    {
        _player = player;
        _data = data;
        _rb = rb;
        _hitPredictionHandler = _player.GetComponent<HitPredictionHandler>();
        _orientation = _player.GetComponentInChildren<PlayerMovementOrientation>();
        _handRotation = _player.transform.parent.GetComponentInChildren<SwingingHandRotation>();
        _swingParticles = _handRotation.GetComponentInChildren<ParticleSystem>();
        _ropeAnchor = _handRotation.GetComponentInChildren<RopeAnchor>();
        _hitPredictionHandler.gameObject.SetActive(true);
    }

    public bool IsSwinging => _swingPoint != Vector3.zero;
    public bool IsReady => _predictionHit.point != Vector3.zero;
    private Vector3 _swingPoint;
    private SpringJoint _joint;
    private RaycastHit _predictionHit; //The desired location of the hit prediction object.
    private RopeHandler _activeRope;

    /// <summary>
    /// Checks and returns bool for if swinging is possible.
    /// </summary>
    /// <returns></returns>
    public bool CheckSwing()
    {
        if (IsReady)
        {
            InitializeSwing();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitializeSwing() //To do: insert factory and object pooling logic! CAN NOT USE INSTANTIATE HERE
    {
        _swingPoint = _predictionHit.point;

        InitializeSpringJoint();
        _activeRope = MonoBehaviour.Instantiate(_player.RopeHandler, _player.transform.position, Quaternion.identity);
        _activeRope.Initialize(_ropeAnchor.transform, _predictionHit);

        _handRotation.SetTarget(_swingPoint);
        _swingParticles.Play(); // Should be along with the factory logic
    }

    /// <summary>
    /// Relevant data for spring joint component.
    /// </summary>
    private void InitializeSpringJoint()
    {
        _joint = _player.gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _swingPoint;

        float distanceFromPoint = Vector3.Distance(_player.transform.position, _swingPoint);

        _joint.maxDistance = distanceFromPoint * 0.8f;
        _joint.minDistance = distanceFromPoint * 0.1f;

        _joint.spring = 4.5f;
        _joint.damper = 7f;
        _joint.massScale = 4.5f;
        Debug.Log("Joint configured!");
    }

    /// <summary>
    /// When Swinging State ends- finish up all variables.
    /// </summary>
    public void StopSwing() //To do: implement usage of factory and object pooling logic! CAN NOT USE DESTROY
    {
        if (IsSwinging)
        {
            _handRotation.ResetTarget();
            _activeRope.CutRope(_rb.linearVelocity);
            MonoBehaviour.Destroy(_joint);
            _hitPredictionHandler.SetActive(true);
            _swingPoint = Vector3.zero;
        }
    }

    /// <summary>
    /// Logic for movement when swinging.
    /// </summary>
    /// <param name="moveInput"></param>
    public void SwingMove(Vector2 moveInput)
    {
        MoveSideways();
        //Lengthening the joint
        if (_player.MoveInput.y < 0)
        {
            ExtendRope();
        }
        else
        {
            MoveForward();
        }
    }

    private void MoveSideways()
    {
        Vector3 moveDirection = _orientation.transform.right * _player.MoveInput.x * _data.HorizontalThrustForce;
        _rb.AddForce(moveDirection.normalized);
    }

    //When player presses go back key
    private void ExtendRope()
    {
        float extendedDistanceFromPoint = Vector3.Distance(_player.transform.position, _swingPoint) + _data.ExtendRopeSpeed;
        _joint.maxDistance = extendedDistanceFromPoint * 0.8f;
        _joint.minDistance = extendedDistanceFromPoint * 0.2f;
    }

    private void MoveForward()
    {
        Vector3 moveDirection = _orientation.transform.forward * _player.MoveInput.y * _data.HorizontalThrustForce;
        _rb.AddForce(moveDirection.normalized);
    }

    //To Do: change to when player presses Jump key
    public void ShortenRope()
    {
        Vector3 directionToPoint = _swingPoint - _player.transform.position;
        _rb.AddForce(directionToPoint.normalized * _data.ForwardThrustForce * Time.deltaTime);

        float distanceFromPoint = Vector3.Distance(_player.transform.position, _swingPoint);

        _joint.maxDistance = distanceFromPoint * 0.8f;
        _joint.minDistance = distanceFromPoint * 0.25f;
    }

    /// <summary>
    /// Shoots a ray and a sphere cast to measure if Grappable is in the player's aim.
    /// </summary>
    public void CheckForSwingPoints()
    {
        if (_swingPoint == Vector3.zero)
        {
            _predictionHit = _hitPredictionHandler.ShootRaycast(_data.MaxDistance, _data.HitLayer);
        }
        else
        {
            _hitPredictionHandler.SetActive(false);
        }
    }
}