using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollowing))]
public class AStar : SteeringBehavior {

    private PathFollowing pathfollow;

    public Graph graph;
    public Transform target;
    public bool start;

	// Use this for initialization
	void Start () {
        pathfollow = transform.GetComponent<PathFollowing>();
	}
	
	// Update is called once per frame
	void Update () {
		if (start)
        {
            //Debug.Log(graph.posicionEnNodo(target.position).id);
            List<int> nodos = graph.aStar(transform.position, target.position);

            foreach (int i in nodos)
            {
                graph.nodos[i].drawNode();
            }

            pathfollow.path = nodos;

        }
        else
        {
            pathfollow.path = new List<int>();
        }
	}
}
