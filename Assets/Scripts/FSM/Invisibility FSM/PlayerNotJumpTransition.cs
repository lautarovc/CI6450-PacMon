using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotJumpTransition : PlayerJumpTransition {

    public override bool isTriggered()
    {
        return !base.isTriggered();
    }
}
