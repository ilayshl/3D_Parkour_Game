using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform cheeseAmmo;
    [SerializeField] protected TriggerType triggerType = TriggerType.Manual;
    [SerializeField] protected Projectile projectileToShoot;
    [SerializeField] protected float animationTime = .25f;
    //[SerializeField] int ammo = 10;
    [SerializeField] protected Transform ammoStartPosition;
    [SerializeField] protected Transform ammoEndPosition;
    [SerializeField] protected Transform shootingPos;
    [SerializeField] protected int bulletsToShoot = 5;
    [SerializeField] protected float shootForce;
    protected IWeaponHolder _holder;
    protected bool _canShoot = true;
    protected bool _shooting;
    protected PoolHolder<Projectile> _pool;

    void Start()
    {
      InitiatePool();
    }

    private void InitiatePool()
    {
        _pool = new PoolHolder<Projectile>(projectileToShoot, 50);
    }

    public abstract void Shoot();
}

public enum TriggerType
{
    Manual,
    SemiAuto,
    Auto,
}
