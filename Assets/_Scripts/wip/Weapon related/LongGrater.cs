using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class LongGrater : MonoBehaviour, IWeapon
{
    private IWeaponHolder holder;
    private bool _canShoot = true;
    [SerializeField] private float animationTime = .25f;
    //[SerializeField] int ammo = 10;
    [SerializeField] Transform cheeseAmmo;
    [SerializeField] Transform ammoStartPosition;
    [SerializeField] Transform ammoEndPosition;
    [SerializeField] Projectile bullet;
    [SerializeField] Transform shootingPos;
    [SerializeField] private int bulletsToShoot = 5;
    bool shooting;
    List<Projectile> bulletList = new();

    void Start()
    {
        // POOLING LOGIC- TO WORK ON 
        for(int i = 0; i < 50; i++)
        {
            bulletList.Add(Instantiate(bullet));
            bullet.Deactivate();
        }
        Debug.Log(bulletList.Count);
    }

    public void Shoot()
    {
    if(_canShoot)
        {
            _canShoot = !_canShoot;
            shooting = true;
            StartCoroutine(nameof(ShootingProcess));
            cheeseAmmo.DOMove(ammoEndPosition.position, animationTime).OnComplete(ReturnAmmo);
        }
    }

    private void ReturnAmmo()
    {
        shooting = false;
        cheeseAmmo.DOMove(ammoStartPosition.position, animationTime).OnComplete(() => _canShoot = true);
    }

    private IEnumerator ShootingProcess()
    {
        Debug.Log("Started coroutine");
        int bulletsShot = 0;
        while(shooting == true)
        {
            yield return new WaitForSeconds(animationTime / bulletsToShoot);
            var cheeseBullet = Instantiate(bullet);
            cheeseBullet.Initialize(this, shootingPos.position, cheeseAmmo.transform.rotation);
            bulletsShot++;
        }
                Debug.Log($"Ended coroutine with {bulletsShot} bullets.");
    }
        
    }

