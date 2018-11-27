using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Priority_Queue;

public class Graph : MonoBehaviour {

    public Dictionary<int, Node> nodos = new Dictionary<int, Node>();
    public float[,] lados;

    public int nodeToDraw;
    public List<int> adjacents;


    public void edgesFromNodes()
    {
        int nodeSize = nodos.Count();

        for (int i = 0; i < nodeSize; i++)
        {
            for (int j = 0; j < nodeSize; j++)
            {
                if (i == j)
                {
                    continue;
                }

                int common = verticesComunes(nodos[i].puntos, nodos[j].puntos, 0.1f);

                if (common > 1)
                {
                    float peso = 1;

                    if (nodos[i].altura != nodos[j].altura)
                    {
                        peso = 5000;
                    }
                    else
                    {
                        peso = Vector3.Distance(nodos[i].centro, nodos[j].centro);
                    }

                    //Edge lado = ScriptableObject.CreateInstance<Edge>();
                    //lado.loadData(nodos[i], nodos[j], peso);
                    //lados.Add(lado);

                    lados[i, j] = peso;
                }

                else
                {
                    lados[i, j] = -1;
                }

            }

        }
    }

    public void loadWallNodes(string wallTag, int[] verticalNodes)
    {
        GameObject walls = GameObject.FindGameObjectsWithTag(wallTag)[0];
        List<List<Vector3>> children = new List<List<Vector3>>();

        foreach (Transform child in walls.transform)
        {
            Vector3[] childVerts = child.gameObject.GetComponent<MeshFilter>().mesh.vertices;
            List<Vector3> topVerts = new List<Vector3>();

            foreach (Vector3 vertex in childVerts)
            {
                Vector3 vertexPos = child.TransformPoint(vertex);
                if (vertexPos.y >= 2 && !topVerts.Contains(vertexPos))
                {
                    topVerts.Add(vertexPos);
                }
            }

            children.Add(topVerts);
        }

        int nodeId = nodos.Keys.Count();

        foreach (List<Vector3> childVertices in children) {

            for (int i = 0; i < childVertices.Count()-2; i ++)
            {
                Vector3[] vertices = new Vector3[3];

                vertices[0] = childVertices[i];
                vertices[1] = childVertices[i + 1];
                vertices[2] = childVertices[i + 2];

                string normal = "ninguna";
                int alt = 0;

                if (verticalNodes.Contains(nodeId))
                {
                    normal = "horizontal";
                    alt = 2;
                }
                else
                {
                    normal = "vertical";
                    alt = 2;
                }
                Node newNode = ScriptableObject.CreateInstance<Node>();
                newNode.loadData(nodeId, vertices, false, normal, alt);
                //Node newNode.Node(i, vertices, false, normal, alt);
                nodos.Add(nodeId, newNode);
                nodeId++;

                //Debug.Log(newNode.id.ToString() + ": " + newNode.normal);
            }
        }

    }

    public void loadFloorNodes(string floorTag, int alt)
    {
        GameObject walls = GameObject.FindGameObjectsWithTag(floorTag)[0];
        List<List<Vector3>> children = new List<List<Vector3>>();

        foreach (Transform child in walls.transform)
        {
            Vector3[] childVerts = child.gameObject.GetComponent<MeshFilter>().mesh.vertices;

            List<Vector3> topVerts = new List<Vector3>();

            foreach (Vector3 vertex in childVerts)
            {
                Vector3 vertexPos = child.TransformPoint(vertex);
                bool heightCheck = false;

                if (alt == 2) {
                    heightCheck = vertexPos.y >= 2;
                }
                else
                {
                    heightCheck = vertexPos.y < 2;
                }

                if (heightCheck && !topVerts.Contains(vertexPos))
                {
                    topVerts.Add(vertexPos);
                }
            }

            children.Add(topVerts);
        }

        int nodeId = nodos.Keys.Count();

        foreach (List<Vector3> childVertices in children)
        {

            for (int i = 0; i < childVertices.Count() - 2; i++)
            {
                Vector3[] vertices = new Vector3[3];

                if (i == 0)
                {
                    vertices[0] = childVertices[i];
                    vertices[1] = childVertices[i + 1];
                    vertices[2] = childVertices[i + 2];
                }

                if (i == 1)
                {
                    vertices[0] = childVertices[i-1];
                    vertices[1] = childVertices[i+1];
                    vertices[2] = childVertices[i + 2];
                }

                string normal = "ninguna";

                Node newNode = ScriptableObject.CreateInstance<Node>();
                newNode.loadData(nodeId, vertices, false, normal, alt);
                //Node newNode.Node(i, vertices, false, normal, alt);
                nodos.Add(nodeId, newNode);
                nodeId++;

                //Debug.Log(newNode.id.ToString() + ": " + newNode.normal);
            }
        }

    }

