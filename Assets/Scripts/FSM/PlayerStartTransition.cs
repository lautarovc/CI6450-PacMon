using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartTransition : Transition {

    public Transform player;

    public override void makeAction()
    {
        return;
    }

    public override bool isTriggered()
    {
        return player.position.z > -3;
    }
}
