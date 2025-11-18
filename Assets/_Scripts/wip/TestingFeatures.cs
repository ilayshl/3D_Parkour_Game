using DG.Tweening;
using UnityEngine;

public class TestingFeatures : MonoBehaviour
{
    Weapon weapon;
    InputReader input = new();

    void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
    }
    void OnEnable()
    {
        input.ShootEvent += ShootTesting;
    }

    void Start()
    {
        input.Initiate();
    }


    void OnDisable()
    {
        input.ShootEvent -= ShootTesting;
    }

    private void ShootTesting()
    {
            //weapon.Shoot();
    }

}
