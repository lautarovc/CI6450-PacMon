using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : State {

    public AStar aStar;
    public Graph graph;
    public Vector3 target;
    public Transform targetTransform = null;
    public Transform character;

    public override void makeEntryAction()
    {
        aStar.target = target;
        aStar.start = true;
    }

    public override void makeAction()
    {   
        if (targetTransform != null) aStar.target = targetTransform.position;
        //character.localScale = new Vector3(90, 90, 90);
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
