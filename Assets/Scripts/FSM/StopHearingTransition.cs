using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopHearingTransition : Transition {

    public Transform character;

    public override void makeAction()
    {
        return;
    }

    public override bool isTriggered()
    {
        PathFollowing pathfollowing = character.GetComponent<PathFollowing>();
        int pathLength = pathfollowing.path.Count;

        Kinematic characterKinematic = character.GetComponent<Kinematic>();
        

        return (pathfollowing.path != null && pathLength <= 1) || characterKinematic.velocity == Vector3.zero;
    }
}
