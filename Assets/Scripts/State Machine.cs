using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State : MonoBehaviour
{
    public abstract State RunCurrentState();

}
public class StateMachine : MonoBehaviour
{
    State currentState;
    void Update()
    {
        RunStateMachine();
    }
    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();
    }
    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }
}

