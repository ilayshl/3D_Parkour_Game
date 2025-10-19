using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public InputReader InputReader { get; private set; }

    void Awake()
    {
        InputReader = GetComponent<InputReader>();
    }
}