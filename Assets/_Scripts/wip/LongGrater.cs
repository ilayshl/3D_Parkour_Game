using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LongGrater : Weapon
{
    public override void Shoot()
    {
    if(_canShoot)
        {
            _canShoot = !_canShoot;
            _shooting = true;
            StartCoroutine(nameof(ShootingProcess));
            cheeseAmmo.DOMove(ammoEndPosition.position, animationTime).OnComplete(ReturnAmmo);
        }
    }

    private Projectile CreateProjectile()
    {
        var projectileShot = ObjectPoolManager.SpawnObject(projectileToShoot, shootingPos.position, cheeseAmmo.rotation);
        projectileShot.Initialize(this, shootForce);
        return projectileShot;
    }

    private void ReturnAmmo()
    {
        _shooting = false;
        cheeseAmmo.DOMove(ammoStartPosition.position, animationTime).OnComplete(() => _canShoot = true);
    }

    private IEnumerator ShootingProcess()
    {
        Debug.Log("Started coroutine");
        int bulletsShot = 0;
        while(_shooting == true)
        {
            CreateProjectile().gameObject.SetActive(true);
            bulletsShot++;
            yield return new WaitForSeconds(animationTime / bulletsToShoot);
        }
                Debug.Log($"Ended coroutine with {bulletsShot} bullets.");
    }
}

