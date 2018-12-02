using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpTransition : PlayerStartTransition {

    public override bool isTriggered()
    {
        bool jumping = player.GetComponent<Kinematic>().jumping || player.GetComponent<Kinematic>().ground == 2;

        return jumping;
    }
}
