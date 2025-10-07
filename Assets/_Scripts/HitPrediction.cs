using UnityEngine;

public class HitPrediction : MonoBehaviour
{
    [SerializeField] float minScale = 0.07f;
    [SerializeField] float maxScale = 0.8f;
    public void SetPosition(Vector3 source, Vector3 hitPosition, float maxDistance)
    {
        transform.position = hitPosition;
        gameObject.SetActive(hitPosition != Vector3.zero);
        CalculatePredictionPointSize(source, maxDistance);
    }

    private void CalculatePredictionPointSize(Vector3 source, float maxDistance)
    {
        float rangeFromPlayer = Vector3.Distance(transform.position, source) / maxDistance;
        float newScale = Mathf.Lerp(minScale, maxScale, rangeFromPlayer);
        transform.localScale = Vector3.one * newScale;
    }
}
