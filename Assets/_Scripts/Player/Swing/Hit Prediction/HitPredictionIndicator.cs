using UnityEngine;

/// <summary>
/// The object that is used as a hit prediction indicator.
/// </summary>
public class HitPredictionIndicator : MonoBehaviour
{
    [SerializeField] float minScale = 0.07f;
    [SerializeField] float maxScale = 0.8f;
    public void SetPosition(Vector3 hitPosition)
    {
        transform.position = hitPosition;
    }

    /// <summary>
    /// Grows larger when farther and smaller when closer.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="maxDistance"></param>
    public void CalculatePredictionPointSize(Vector3 source, float maxDistance)
    {
        float rangeFromPlayer = Vector3.Distance(transform.position, source) / maxDistance;
        float newScale = Mathf.Lerp(minScale, maxScale, rangeFromPlayer);
        transform.localScale = Vector3.one * newScale;
    }
}
