using System.Collections;
using UnityEngine;

public class RopeCutoff : MonoBehaviour
{
    [SerializeField] private float lifetime = 4;
    [SerializeField] private Transform anchorEdge, movingEdge;
    private LineRenderer _lr;
    private Coroutine _drawLine;

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
        Destroy(gameObject, lifetime);
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
        Debug.Log("Initialized line renderer");
    }

    private void InitializeSpringJoint()
    {
        var joint = anchorEdge.GetComponent<SpringJoint>();

        float distanceFromPoint = Vector3.Distance(anchorEdge.position, movingEdge.position);

        joint.maxDistance = distanceFromPoint * 0.15f;
        joint.minDistance = distanceFromPoint * 0.1f;

        joint.spring = 30f;
        joint.damper = 10f;
        joint.massScale = 1f;

        _drawLine = StartCoroutine(nameof(DrawLine));
    }

    private void DrawLine()
    {
            _lr.SetPosition(0, anchorEdge.position);
            _lr.SetPosition(1, movingEdge.position);
    }
}
