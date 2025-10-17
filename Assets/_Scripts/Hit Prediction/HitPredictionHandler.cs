using UnityEngine;

public class HitPredictionHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HitPredictionObject predictionObject;
    [SerializeField] private float sphereRadius = 3f;

    public RaycastHit ShootRaycast(float maxDistance, LayerMask hitLayer)
    {
        RaycastHit raycastHit;
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                out raycastHit, maxDistance, hitLayer);

        RaycastHit sphereCastHit;
        Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward,
                out sphereCastHit, maxDistance, hitLayer);

        RaycastHit _predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
        predictionObject.SetPosition(_predictionHit.point);
        predictionObject.CalculatePredictionPointSize(playerCamera.transform.position, maxDistance);
        return _predictionHit;
    }

    public void SetActive(bool value)
    {
        predictionObject.gameObject.SetActive(value);
    }

    }
