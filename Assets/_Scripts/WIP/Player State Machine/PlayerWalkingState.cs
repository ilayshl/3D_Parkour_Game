using System;
using UnityEngine;

public class PlayerWalkingState : PlayerMovementState
{
    private Vector3 input;

    public PlayerWalkingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += Jump;
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= Jump;
    }

    public override void Tick(float deltaTime)
    {
        GetInput();
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
        Move(fixedDeltaTime);
    }

    private void GetInput()
    {
        //Get Input logic
        GetJumpInput();
    }

    private void GetJumpInput()
    {
        //Check for jump
    }

    private void Jump()
    {
        Rigidbody rb = stateMachine.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(rb.transform.up * 20, ForceMode.Impulse);
    }

    private void Move(float fixedDeltaTime)
    {
        Debug.Log(stateMachine.InputReader.MovementInput);
    }
}
