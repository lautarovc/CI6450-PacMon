using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FastSeek),typeof(Arrive))]
public class PathFollowing : SteeringBehavior {

    private FastSeek seek;
    private Arrive arrive;
    private Graph graph;

    public List<int> path;
    public int currentNode = 0;

	// Use this for initialization
	void Start () {
        seek = transform.GetComponent<FastSeek>();
        arrive = transform.GetComponent<Arrive>();
        GameObject graphObj = GameObject.FindGameObjectsWithTag("Graph")[0];
        graph = graphObj.GetComponent<Graph>();

        path = new List<int>();
    }

    // Update is called once per frame
    void Update () {
        //currentNode = graph.posicionEnNodo(transform.position).id;
        bool moves = getSteering();
        if (moves)
        {
            characterKinematic.FastMove();
        }
        else
        {
            linearStop();
        }
    }

    public override bool getSteering()
    {
        Vector3 target = new Vector3();
        bool isTarget = false;
        bool moves = false;

        if (path.Count > 1)
        {   
            int nodeId = path[currentNode+1];
            target = graph.nodos[nodeId].centro;
            isTarget = true;
        }
        

        if (isTarget)
        {
            if (false && currentNode == path.Count-1)
            {
                moves = arrive.getSteering(target);
                linearSteering = arrive.linearSteering;
                angularSteering = arrive.angularSteering;
            }
            else
            {
                moves = seek.getSteering(target);
                linearSteering = seek.linearSteering;
                angularSteering = seek.angularSteering;
            }
        }

        return moves;
    }
}
