using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RotateState : State {

    public Transform character;
    public Graph graph;

    private List<Vector3> rotations;
    private int currentRotation;
    private float degree; 

    public override void makeEntryAction()
    {
        Kinematic characterKinematic = character.GetComponent<Kinematic>();

        Vector3 upPosition = character.position + (Vector3.forward * 3f);
        Vector3 rightPosition = character.position + (Vector3.right * 3f);
        Vector3 downPosition = character.position + (Vector3.back * 3f);
        Vector3 leftPosition = character.position + (Vector3.left * 3f);

        rotations = new List<Vector3>(new Vector3[] { upPosition, rightPosition, downPosition, leftPosition });
        List<Vector3> realRotations = new List<Vector3>();

        foreach (Vector3 position in rotations)
        {
            bool nearWall = false;

            if (characterKinematic.outsideMap(position))
            {
                continue;
            }

            for (int i = 0; i < 108; i++) {
                float d = HandleUtility.DistancePointLine(graph.nodos[i].centro, character.position, position);
                if (d < 1)
                {
                    nearWall = true;
                    break;
                }
            }

            if (!nearWall) realRotations.Add(position);
        }

        rotations = realRotations;
    }

    public override void makeAction()
    {
        if (currentRotation >= rotations.Count)
        {
            currentRotation = 0;
        }

        Kinematic characterKinematic = character.GetComponent<Kinematic>();

        Face looking = character.GetComponent<Face>();

        if (characterKinematic.rotation != 0f && looking.enabled) return;

        character.GetComponent<LookWhereYoureGoing>().enabled = false;

        if (rotations.Count > 0)
        {
            looking.targetPos = rotations[currentRotation];
            looking.enabled = true;
        }
        currentRotation += 1;
    }

    public override void makeExitAction()
    {
        character.GetComponent<Face>().enabled = false;
        character.GetComponent<LookWhereYoureGoing>().enabled = true;

        return;
    }

    public override List<Transition> getTransitions()
    {
        return transitions;
    }
}
