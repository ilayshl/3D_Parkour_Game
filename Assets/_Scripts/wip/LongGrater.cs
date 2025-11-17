using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class LongGrater : Weapon
{
    
    List<Projectile> bulletList = new();

    void Start()
    {
        // POOLING LOGIC- TO WORK ON 
        /* for(int i = 0; i < 50; i++)
        {
            bulletList.Add(Instantiate(bullet));
            bullet.Deactivate();
        }
        Debug.Log(bulletList.Count); */
    }

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
            var cheeseBullet = Instantiate(projectileToShoot);
            cheeseBullet.Initialize(this, shootingPos.position, cheeseAmmo.transform.rotation, shootForce);
            cheeseBullet.gameObject.SetActive(true);
            bulletsShot++;
            yield return new WaitForSeconds(animationTime / bulletsToShoot);
        }
                Debug.Log($"Ended coroutine with {bulletsShot} bullets.");
    }
}

