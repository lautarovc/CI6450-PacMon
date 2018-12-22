using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State {

    public AStar aStar;
    public Graph graph;
    public List<int> strategyPos;
    private int currentPos;

    public override void makeEntryAction()
    {
        foreach (int pos in strategyPos)
        {
            if (!graph.nodos[pos].ocupado)
            {
                currentPos = pos;
                graph.nodos[currentPos].ocupado = true;

                aStar.target = graph.nodos[pos].centro;
                break;
            }
        }

        aStar.start = true;
    }

    public override void makeAction()
    {
        graph.nodos[currentPos].ocupado = true;
        return;
    }

    public override void makeExitAction()
    {
        graph.nodos[currentPos].ocupado = false;
        return;
    }

    public override List<Transition> getTransitions()
    {
        return transitions;
    }
}
