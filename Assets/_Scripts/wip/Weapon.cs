using UnityEditor;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform muzzle => shootingPos;
    [Header("Projectile Prefab")]
    [SerializeField] protected Projectile projectileToShoot;
    [Header("Weapon settings")]
    [SerializeField] protected TriggerType triggerType = TriggerType.Manual;
    [SerializeField] protected int bulletsToShoot;
    [SerializeField] protected float shootForce;
    [SerializeField] protected float magazineSize;
    [SerializeField] protected float spread;
    //[SerializeField] int ammo = 10;
    [Header("Animation anchors")]
    [SerializeField] protected Transform cheeseMagazine;
    [SerializeField] protected Transform magazineStartPosition;
    [SerializeField] protected Transform magazineEndPosition;
    [SerializeField] protected Transform shootingPos;
    [SerializeField] protected float animationTime;
    protected bool _canShoot = true;
    protected bool _isShooting = false;
    protected ShootingHelper _shootHelper;
    private float _ammoLeft;
    private IWeaponHolder _holder;


    private void Awake()
    {
        _ammoLeft = magazineSize;
    }

    public void SetShootHelper(ShootingHelper helper)
    {
        _shootHelper = helper;
    }

    public void CheckForShoot()
    {
        if(_ammoLeft <= 0)
        {
            //No ammo logic - reload
            return;
        }
        Shoot();
    }

    protected abstract void Shoot();
}

public enum TriggerType
{
    Manual,
    SemiAuto,
    Auto,
}
