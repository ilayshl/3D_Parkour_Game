using UnityEngine;

public class PlayerMovement
{
    private const float MOVE_SPEED = 10;
    public float Damp = 0.5f;
    public float MoveSpeedLimitMult = 1; //Normally 1
    public float MoveSpeedMult = 1; //Normally 1
    private Transform _orientation;
    private Rigidbody _rb;



    public PlayerMovement(Transform movementOrientation, Rigidbody rb)
    {
        _orientation = movementOrientation;
        _rb = rb;
    }

    public void Move(Vector2 moveInput)
    {
        Vector3 currentInput = (_orientation.forward * moveInput.y) + (_orientation.right * moveInput.x);
        Vector3 forceToAdd = currentInput.normalized * MOVE_SPEED * 350 * Time.fixedDeltaTime;
        _rb.AddForce(forceToAdd * MoveSpeedMult, ForceMode.Force);
    }

    public void LimitSpeed()
    {
        if (_rb.linearVelocity.magnitude > MOVE_SPEED * MoveSpeedLimitMult)
         {
             Vector3 limitVelocity = _rb.linearVelocity.normalized * MOVE_SPEED * MoveSpeedLimitMult;
            _rb.linearVelocity = new Vector3(limitVelocity.x, _rb.linearVelocity.y, limitVelocity.z);
             Debug.Log("Limited speed to "+_rb.linearVelocity);
         }
    }
    
    /*
     private void Jump()
     {
         _playerData.rb.linearVelocity = new Vector3(_playerData.rb.linearVelocity.x, 0, _playerData.rb.linearVelocity.z);
         _playerData.rb.AddForce(transform.up * _playerData.JumpForce, ForceMode.Impulse);
         _readyToJump = false;
     }

     private void ResetJump()
     {
         _readyToJump = true;
     }

      */
}