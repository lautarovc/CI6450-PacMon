using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {

    public List<State> states;
    public State initialState;
    public State currentState;

	// Use this for initialization
	void Start () {
        currentState = initialState;
        currentState.makeEntryAction();
    }
	
	// Update is called once per frame
	void Update () {
        Transition triggeredTransition = null;
        State targetState = null;

        foreach (Transition trans in currentState.getTransitions())
        {
            if (trans.isTriggered())
            {
                triggeredTransition = trans;
                break;
            }
        }

        if (triggeredTransition != null)
        {   
            targetState = triggeredTransition.targetState;
            Debug.Log(targetState);
            currentState.makeExitAction();
            triggeredTransition.makeAction();
            targetState.makeEntryAction();

            currentState = targetState;
            return;
        }
        else
        {
            currentState.makeAction();
            return;
        }
	}
}
