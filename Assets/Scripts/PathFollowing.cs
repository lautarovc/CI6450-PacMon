using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FastSeek))]
public class PathFollowing : SteeringBehavior {

    private FastSeek seek;
    private Graph graph;

    public List<int> path;
    public int currentNode = 0;
    public int graphNode;

	// Use this for initialization
	void Start () {
        seek = transform.GetComponent<FastSeek>();

        GameObject graphObj = GameObject.FindGameObjectsWithTag("Graph")[0];
        graph = graphObj.GetComponent<Graph>();
        path = new List<int>();

        graphNode = graph.posicionEnNodo(transform.position).id;
        graph.nodos[graphNode].ocupado = true;

    }

    // Update is called once per frame
    void Update () {
        
        if (path.Count > 0 && path[currentNode] != graphNode)
        {
            graph.nodos[graphNode].ocupado = false;
            graphNode = graph.posicionEnNodo(transform.position).id;
            graph.nodos[graphNode].ocupado = true;
        }
        else
        {
            graph.nodos[graphNode].ocupado = true;
        }

        //bool moves = getSteering();
        //if (moves)
        //{
        //    characterKinematic.FastMove();
        //}
        //else
        //{
        //    linearStop();
        //}
    }

    public override bool getSteering()
    {
        Vector3 target = new Vector3();
        bool isTarget = false;
        bool moves = false;

        if (path.Count > 1)
        {   
            int nodeId = path[currentNode+1];
            if (graph.nodos[nodeId].normal != "ninguna") return moves;
            target = graph.nodos[nodeId].centro;
            isTarget = true;
        }
        

        if (isTarget)
        {
            moves = seek.getSteering(target);
            linearSteering = seek.linearSteering;
            angularSteering = seek.angularSteering;
        }

        return moves;
    }
}
