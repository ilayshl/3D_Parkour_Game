using UnityEngine;

public class ShootingHelper
{
    private Transform _camera;
    private Transform _muzzle;

    public ShootingHelper(Transform camera)
    {
        _camera = camera;
    }

    public Vector3 GetDirection(Transform muzzle)
    {
        Vector3 pointHit;
        RaycastHit hit;
        if(Physics.Raycast(_camera.position, _camera.forward, out hit, Mathf.Infinity))
        {
            pointHit = hit.point;
            Debug.DrawLine(_camera.position, pointHit, Color.white, 10f);
        }
        else
        {
            pointHit = _camera.position + _camera.forward * 75f;
            Debug.DrawLine(_camera.position, pointHit, Color.red, 10f);
        }

        return (pointHit - muzzle.position).normalized;
    }
}
