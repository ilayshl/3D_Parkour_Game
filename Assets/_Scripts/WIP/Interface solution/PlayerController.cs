using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputReader InputReader { get; private set; }
    public PlayerSwing PlayerSwing { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public float CurrentVelocity { get; private set; }


    void Awake()
    {
        InputReader = GetComponent<InputReader>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CurrentVelocity = Rigidbody.linearVelocity.magnitude;
        GetInput();
    }

    private void GetInput()
    {
        MoveInput = InputReader.MovementInput;
        LookInput = InputReader.LookInput;
    }

     private void LimitSpeed()
    {
        if (Rigidbody.linearVelocity.magnitude > LimitedSpeedValue())
        {
            Vector2 limitVelocity = CurrentVelocity.normalized * _playerData.MoveSpeed * _speedLimitMult;
            Rigidbody.linearVelocity = new Vector3(limitVelocity.x, _playerData.rb.linearVelocity.y, limitVelocity.y);
        }
    }
}