    public List<Node> adyacentes(Node nodo)
    {
        List<Node> listaAdyacentes = new List<Node>();

        for(int i=0; i < nodos.Count(); i++)
        {
            float peso = lados[nodo.id, i];

            if (peso > 0)
            {
                listaAdyacentes.Add(nodos[i]);
            }

        }

        return listaAdyacentes;

    }

    public List<int> adyacentesId(Node nodo)
    {
        List<int> listaAdyacentes = new List<int>();

        for (int i = 0; i < nodos.Count(); i++)
        {
            float peso = lados[nodo.id, i];

            if (peso > 0)
            {
                listaAdyacentes.Add(nodos[i].id);
            }

        }

        return listaAdyacentes;

    }

    public Node posicionEnNodo(Vector3 pos)
    {
        foreach(Node nodo in nodos.Values)
        {
            bool dentro = dentroTriangulo(nodo.puntos[0], nodo.puntos[1], nodo.puntos[2], pos, 0.1f);

            if (dentro)
            {
                return nodo;
            }
        }

        return null;
    }
    
    public List<int> aStar(Vector3 current, Vector3 goal)
    {
        FastPriorityQueue<PriorityNode> open = new FastPriorityQueue<PriorityNode>(350);
        Dictionary<int, bool> visited = new Dictionary<int, bool>();
        Dictionary<int, int> parents = new Dictionary<int, int>();

        Node actual = posicionEnNodo(current);
        Node end = posicionEnNodo(goal);
        float g = 0;
        float f = g + 0;
        PriorityNode actualRecord = new PriorityNode(actual.id, g, -1);

        open.Enqueue(actualRecord, f);

        while (open.Count() > 0)
        {
            actualRecord = open.Dequeue();
            actual = nodos[actualRecord.id];

            bool visit;
            if (visited.TryGetValue(actual.id, out visit)) continue;

            parents.Add(actual.id, actualRecord.parent);
            visited.Add(actual.id, true);

            if (actual.id == end.id) break;

            foreach (Node sucesor in adyacentes(actual))
            {
                g = actualRecord.g + lados[actual.id, sucesor.id];
                f = g + Vector3.Distance(sucesor.centro, end.centro);

                PriorityNode sucesorRecord = new PriorityNode(sucesor.id, g, actual.id);

                open.Enqueue(sucesorRecord, f);              
            }
        }

        if(actual.id != end.id)
        {
            Debug.Log("Error: goal not found.");
            return null;
        }

        return reconstruccionCamino(actual.id, parents);

    }

    // Helper functions

    private float areaTriangulo(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return Mathf.Abs((p1.x * (p2.z - p3.z) + p2.x * (p3.z - p1.z) + p3.x * (p1.z - p2.z)) / 2);
    }

