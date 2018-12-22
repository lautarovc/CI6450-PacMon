using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisionCone : MonoBehaviour {

    public Transform target;
    public float targetRadius = 1.5f;
    public Vector3[] vertices = new Vector3[3];
    private Vector3[] relativeVertices = new Vector3[3];

    public float vertexDistance = 5f;
    public float vertexAngle = 30f;
    public bool detected = false;

    private Graph graph;

    // Use this for initialization
    void Start () {
        vertices[0] = transform.position;
        relativeVertices[0] = Vector3.zero;

        GameObject graphObj = GameObject.FindGameObjectsWithTag("Graph")[0];
        graph = graphObj.GetComponent<Graph>();
    }

    // Update is called once per frame
    void Update () {
        defineVertices();
        rotateVertices();

        detected = inSight(target.position);

        drawCone();
    }

    private void defineVertices()
    {
        Vector3 vertexPoint = transform.position + (Vector3.forward * vertexDistance);
        Vector3 relativeVertex = vertexPoint - transform.position;

        // Rotate side rays
        relativeVertices[1] = Quaternion.AngleAxis(-vertexAngle, Vector3.up) * relativeVertex;
        relativeVertices[2] = Quaternion.AngleAxis(vertexAngle, Vector3.up) * relativeVertex;

    }

    private void rotateVertices()
    {
        float characterAngle = transform.rotation.eulerAngles.y;

        relativeVertices[1] = Quaternion.AngleAxis(characterAngle, Vector3.up) * relativeVertices[1];
        relativeVertices[2] = Quaternion.AngleAxis(characterAngle, Vector3.up) * relativeVertices[2];

        vertices[0] = transform.position;
        vertices[1] = transform.position + relativeVertices[1];
        vertices[2] = transform.position + relativeVertices[2];
    }

    private void drawCone()
    {
        Color color = Color.cyan;
        if (detected) color = Color.magenta;

        Debug.DrawRay(vertices[0], relativeVertices[1], color, 0.0f, false);
        Debug.DrawRay(vertices[1], vertices[2] - vertices[1], color, 0.0f, false);
        Debug.DrawRay(vertices[0], relativeVertices[2], color, 0.0f, false);
    }

    public bool inCone(Vector3 pos, float epsilon)
    {
        /* Calculate area of triangle ABC */
        float A = triangleArea(vertices[0], vertices[1], vertices[2]);

        /* Calculate area of triangle PBC */
        float A1 = triangleArea(pos, vertices[1], vertices[2]);

        /* Calculate area of triangle PAC */
        float A2 = triangleArea(pos, vertices[0], vertices[2]);

        /* Calculate area of triangle PAB */
        float A3 = triangleArea(pos, vertices[0], vertices[1]);

        /* Check if sum of A1, A2 and A3 is same as A */
        bool inside = Mathf.Abs(A1 + A2 + A3 - A) <= epsilon;

        return inside;
    }

    public bool inSight(Vector3 pos)
    {
        if (!inCone(pos, 3f)) {
            return false;
        }

        for (int i=0;i<108;i++)
        {
            Node wallNode = graph.nodos[i];

            if (Vector3.Distance(wallNode.centro, vertices[0]) > vertexDistance) continue;

            // Check if line position-pos intersects with any of the lines in node
            bool intersectsSide1 = FasterLineSegmentIntersection(vertices[0], pos, wallNode.puntos[0], wallNode.puntos[1]);
            bool intersectsSide2 = FasterLineSegmentIntersection(vertices[0], pos, wallNode.puntos[1], wallNode.puntos[2]);
            bool intersectsSide3 = FasterLineSegmentIntersection(vertices[0], pos, wallNode.puntos[2], wallNode.puntos[0]);

            if (intersectsSide1 || intersectsSide2 || intersectsSide3)
            {
                return false;
            }
        }

        return true;
    }

    // Helper Functions

    private float triangleArea(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return Mathf.Abs((p1.x * (p2.z - p3.z) + p2.x * (p3.z - p1.z) + p3.x * (p1.z - p2.z)) / 2);
    }

    // Faster Line Segment Intersection by Franklin Antonio: http://www.stefanbader.ch/faster-line-segment-intersection-for-unity3dc/
    private bool FasterLineSegmentIntersection(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
    {
        Vector2 p1 = new Vector2(a1.x, a1.z);
        Vector2 p2 = new Vector2(a2.x, a2.z);
        Vector2 p3 = new Vector2(b1.x, b1.z);
        Vector2 p4 = new Vector2(b2.x, b2.z);

        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = alphaDenominator; /*2013/07/05, fix by Deniz*/

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;
    }
}
