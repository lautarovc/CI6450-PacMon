using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : State {

    public AStar aStar;
    public Transform player;
    public Transform character;

    public override void makeEntryAction()
    {
        return;
    }

    public override void makeAction()
    {
        aStar.target = player;
        aStar.start = true;
        character.localScale = new Vector3(90, 90, 90);
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