    private bool dentroTriangulo(Vector3 a, Vector3 b, Vector3 c, Vector3 pos, float epsilon)
    {
        /* Calculate area of triangle ABC */
        float A = areaTriangulo(a, b, c);

        /* Calculate area of triangle PBC */
        float A1 = areaTriangulo(pos, b, c);

        /* Calculate area of triangle PAC */
        float A2 = areaTriangulo(pos, a, c);

        /* Calculate area of triangle PAB */
        float A3 = areaTriangulo(pos, a, b);

        /* Check if sum of A1, A2 and A3 is same as A */
        bool inside = Mathf.Abs(A1 + A2 + A3 - A) <= epsilon;

        return inside;
    }

    private int verticesComunes(Vector3[] puntos1, Vector3[] puntos2, float epsilon)
    {
        List<Vector3[]> vertices = new List<Vector3[]>();

        foreach (Vector3 vertex1 in puntos1)
        {
            foreach (Vector3 vertex2 in puntos2)
            {
                float distance = Vector3.Distance(vertex1, vertex2);

                if (distance <= epsilon)
                {
                    Vector3[] common = new Vector3[] { vertex1, vertex2 };
                    vertices.Add(common);
                }

            }
        }

        if (vertices.Count() == 1)
        {
            foreach (Vector3 vertex1 in puntos1)
            {
                if (vertex1 == vertices[0][0])
                {
                    continue;
                }

                foreach (Vector3 vertex2 in puntos2)
                {
                    if (vertex2 == vertices[0][1])
                    {
                        continue;
                    }

                    float d = HandleUtility.DistancePointLine(vertex2, vertices[0][0], vertex1);
                    float d1 = HandleUtility.DistancePointLine(vertex1, vertices[0][1], vertex2);


                    if (d <= epsilon || d1 <= epsilon)
                    {
                        vertices.Add(new Vector3[] { vertex1, vertex2 });
                    }

                }
            }
        }

        return vertices.Count();
    }

    private List<int> reconstruccionCamino(int nodoActual, Dictionary<int,int> padres)
    {
        List<int> camino = new List<int>();
        camino.Add(nodos[nodoActual].id);

        while (padres[nodoActual] != -1)
        {
            nodoActual = padres[nodoActual];
            camino.Insert(0, nodos[nodoActual].id);
        }

        return camino;
    }
    
    // Debug functions

    public void drawAllNodes()
    {
        foreach (Node node in nodos.Values)
        {
            node.drawNode();
        }
    }

    public void drawEdges()
    {
        for (int i = 0; i < nodos.Count(); i++)
        {
            for (int j = i; j < nodos.Count(); j++)
            {
                if (lados[i, j] > 0)
                {
                    Debug.DrawRay(nodos[i].centro, nodos[j].centro - nodos[i].centro, Color.blue, 0.0f, false);

                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        loadWallNodes("WallMesh", new[] {3,4,5,6,7,8,9,11,24,26,27,28,29,30,31,32});
        loadWallNodes("WallMesh1", new[] { 40, 42, 43, 44, 45, 46, 47, 48, 49, 52, 54, 55, 56, 57, 58, 59, 61 });
        loadWallNodes("WallMesh2", new[] { 65, 74, 76, 77, 78, 79, 81, 85, 86, 87, 88, 89, 90, 91, 93, 104, 106, 107 });
        loadFloorNodes("FloorMesh", 0);
        loadFloorNodes("FloorMesh1", 2);
        loadFloorNodes("FloorMesh2", 2);
        loadFloorNodes("FloorMesh3", 2);
        lados = new float[nodos.Count(), nodos.Count()];
        edgesFromNodes();
    }
	
	// Update is called once per frame
	void Update () {
        //drawAllNodes();

        adjacents = adyacentesId(nodos[nodeToDraw]);

        nodos[nodeToDraw].drawNode();
        drawEdges();
    }

    // Helper Classes
    public class PriorityNode : FastPriorityQueueNode
    {
        public int id;
        public float g;
        public int parent;

        public PriorityNode(int nodeId, float distance, int parId)
        {
            id = nodeId;
            g = distance;
            parent = parId;
        }
    }
}
