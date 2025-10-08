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

    public void Initialize(Vector3 startPos, Vector3 endPos)
    {
        anchorEdge.position = startPos;
        movingEdge.position = endPos;
        InitializeSpringJoint();
    }

    public void SetMomentum(Vector3 matchVelocity)
    {
        movingEdge.GetComponent<Rigidbody>().linearVelocity = matchVelocity * 0.5f;
    }

    private void InitializeSpringJoint()
    {
        var joint = anchorEdge.GetComponent<SpringJoint>();

        float distanceFromPoint = Vector3.Distance(anchorEdge.position, movingEdge.position);

        joint.maxDistance = distanceFromPoint * 0.15f;
        joint.minDistance = distanceFromPoint * 0.1f;

        joint.spring = 40f;
        joint.damper = 15f;
        joint.massScale = 1f;

        _drawLine = StartCoroutine(nameof(DrawLine));
    }

    private IEnumerator DrawLine()
    {
        float timePassed = 0;
        while (timePassed < lifetime)
        {
            timePassed += Time.deltaTime;
            _lr.SetPosition(0, anchorEdge.position);
            _lr.SetPosition(1, movingEdge.position);
            yield return null;
        }
        _drawLine = null;
        Destroy(gameObject);
    }
}
