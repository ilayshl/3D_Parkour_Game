using UnityEngine;

public class HitPredictionHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HitPredictionIndicator predictionPrefab;
    [SerializeField] private float sphereRadius = 3f;
    private HitPredictionIndicator _activeIndicator;

    void Start()
    {
        _activeIndicator = Instantiate(predictionPrefab);
    }

    public RaycastHit ShootRaycast(float maxDistance, LayerMask hitLayer)
    {
        RaycastHit raycastHit;
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                out raycastHit, maxDistance, hitLayer);

        RaycastHit sphereCastHit;
        Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward,
                out sphereCastHit, maxDistance, hitLayer);

        RaycastHit _predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
        _activeIndicator.SetPosition(_predictionHit.point);
        _activeIndicator.CalculatePredictionPointSize(playerCamera.transform.position, maxDistance);
        return _predictionHit;
    }

    public void SetActive(bool value)
    {
        _activeIndicator.gameObject.SetActive(value);
    }

    }
