using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLostTransition : Transition {

    public Transform character;
    private float countdown = 3f;

    public override void makeAction()
    {
        return;
    }

    public override bool isTriggered()
    {
        VisionCone characterVision = character.GetComponent<VisionCone>();

        if (characterVision.detected)
        {
            countdown = 3f;
            return false;
        }

        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            return false;
        }

        countdown = 3f;
        return true;
    }
}