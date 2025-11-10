using UnityEngine;

public class PlayerMovement
{
    private const float MOVE_SPEED = 10;
    private const float JUMP_FORCE = 12;
    private float _moveSpeedLimitMult = 1; //Normally 1
    private float _moveSpeedMult = 1; //Normally 1
    private PlayerMovementOrientation _orientation;
    private Rigidbody _rb;

    public PlayerMovement(PlayerMovementOrientation orientation, Rigidbody rb)
    {
        _orientation = orientation;
        _rb = rb;
    }
    
    public void SetMovementData(PlayerMovementData data)
    {
        _moveSpeedLimitMult = data.SpeedLimitMult;
        _rb.linearDamping = data.Damp;
        _moveSpeedMult = data.MoveSpeedMult;
    }

    public void Move(Vector2 moveInput)
    {
        Vector3 currentInput = (_orientation.transform.forward * moveInput.y) + (_orientation.transform.right * moveInput.x);
        Vector3 forceToAdd = currentInput.normalized * MOVE_SPEED * 350 * Time.fixedDeltaTime;
        _rb.AddForce(forceToAdd * _moveSpeedMult, ForceMode.Force);
    }

    public void LimitSpeed()
    {
        if (_rb.linearVelocity.magnitude > MOVE_SPEED * _moveSpeedLimitMult)
         {
             Vector3 limitVelocity = _rb.linearVelocity.normalized * MOVE_SPEED * _moveSpeedLimitMult;
            _rb.linearVelocity = new Vector3(limitVelocity.x, _rb.linearVelocity.y, limitVelocity.z);
         }
    }
    public void Jump()
     {
         _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(_orientation.transform.up * JUMP_FORCE, ForceMode.Impulse);
         //_readyToJump = false;
     }

    
    /* public void ResetJump()
    {
        _readyToJump = true;
    } */
}