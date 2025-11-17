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

    public void Initialize(Transform playerPos, RaycastHit ray)
    {
        Debug.Log(ray.point);
        _activeRaycast = ray;
        _startPos = playerPos;
        InitializeRope();
    }

    public void CutRope(Vector3 playerMomentum)
    {
        SpawnRopeCutoff(playerMomentum);
        rope.DestroyRope();
    }

    private void InitializeRope()
    {
        rope.transform.position = _activeRaycast.point;
        rope.Initialize(_startPos, _activeRaycast.point);
        rope.OnRopeComplete += InitializeSplashModel;
    }

    private void InitializeSplashModel()
    {
        Quaternion lookDirection = Quaternion.LookRotation(_activeRaycast.normal) * Quaternion.Euler(90, 0, 0); ;
        Vector3 splashPosition = _activeRaycast.point;
        splashModel.transform.position = splashPosition;
        splashModel.transform.rotation = lookDirection;
        splashModel.gameObject.SetActive(true);
        splashModel.DOScale(Vector3.one, .1f).SetEase(Ease.OutBack);
    }


    private void SpawnRopeCutoff(Vector3 playerMomentum)
    {
        ropeCutoff.gameObject.SetActive(true);
        ropeCutoff.transform.position = _activeRaycast.point;
        Vector3 midway = Vector3.Lerp(_startPos.position, _activeRaycast.point, 0.3f);
        ropeCutoff.Initialize(_activeRaycast.point, midway);
        ropeCutoff.SetMomentum(playerMomentum);

        rope.OnRopeComplete -= InitializeSplashModel;

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(cutoffLifetime);
        ropeCutoff.Destroy(.25f);
        splashModel.transform.DOScale(Vector3.zero, .25f).OnComplete(ReturnToPool);

    }

    private void ReturnToPool()
    {
        ropeCutoff.gameObject.SetActive(false);
        splashModel.gameObject.SetActive(false);
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}
