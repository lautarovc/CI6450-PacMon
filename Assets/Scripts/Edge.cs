using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : ScriptableObject {

    public Node nodo1;
    public Node nodo2;
    public float peso;

    public void loadData(Node n1, Node n2, float p)
    {
        nodo1 = n1;
        nodo2 = n2;
        peso = p;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
