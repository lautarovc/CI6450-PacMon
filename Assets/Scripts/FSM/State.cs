using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject {

    public List<Transition> transitions = new List<Transition>();

    public abstract void makeEntryAction();

    public abstract void makeAction();

    public abstract void makeExitAction();

    public abstract List<Transition> getTransitions();
}
