using UnityEngine;

public class HitPredictionObject : MonoBehaviour
{
    [SerializeField] float minScale = 0.07f;
    [SerializeField] float maxScale = 0.8f;
    public void SetPosition(Vector3 hitPosition)
    {
        transform.position = hitPosition;
        gameObject.SetActive(hitPosition != Vector3.zero);
    }

    public void CalculatePredictionPointSize(Vector3 source, float maxDistance)
    {
        float rangeFromPlayer = Vector3.Distance(transform.position, source) / maxDistance;
        float newScale = Mathf.Lerp(minScale, maxScale, rangeFromPlayer);
        transform.localScale = Vector3.one * newScale;
    }
}
