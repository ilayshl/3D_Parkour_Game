using UnityEngine;

/// <summary>
/// Holds data for swinging, because I intend on giving it stats that can change with each character selected.
/// </summary>
[CreateAssetMenu(fileName = "New Swing Data", menuName = "Cheese Pull/Swing Data")]
public class PlayerSwingData : ScriptableObject
{
    [Header("Ray Variables")]
    [SerializeField] private int maxDistance = 25;
    [SerializeField] private LayerMask hitLayer;
    public int MaxDistance => maxDistance;
    public LayerMask HitLayer => hitLayer;

    [Header("Movement Variables")]
    [SerializeField] private float horizontalThrustForce = 2000;
    [SerializeField] private float forwardThrustForce = 3000;
    [SerializeField] private float extendRopeSpeed = 20;
    public float HorizontalThrustForce => horizontalThrustForce;
    public float ForwardThrustForce => forwardThrustForce;
    public float ExtendRopeSpeed => extendRopeSpeed;

    [Header("References")]
    [SerializeField] private RopeHandler ropeHandler;
    public RopeHandler RopeHandler => ropeHandler;
}
