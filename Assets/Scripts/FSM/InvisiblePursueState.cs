using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiblePursueState : PursueState {

    public override void makeAction()
    {   
        aStar.target = player;
        aStar.start = true;
        character.localScale = new Vector3(5, 5, 5);
    }
}
