using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : FSM {

    void Awake()
    {
        PatrolState patrol = ScriptableObject.CreateInstance<PatrolState>();
        patrol.aStar = GetComponent<AStar>();
        patrol.graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        patrol.strategyPos = new List<int>(new int[] {192, 165, 235, 118 });

        RotateState patrolRotation = ScriptableObject.CreateInstance<RotateState>();
        patrolRotation.graph = patrol.graph;

        PursueState visionPursue = ScriptableObject.CreateInstance<PursueState>();
        visionPursue.aStar = patrol.aStar;
        visionPursue.graph = patrol.graph;

        PursueState hearingPursue = ScriptableObject.CreateInstance<PursueState>();
        hearingPursue.aStar = visionPursue.aStar;
        hearingPursue.graph = visionPursue.graph;

        StartRotationTransition rotationTrans = ScriptableObject.CreateInstance<StartRotationTransition>();
        rotationTrans.targetState = patrolRotation;

        PlayerInSightTransition visionTrans = ScriptableObject.CreateInstance<PlayerInSightTransition>();
        visionTrans.targetState = visionTrans.pursueState = visionPursue;

        PlayerHeardTransition heardTrans = ScriptableObject.CreateInstance<PlayerHeardTransition>();
        heardTrans.targetState = heardTrans.pursueState = hearingPursue;

        PlayerLostTransition lostTrans = ScriptableObject.CreateInstance<PlayerLostTransition>();
        lostTrans.targetState = patrol;

        StopHearingTransition stopHearingTrans = ScriptableObject.CreateInstance<StopHearingTransition>();
        stopHearingTrans.targetState = patrol;

        rotationTrans.character = transform;
        visionTrans.character = transform;
        heardTrans.character = transform;
        lostTrans.character = transform;
        stopHearingTrans.character = transform;

        patrolRotation.character = transform;
        visionPursue.character = transform;
        hearingPursue.character = transform;

        patrol.transitions.Add(rotationTrans);
        patrol.transitions.Add(visionTrans);
        patrol.transitions.Add(heardTrans);

        patrolRotation.transitions.Add(visionTrans);
        patrolRotation.transitions.Add(heardTrans);

        visionPursue.transitions.Add(visionTrans);
        visionPursue.transitions.Add(lostTrans);

        hearingPursue.transitions.Add(visionTrans);
        hearingPursue.transitions.Add(heardTrans);
        hearingPursue.transitions.Add(stopHearingTrans);

        states = new List<State>();
        states.Add(patrol);
        states.Add(patrolRotation);
        states.Add(visionPursue);
        states.Add(hearingPursue);

        initialState = patrol;

    }
}
