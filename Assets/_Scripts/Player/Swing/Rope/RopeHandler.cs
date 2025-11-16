using System.Collections;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Is held by the prefab of the entire rope, manages the logic of the rope.
/// </summary>
public class RopeHandler : MonoBehaviour
{   
    [SerializeField] private float cutoffLifetime = 4;
    [SerializeField] private Transform splashModel;
    [SerializeField] private RopeView rope;
    [SerializeField] private RopeCutoff ropeCutoff;
    private RaycastHit _activeRaycast;
    private Transform _startPos;
    private RopeView _activeRope;
    private Transform _activeSplashModel;

    public void Initialize(Transform playerPos, RaycastHit ray)
    {
        _activeRaycast = ray;
        _startPos = playerPos;
        InitializeRope();
    }

    public void CutRope(Vector3 playerMomentum)
    {
        if (_activeSplashModel != null)
        {
        SpawnRopeCutoff(playerMomentum);
        }
        _activeRope.DestroyRope();
    }

    private void InitializeSplashModel()
    {
        Quaternion lookDirection = Quaternion.LookRotation(_activeRaycast.normal) * Quaternion.Euler(90, 0, 0); ;
        Vector3 splashPosition = _activeRaycast.point;
        _activeSplashModel = Instantiate(splashModel, splashPosition, lookDirection, this.transform);
        _activeSplashModel.DOScale(Vector3.one, .1f).SetEase(Ease.OutBack);
    }

    private void InitializeRope()
    {
        _activeRope = Instantiate(rope, _activeRaycast.point, Quaternion.identity, this.transform);
        _activeRope.Initialize(_startPos, _activeRaycast.point);
        _activeRope.OnRopeComplete += InitializeSplashModel;
    }

    private void SpawnRopeCutoff(Vector3 playerMomentum)
    {
        RopeCutoff newCutoff = Instantiate(ropeCutoff, _activeRaycast.point, Quaternion.identity, this.transform);
        Vector3 midway = Vector3.Lerp(_startPos.position, _activeRaycast.point, 0.3f);
        newCutoff.Initialize(_activeRaycast.point, midway);
        newCutoff.SetMomentum(playerMomentum);

        _activeRope.OnRopeComplete -= InitializeSplashModel;

        StartCoroutine(Destroy(newCutoff));
    }

    private IEnumerator Destroy(RopeCutoff activeCutoff)
    {
        float timeElapsed = 0f;
        while(timeElapsed < cutoffLifetime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        activeCutoff.Destroy(cutoffLifetime);
        _activeSplashModel.transform.DOScale(Vector3.zero, .25f).OnComplete(() => Destroy(this.gameObject));

    }
}
