using UnityEngine;

public class GroundSensor
{
    private const float RADIUS = 0.2f;
    private Transform _target;
    private LayerMask _groundLayer;
    public GroundSensor(Transform target, LayerMask groundLayer)
    {
        _target = target;
        _groundLayer = groundLayer;
    }

    public bool CheckForGround()
    {
        return Physics.CheckSphere(_target.position, RADIUS, _groundLayer);
    }
}
