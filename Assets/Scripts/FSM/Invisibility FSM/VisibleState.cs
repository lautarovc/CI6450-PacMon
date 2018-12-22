using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleState : InvisibleState {

    public override void makeAction()
    {
        character.localScale = new Vector3(90, 90, 90);
    }
}
