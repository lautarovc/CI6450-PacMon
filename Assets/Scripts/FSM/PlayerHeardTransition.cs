using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeardTransition : Transition {

    public Transform character;
    public PursueState pursueState;

    public override void makeAction()
    {
        Hearing characterHearing = character.GetComponent<Hearing>();
        pursueState.target = characterHearing.soundOrigin;
        return;
    }

    public override bool isTriggered()
    {
        Hearing characterHearing = character.GetComponent<Hearing>();

        return characterHearing.detected;
    }
}
