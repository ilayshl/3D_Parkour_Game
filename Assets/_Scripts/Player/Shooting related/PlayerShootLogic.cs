using UnityEngine;

public class PlayerShootLogic
{
    private ShootingHelper _shootHelper;
    private Weapon _weapon;
    private Transform _cameraView;

    public PlayerShootLogic(Weapon weapon, Transform cameraView)
    {
        _weapon = weapon;
        _cameraView = cameraView;
        _shootHelper = new(cameraView);
        weapon.SetShootHelper(_shootHelper);
    }

    public void Shoot()
    {
        _weapon.CheckForShoot();
    }

}
