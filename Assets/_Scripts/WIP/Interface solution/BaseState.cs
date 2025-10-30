using UnityEngine;

public abstract class BaseState : IState
{
    //protected readonly PlayerController player;
    protected readonly StateMachine stateMachine;

    public BaseState(StateMachine stateMachine)
    {
        //this.player = player;
        this.stateMachine = stateMachine;
    } 
    
    public void OnEnter()
    {
        //do something
    }

    public void Update()
    {
        //do something
    }

    public void FixedUpdate()
    {
        //do something
    }

    public void OnExit()
    {
        //do something
    }
}
