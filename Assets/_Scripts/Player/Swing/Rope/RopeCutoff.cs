using UnityEngine;

/// <summary>
/// Responsible for the RopeCutoff object that spawns when the player stops swinging.
/// </summary>
public class RopeCutoff : MonoBehaviour
{
    [SerializeField] private float lifetime = 4;
    [SerializeField] private Transform anchorEdge, movingEdge;
    private LineRenderer _lr;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        DrawLine();
    }

    public void Initialize(Vector3 startPos, Vector3 endPos)
    {
        anchorEdge.position = startPos;
        movingEdge.position = endPos;
        InitializeLineRenderer();
        InitializeSpringJoint();
        Destroy(gameObject.transform.parent.gameObject, lifetime);
    }

    public void SetMomentum(Vector3 matchVelocity)
    {
        var rb = movingEdge.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.linearVelocity = matchVelocity * 0.5f;
    }

    private void InitializeLineRenderer()
    {
        _lr.positionCount = 2;
    }

    //Don't mind the magic numbers it just works lol
    private void InitializeSpringJoint()
    {
        var joint = anchorEdge.GetComponent<SpringJoint>();

        float distanceFromPoint = Vector3.Distance(anchorEdge.position, movingEdge.position);

        joint.maxDistance = distanceFromPoint * 0.15f;
        joint.minDistance = distanceFromPoint * 0.1f;

        joint.spring = 30f;
        joint.damper = 10f;
        joint.massScale = 1f;
    }

    private void DrawLine()
    {
        _lr.SetPosition(0, anchorEdge.position);
        _lr.SetPosition(1, movingEdge.position);
    }
}
