using DG.Tweening;
using UnityEngine;

/// <summary>
/// Responsible for the RopeCutoff object that spawns when the player stops swinging.
/// </summary>
public class RopeCutoff : MonoBehaviour
{
    [SerializeField] private Transform anchorEdge, movingEdge;
    private LineRenderer _lr;
    private Rigidbody _rb;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _rb = movingEdge.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        DrawLine();
    }

    public void Initialize(Vector3 startPos, Vector3 endPos)
    {
        anchorEdge.position = startPos;
        movingEdge.position = endPos;
        gameObject.SetActive(true);
        InitializeLineRenderer();
        InitializeSpringJoint();
    }

    public void SetMomentum(Vector3 matchVelocity)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.linearVelocity = matchVelocity * 0.5f;
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

    public void Destroy(float time)
    {
        _rb.DOMove(anchorEdge.position, time).OnComplete(() => gameObject.SetActive(false));
    }
}
