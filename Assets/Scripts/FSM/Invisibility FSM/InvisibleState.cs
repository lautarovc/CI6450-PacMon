using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleState : State {

    public Transform character;

    public override void makeEntryAction()
    {
        return;
    }

    public override void makeAction()
    {   
        character.localScale = new Vector3(5, 5, 5);
    }

    public override void makeExitAction()
    {
        return;
    }

    public override List<Transition> getTransitions()
    {
        return transitions;
    }
}
