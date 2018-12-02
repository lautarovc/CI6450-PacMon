using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandbyState : State {

    public AStar aStar;

    public override void makeEntryAction()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("StartTile");

        foreach (GameObject tile in tiles)
        { 
            tile.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        }
    }

    public override void makeAction()
    {
        aStar.start = false;
    }

    public override void makeExitAction()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("StartTile");

        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
        }
    }

    public override List<Transition> getTransitions()
    {
        return transitions;
    }
}
