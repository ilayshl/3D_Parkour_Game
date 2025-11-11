using UnityEngine;

/// <summary>
/// Facade AND builder script that holds all PlayerMovement, PlayerSwing and PlayerAbility instanced scripts.
/// </summary>
public class PlayerControllerFacade{
    public Vector2 MoveInput { get => InputReader.MovementInput; }
    public bool IsGrounded => _movement.CheckForGround();
    public readonly InputReader InputReader;
    public Rigidbody Rigidbody { get; private set; }
    private PlayerMovementLogic _movement;
    private PlayerSwingLogic _swing;
    private PlayerAbilityFacade _ability;

    public PlayerControllerFacade(Rigidbody rb, InputReader input)
    {
        Rigidbody = rb;
        InputReader = input;
    }

    #region Builder initialization
    public void InitializeMovement(PlayerMovementOrientation orientation, Rigidbody rb, LayerMask ground)
    {
        _movement = new(orientation, rb);
        _movement.InitializeGroundSensor(ground);
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
    }

    public void HandleMove()
    {
        _movement.Move(MoveInput);
    }

    public bool CheckSwing()
    {
        return _swing.CheckSwing();
    }
    
    public void HandleSwingStart()
    {
        _swing.CheckSwing();
    }

    public void HandleSwingMove()
    {
        _swing.SwingMove(MoveInput);
    }

    public void HandleSwingStop()
    {
        _swing.StopSwing();
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
