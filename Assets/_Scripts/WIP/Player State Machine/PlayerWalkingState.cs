using System;
using UnityEngine;

public class PlayerWalkingState : PlayerMovementState
{
    private Vector3 _input;
    private Rigidbody _rb;
    private Transform _movementOrientation;

    public PlayerWalkingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += Jump;
        stateMachine.InputReader.MoveEvent += GetInput;
        if (_rb == null) _rb = stateMachine.GetComponent<Rigidbody>();
        //if(_movementOrientation == null) _movementOrientation = stateMachine.
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= Jump;
        stateMachine.InputReader.MoveEvent -= GetInput;
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
        Move(fixedDeltaTime);
    }

    private void GetInput(Vector2 input)
    {
        _input = input;
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(_rb.transform.up * 20, ForceMode.Impulse);
    }

    private void Move(float fixedDeltaTime)
    {
        /* moveDirection = movementOrientation.forward * Input.GetAxisRaw("Vertical") + movementOrientation.right * Input.GetAxisRaw("Horizontal");
        Vector3 forceToAdd = moveDirection.normalized * _playerData.MoveSpeed * 15;
        forceToAdd = _isGrounded ? forceToAdd : forceToAdd * groundDrag;
        _playerData.rb.AddForce(forceToAdd, ForceMode.Force); */
    }
}
