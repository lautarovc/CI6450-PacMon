using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSightTransition : Transition {

    public Transform character;
    public PursueState pursueState;


    public override void makeAction()
    {   
        VisionCone characterVision = character.GetComponent<VisionCone>();
        pursueState.target = characterVision.target.position;
        pursueState.targetTransform = characterVision.target;
        return;
    }

    public override bool isTriggered()
    {
        VisionCone characterVision = character.GetComponent<VisionCone>();

        return characterVision.detected;
    }
}
