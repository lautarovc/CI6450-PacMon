using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {

    public Transform target;
    private Kinematic targetKinematic;

    public float perfectRadius = 5f;
    public float maxRadius = 8f;
    public float errorRadius = 3f;
    public bool detected = false;
    private bool prevDetection = false;

    public Vector3 soundOrigin;

    // Graph for tests
    private Graph graph;

	// Use this for initialization
	void Start () {
        targetKinematic = target.GetComponent<Kinematic>();

        GameObject graphObj = GameObject.FindGameObjectsWithTag("Graph")[0];
        graph = graphObj.GetComponent<Graph>();
    }
	
	// Update is called once per frame
	void Update () {
        drawRadii();

        if (targetKinematic.jumping && !prevDetection) detected = listen();

        else if (!targetKinematic.jumping) detected = false;

        prevDetection = detected;        

        if (detected)
        {
            Node node = graph.posicionEnNodo(soundOrigin);
            node.drawNode();
        }
    }

    private void OnDrawGizmos()
    {
    }

    private void drawRadii()
    {
        DebugExtension.DebugCircle(transform.position, Vector3.up*2, Color.black, perfectRadius);
        DebugExtension.DebugCircle(transform.position, Vector3.up*2, Color.gray, maxRadius);

    }

    private bool listen()
    {

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= perfectRadius)
        {
            soundOrigin = target.position;
            return true;
        }

        else if (distance <= maxRadius)
        {
            soundOrigin = randomSurroundingPosition(target.position);
            return true;
        }

        return false;
    }

    private Vector3 randomSurroundingPosition(Vector3 pos)
    {
        Vector2 randomPos = Random.insideUnitCircle * errorRadius;
        Vector3 surroundingPos = pos + new Vector3(randomPos.x, 0, randomPos.y);

        if (targetKinematic.outsideMap(surroundingPos)) return randomSurroundingPosition(pos);

        return surroundingPos;
    }
}
