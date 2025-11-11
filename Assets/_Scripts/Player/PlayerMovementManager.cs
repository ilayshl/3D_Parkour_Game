using System;
using UnityEngine;

/// <summary>
/// Facade AND builder script that holds all PlayerMovement, PlayerSwing and PlayerAbility instanced scripts.
/// </summary>
public class PlayerMovementManager{
    public event Action SwingUpdateEvent;
    public Vector2 MoveInput;
    public Rigidbody Rigidbody { get; private set; }
    private PlayerMovement _movement;
    private PlayerSwing _swing;
    private PlayerAbility _ability;

    public PlayerMovementManager(Rigidbody rb)
    {
        Rigidbody = rb;
    }

    #region Builder initialization
    public void InitializeMovement(PlayerMovementOrientation orientation, Rigidbody rb)
    {
        _movement = new(orientation, rb);
    }

    public void InitializeSwing(PlayerSwingData data, Rigidbody rb)
    {
        _swing = new(data, rb);
    }

    public void InitializeAbility(IAbility ability)
    {
        _ability = new(ability);
    }
    #endregion

    public void SetMovementData(PlayerMovementData data)
    {
        _movement.SetMovementData(data);
    }
    
    public void Update()
    {
        _movement.LimitSpeed();
        _swing.CheckForSwingPoints();
    }

    public void HandleMove()
    {
        _movement.Move(MoveInput);
    }

    public void HandleSwingStart()
    {
        _swing.CheckSwing();
        SwingUpdateEvent?.Invoke();
    }

    public void HandleSwingMove()
    {
        _swing.SwingMove(MoveInput);
    }

    public void HandleSwingStop()
    {
        _swing.StopSwing();
        SwingUpdateEvent?.Invoke();
    }

    public void HandleJump()
    {
        _movement.Jump();
    }

    public void ShortenRope()
    {
        _swing.ShortenRope();
    }
}
