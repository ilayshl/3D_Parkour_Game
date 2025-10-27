using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private protected State _currentState;

    public void ChangeState(State newState)
    {
        if(newState == null)
        {
            Debug.LogError($"[{this.name}] State transitioned to can't be null!");
            return;
        }
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }
}
