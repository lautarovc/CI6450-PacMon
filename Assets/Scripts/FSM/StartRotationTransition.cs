using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotationTransition : Transition {

    public Transform character;

    public override void makeAction()
    {
        return;
    }

    public override bool isTriggered()
    {
        PathFollowing pathfollowing = character.GetComponent<PathFollowing>();
        int pathLength = pathfollowing.path.Count;

        return (pathfollowing.path != null && pathLength == 1);
    }
}
