using UnityEngine;

public class PlayerMovementManager
{
    PlayerMovement _playerMovement;
    PlayerSwing _playerSwing;
    PlayerAbility _playerAbility;

    public PlayerMovementManager()
    {
        Initialize();
    }

    private void Initialize()
    {
        StartCoroutine(nameof(CheckForSwingPoints));
    }
    
    public void SetMovementData(PlayerMovementData data)
    {
        _playerMovement.SetMovementData(data);
    }

    public void HandleMove()
    {
        _playerMovement.Move(MoveInput);
        _playerMovement.LimitSpeed();
    }

    public void HandleSwingStart()
    {
        _playerSwing.CheckSwing();
    }

    public void HandleSwingMove()
    {
        _playerSwing.SwingMove(MoveInput);
    }

    public void HandleSwingStop()
    {
        _playerSwing.StopSwing();
        StartCoroutine(nameof(CheckForSwingPoints));
    }
    
    public void HandleJump()
    {
        _playerMovement.Jump();
    }

    private IEnumerator CheckForSwingPoints()
    {
        while (!_playerSwing.IsSwinging)
        {
            _playerSwing.CheckForSwingPoints();
            yield return null;
        }
    }
    
    public void ShortenRope()
    {
        _playerSwing.ShortenRope();
    }
}
