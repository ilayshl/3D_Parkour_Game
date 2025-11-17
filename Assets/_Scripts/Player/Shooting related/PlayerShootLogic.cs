using UnityEngine;

public class PlayerShootLogic
{
    private Weapon _weapon;
    private Transform _cameraView;

    /* public PlayerShootLogic(Parameters)
    {

        
    } */
    public void Shoot()
    {
        _weapon.Shoot();
    }
}
