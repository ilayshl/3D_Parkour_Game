using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the hit prediction logic (what the player sees when aiming at a Grappable while not swinging).
/// </summary>
public class HitPredictionHandler : MonoBehaviour
{
    public RaycastHit PredictionHit { get; private set; }
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HitPredictionIndicator predictionPrefab;
    [SerializeField] private float sphereRadius = 3f;
    private HitPredictionIndicator _activeIndicator;
    private Coroutine _activeCoroutine;
    private float _maxDistance;
    private LayerMask _hitLayer;

    void Start()
    {
        _activeIndicator = Instantiate(predictionPrefab);
        SetActive(true);
    }

    public void Initiaize(float maxDistance, LayerMask hitLayer)
    {
        _maxDistance = maxDistance;
        _hitLayer = hitLayer;
    }

    /// <summary>
    /// Calculates both direct raycast and sphere cast forward and returning the one that hits first.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalculateHitPrediction()
    {
        while (_activeIndicator.gameObject.activeInHierarchy)
        {
            RaycastHit raycastHit;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                    out raycastHit, _maxDistance, _hitLayer);

            RaycastHit sphereCastHit;
            Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward,
                    out sphereCastHit, _maxDistance, _hitLayer);

            RaycastHit predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
            _activeIndicator.SetPosition(predictionHit.point);
            _activeIndicator.CalculatePredictionPointSize(playerCamera.transform.position, _maxDistance);
            PredictionHit = predictionHit;
            yield return null;
        }
        _activeCoroutine = null;
    }

    /// <summary>
    /// Decides the visibility of the active hit prediction indicator.
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        _activeIndicator.gameObject.SetActive(value);
        _activeCoroutine = StartCoroutine(nameof(CalculateHitPrediction)); //Always activates on true. On false immediately nullifies.
    }

}
