using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : ScriptableObject {

    public State targetState;

    public abstract void makeAction();

    public abstract bool isTriggered();
}
