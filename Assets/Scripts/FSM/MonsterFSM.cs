using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : FSM {

    void Awake()
    {
        StandbyState standby = ScriptableObject.CreateInstance<StandbyState>();
        standby.aStar = GetComponent<AStar>();

        PursueState pursue = ScriptableObject.CreateInstance<PursueState>();
        pursue.aStar = standby.aStar;

        InvisiblePursueState invisiblePursue = ScriptableObject.CreateInstance<InvisiblePursueState>();
        invisiblePursue.aStar = pursue.aStar;

        PlayerStartTransition trans1 = ScriptableObject.CreateInstance<PlayerStartTransition>();
        trans1.targetState = pursue;

        PlayerJumpTransition trans2 = ScriptableObject.CreateInstance<PlayerJumpTransition>();
        trans2.targetState = invisiblePursue;

        PlayerNotJumpTransition trans3 = ScriptableObject.CreateInstance<PlayerNotJumpTransition>();
        trans3.targetState = pursue;

        GameObject target = GameObject.FindGameObjectsWithTag("Player")[0];

        trans1.player = target.transform;
        trans2.player = target.transform;
        trans3.player = target.transform;
        pursue.player = target.transform;
        pursue.character = transform;
        invisiblePursue.player = target.transform;
        invisiblePursue.character = transform;

        standby.transitions.Add(trans1);
        pursue.transitions.Add(trans2);
        invisiblePursue.transitions.Add(trans3);

        states = new List<State>();
        states.Add(standby);
        states.Add(pursue);
        states.Add(invisiblePursue);

        initialState = standby;

    }
}
