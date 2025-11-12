using System;
using UnityEngine;

/// <summary>
/// Responsible for the visuals of the rope.
/// </summary>
public class RopeView : MonoBehaviour
{
    private const float VISUAL_MIDPOINT = 0.3f;
    private const float ROPE_SHOOT_SPEED = 12f;
    public Action OnRopeComplete;
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

    public void DestroyRope()
    {
        _isActive = false;
    }

    private void DrawRope()
    {
        if (_isActive)
        {
            if (!_isComplete)
            {
                ShootRope();
            }
            else
            {
                DrawCompleteRope();
            }
        }
        else
        {
            RetractRope();
        }
    }

    private void ShootRope()
    {
        float distance = Vector3.Distance(_playerPos.position, _wallPos);
        _currentRopeEndPos = Vector3.MoveTowards(_currentRopeEndPos, _wallPos, Time.deltaTime * distance * ROPE_SHOOT_SPEED);
        SetPositions(_playerPos.position, _currentRopeEndPos);
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
        OnRopeComplete?.Invoke();
    }

    private void DrawCompleteRope()
    {
        Vector3 midway = Vector3.Lerp(_playerPos.position, _wallPos, VISUAL_MIDPOINT);
        SetPositions(_playerPos.position, midway, _wallPos);
    }

    private void RetractRope()
    {
        if (_currentRopeEndPos == _wallPos)
        {
            InitiateLineRenderer();
            Vector3 midway = Vector3.Lerp(_playerPos.position, _wallPos, VISUAL_MIDPOINT);
            _currentRopeEndPos = midway;
        }

        float distance = Vector3.Distance(_playerPos.position, _wallPos);
        _currentRopeEndPos = Vector3.MoveTowards(_currentRopeEndPos, _playerPos.position, Time.deltaTime * distance * 10);
        SetPositions(_playerPos.position, _currentRopeEndPos);
        if (Vector3.Distance(_currentRopeEndPos, _playerPos.position) < 0.5f)
        {
            gameObject.SetActive(false);
        }
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
