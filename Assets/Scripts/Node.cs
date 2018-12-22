using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : ScriptableObject {

    public int id;
    public Vector3[] puntos = new Vector3[3];
    public Vector3 centro;

    public bool ocupado;
    public string normal;
    public int altura;
    public bool dibujar;
    public float size;

    public void loadData(int i, Vector3[] ps, bool ocup, string nor, int alt)
    {
        id = i;
        puntos[0] = new Vector3(ps[0].x, 0, ps[0].z);
        puntos[1] = new Vector3(ps[1].x, 0, ps[1].z);
        puntos[2] = new Vector3(ps[2].x, 0, ps[2].z);

        ocupado = ocup;
        altura = alt;

        if (nor != "horizontal" && nor != "vertical" && nor != "ninguna")
        {
            Debug.Log("Normal erronea asignada");
            normal = "ninguna";
        }
        else
        {
            normal = nor;
        }

        centro = Centro();
        size = Size();
    }

    public Vector3 Centro()
    {
        float x = (puntos[0].x + puntos[1].x + puntos[2].x)/3;
        float z = (puntos[0].z + puntos[1].z + puntos[2].z)/3;

        return new Vector3(x, 0, z);
    }

    public float Size()
    {
        float distance = 0;

        foreach (Vector3 punto in puntos)
        {
            float pointDistance = Vector3.Distance(centro, punto);
            
            if (pointDistance > distance)
            {
                distance = pointDistance;
            }
        }

        return distance;
    }

    public void drawNode()
    {
        Debug.DrawRay(puntos[0], puntos[1]-puntos[0], Color.red, 0.0f, false);
        Debug.DrawRay(puntos[1], puntos[2]-puntos[1], Color.red, 0.0f, false);
        Debug.DrawRay(puntos[2], puntos[0]-puntos[2], Color.red, 0.0f, false);

    }

    void OnGUI()
    {
        Handles.Label(centro, id.ToString());
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (dibujar)
        {
            drawNode();
        }
	}
}
