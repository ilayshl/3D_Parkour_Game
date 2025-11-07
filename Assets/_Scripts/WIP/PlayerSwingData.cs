using UnityEngine;

public class PlayerSwingData
{
[Header("Ray Variables")]
[SerializeField] private int maxDistance = 25;
[SerializeField] private LayerMask hitLayer;
public int MaxDistance => maxDistance;
public LayerMask HitLayer => hitLayer;

[Header("Movement Variables")]
[SerializeField] private float moveSpeed = 10f;
[SerializeField] private float swingSpeedLimitMult = 4f;
[SerializeField] private float horizontalThrustForce = 2000;
[SerializeField] private float forwardThrustForce = 3000;
[SerializeField] private float extendRopeSpeed = 20;
[SerializeField] private float swingDrag = 0.01f;
public float MoveSpeed => moveSpeed;
public float SwingSpeedLimitMult => swingSpeedLimitMult;
public float HorizontalThrustForce => horizontalThrustForce;
public float ForwardThrustForce => forwardThrustForce;
public float ExtendRopeSpeed => extendRopeSpeed;
public float SwingDrag => swingDrag;

/*[Header("References")]
[SerializeField] private HitPredictionHandler hitPredictionHandler;
[SerializeField] private RopeHandler ropeHandler;
[SerializeField] private SwingingHandRotation handRotation;
[SerializeField] private ParticleSystem cheeseBitsParticle;
public HitPredictionHandler HitPredictionHandler => hitPredictionHandler;
public RopeHandler RopeHandler => ropeHandler;
public SwingingHandRotation HandRotation => handRotation;
public ParticleSystem CheeseBitsParticle => cheeseBitsParticle; */

}
