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
    private Vector3 _activePoint;
    private Transform _startPos;

    public void Initialize(Transform playerPos, RaycastHit ray)
    {
        _activeRaycast = ray;
        _activePoint = _activeRaycast.point;
        _startPos = playerPos;
        InitializeRope();
    }

    public void CutRope(Vector3 playerMomentum)
    {
        rope.OnRopeComplete -= InitializeSplashModel;
        rope.Disable();
        SpawnRopeCutoff(playerMomentum);
    }

    private void InitializeRope()
    {
        rope.transform.position = _activePoint;
        rope.Initialize(_startPos);
        rope.OnRopeComplete += InitializeSplashModel;
    }

    private void InitializeSplashModel()
    {
        Quaternion lookDirection = Quaternion.LookRotation(_activeRaycast.normal) * Quaternion.Euler(90, 0, 0); ;
        Vector3 splashPosition = _activePoint;
        splashModel.transform.position = splashPosition;
        splashModel.transform.rotation = lookDirection;
        splashModel.gameObject.SetActive(true);
        splashModel.DOScale(Vector3.one, .1f).SetEase(Ease.OutBack);
    }

    private void SpawnRopeCutoff(Vector3 playerMomentum)
    {
        if(splashModel.gameObject.activeSelf)
        {
        ropeCutoff.transform.position = _activePoint;
        Vector3 midway = Vector3.Lerp(_startPos.position, _activePoint, 0.3f);
        ropeCutoff.Initialize(_activePoint, midway);
        ropeCutoff.SetMomentum(playerMomentum);

        StartCoroutine(Destroy());            
        }
        else
        {
            ReturnToPool();
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(cutoffLifetime);
        ropeCutoff.Destroy(.25f);
        splashModel.transform.DOScale(Vector3.zero, .25f).OnComplete(ReturnToPool);

    }

    private void ReturnToPool()
    {
        splashModel.gameObject.SetActive(false);
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}
