using UnityEngine;

public class LineRendererSimulator : MonoBehaviour
{
    [SerializeField] private Transform startPos, endPos;
    private LineRenderer _lr;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        _lr.SetPosition(0, startPos.position);
        _lr.SetPosition(1, endPos.position);
    }
}
