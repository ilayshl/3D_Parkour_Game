using UnityEngine;

/// <summary>
/// Responsible for the visuals of the rope.
/// </summary>
public class Rope : MonoBehaviour
{
    public bool CanActivate { get => !isActiveAndEnabled; }
    [SerializeField] private RopeCutoff ropeCutoff;
    private bool _isActive = true;
    private bool _isComplete = false;
    private Transform _playerPos;
    private Vector3 _wallPos;
    private Vector3 _currentRopeEndPos;
    private LineRenderer _lr;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        if (_playerPos != null && _wallPos != Vector3.zero)
        {
            DrawRope();
        }
    }

    public void Initialize(Transform startPos, Vector3 endPos)
    {
            _playerPos = startPos;
            _wallPos = endPos;
            _currentRopeEndPos = startPos.position;
            InitiateLineRenderer();
    }

    private void InitiateLineRenderer()
    {
        _lr.positionCount = 2;
    }

    public void CutRope(Vector3 playerMomentum)
    {
        SpawnRopeCutoff(playerMomentum);
        _isActive = false;
    }

    private void DrawRope()
    {
        if (_isActive)
        {
            if (!_isComplete)
            {
                ShootRope();
                Debug.Log("Shooting rope!");
            }
            else
            {
                DrawCompleteRope();
                Debug.Log("Drawing complete rope!");
            }
        }
        else
        {
            RetractRope();
            Debug.Log("Retracting rope!");
        }
    }

    private void ShootRope()
    {
        float distance = Vector3.Distance(_playerPos.position, _wallPos);
        _currentRopeEndPos = Vector3.MoveTowards(_currentRopeEndPos, _wallPos, Time.deltaTime * distance * 10);
        SetPositions(_playerPos.position, _wallPos);
        if (_currentRopeEndPos == _wallPos)
        {
            SetupRopeComplete();
        }
    }

    private void SetupRopeComplete()
    {
        _lr.positionCount = 3;
        DrawCompleteRope();
        _isComplete = true;
    }

    private void DrawCompleteRope()
    {
        Vector3 midway = Vector3.Lerp(_playerPos.position, _wallPos, 0.5f);
        SetPositions(_playerPos.position, midway, _wallPos);
    }

    private void RetractRope()
    {

        if (_currentRopeEndPos == _wallPos)
        {
            InitiateLineRenderer();
            Vector3 midway = Vector3.Lerp(_playerPos.position, _wallPos, 0.5f);
            _currentRopeEndPos = midway;
        }

        float distance = Vector3.Distance(_playerPos.position, _wallPos);
        _currentRopeEndPos = Vector3.MoveTowards(_currentRopeEndPos, _playerPos.position, Time.deltaTime * distance * 15);
        SetPositions(_playerPos.position, _currentRopeEndPos);
        if (Vector3.Distance(_currentRopeEndPos, _playerPos.position) < 0.5f)
        {
            _isActive = false;
            Destroy(gameObject);
        }
    }

    private void SpawnRopeCutoff(Vector3 playerMomentum)
    {
        RopeCutoff newCutoff = Instantiate(ropeCutoff, _wallPos, Quaternion.identity);
        Vector3 midway = Vector3.Lerp(_playerPos.position, _wallPos, 0.5f);
        newCutoff.Initialize(_wallPos, midway);
        newCutoff.SetMomentum(playerMomentum);
    }

    //Method overloading for setting the positions of existing points in the LineRenderer.
    #region SetPositions
    private void SetPositions(Vector3 firstPosition, Vector3 secondPosition)
    {
        _lr.SetPosition(0, firstPosition);
        _lr.SetPosition(1, secondPosition);
    }

    private void SetPositions(Vector3 firstPosition, Vector3 secondPosition, Vector3 thirdPosition)
    {
        _lr.SetPosition(0, firstPosition);
        _lr.SetPosition(1, secondPosition);
        _lr.SetPosition(2, thirdPosition);
    }
    
    #endregion
}
