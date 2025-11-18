using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LongGrater : Weapon
{
    protected override void Shoot()
    {
        if (_canShoot)
        {
            _canShoot = !_canShoot;
            _isShooting = true;
            StartCoroutine(nameof(ShootingProcess));
            cheeseMagazine.DOLocalMove(magazineEndPosition.localPosition, animationTime).OnComplete(ReturnMagazine);
        }
    }

    private Projectile CreateProjectile()
    {
        Vector3 direction = _shootHelper.GetDirection(shootingPos);
        var projectileShot = ObjectPoolManager.SpawnObject(projectileToShoot, shootingPos.position, cheeseMagazine.rotation);
        projectileShot.Initialize(this, direction, shootForce);
        return projectileShot;
    }

    private void ReturnMagazine()
    {
        _isShooting = false;
        cheeseMagazine.DOLocalMove(magazineStartPosition.localPosition, animationTime).OnComplete(() => _canShoot = true);
    }

    private IEnumerator ShootingProcess()
    {
        int bulletsShot = 0;
        while (_isShooting == true)
        {
            CreateProjectile().Activate();
            bulletsShot++;
            yield return new WaitForSeconds(animationTime / bulletsToShoot);
        }
        Debug.Log($"Ended coroutine with {bulletsShot} bullets.");
    }
}

