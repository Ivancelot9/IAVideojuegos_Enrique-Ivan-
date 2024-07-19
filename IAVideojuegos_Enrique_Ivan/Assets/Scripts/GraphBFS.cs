using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GraphBFS : MonoBehaviour
{
    public GameObject nodeA;
    public GameObject nodeB;
    public GameObject nodeC;
    public GameObject nodeD;
    public GameObject nodeE;
    public GameObject nodeF;
    public GameObject nodeG;
    public GameObject nodeH;
    public GameObject capsule; // La cápsula que seguirá las visitas de los nodos
    public float moveSpeed = 1.0f; // Velocidad de movimiento de la cápsula
    public float nodeVisitDelay = 1.0f; // Tiempo de espera después de visitar cada nodo

    private Dictionary<string, GameObject> nodeObjects;
    private Dictionary<string, Node> nodes;
    private List<Edge> edges;
    private Node startNode;
    private Node goalNode;

    void Start()
    {
        nodeObjects = new Dictionary<string, GameObject>()
        {
            { "A", nodeA },
            { "B", nodeB },
            { "C", nodeC },
            { "D", nodeD },
            { "E", nodeE },
            { "F", nodeF },
            { "G", nodeG },
            { "H", nodeH }
        };

        nodes = new Dictionary<string, Node>();
        foreach (var nodeId in nodeObjects.Keys)
        {
            nodes[nodeId] = new Node(nodeId);
        }

        edges = new List<Edge>
        {
            new Edge(nodes["A"], nodes["B"]),
            new Edge(nodes["B"], nodes["C"]),
            new Edge(nodes["B"], nodes["D"]),
            new Edge(nodes["A"], nodes["E"]),
            new Edge(nodes["E"], nodes["F"]),
            new Edge(nodes["E"], nodes["G"]),
            new Edge(nodes["E"], nodes["H"])
        };

        startNode = nodes["H"];
        goalNode = nodes["D"];

        ChangeNodeColor(startNode, Color.blue); // Cambia el color del nodo inicial a azul
        ChangeNodeColor(goalNode, Color.green);  // Cambia el color del nodo objetivo a verde
        StartCoroutine(BreadthFirstSearch(startNode, goalNode));
    }

    private IEnumerator BreadthFirstSearch(Node startNode, Node goalNode)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parentMap = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);
        parentMap[startNode] = null;

        // Espera un momento para asegurarse de que el color azul sea visible
        yield return new WaitForSeconds(0.5f);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();
            yield return StartCoroutine(MoveCapsuleToNode(currentNode)); // Mover la cápsula suavemente al nodo

            // Cambia el color del nodo actual a rojo
            ChangeNodeColor(currentNode, Color.red);
            yield return new WaitForSeconds(nodeVisitDelay); // Espera después de visitar cada nodo

            if (currentNode == goalNode)
            {
                Debug.Log("Camino encontrado desde " + startNode.Id + " hasta " + goalNode.Id + " usando BFS:");
                PrintPath(goalNode, parentMap);
                yield break;
            }

            List<Node> neighbors = FindNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    parentMap[neighbor] = currentNode;
                }
            }

            // Cambia el color del nodo solo cuando se está visitando
            ChangeNodeColor(currentNode, Color.white);
        }

        Debug.Log("No se encontró camino desde " + startNode.Id + " hasta " + goalNode.Id + " usando BFS");
    }

    private List<Node> FindNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Edge edge in edges)
        {
            if (edge.A == node)
            {
                neighbors.Add(edge.B);
            }
            if (edge.B == node)
            {
                neighbors.Add(edge.A);
            }
        }
        return neighbors;
    }

    private void ChangeNodeColor(Node node, Color color)
    {
        if (nodeObjects.TryGetValue(node.Id, out GameObject sphere))
        {
            Renderer renderer = sphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }

    private void PrintPath(Node goalNode, Dictionary<Node, Node> parentMap)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = parentMap.ContainsKey(currentNode) ? parentMap[currentNode] : null;
        }
        path.Reverse();

        foreach (Node node in path)
        {
            Debug.Log(node.Id);
        }
    }

    private IEnumerator MoveCapsuleToNode(Node node)
    {
        if (nodeObjects.TryGetValue(node.Id, out GameObject sphere))
        {
            Vector3 startPosition = capsule.transform.position;
            Vector3 endPosition = sphere.transform.position;
            float journeyLength = Vector3.Distance(startPosition, endPosition);
            float startTime = Time.time;

            while (Vector3.Distance(capsule.transform.position, endPosition) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;
                capsule.transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
                yield return null;
            }

            capsule.transform.position = endPosition; // Asegúrate de que llegue a la posición final
        }
    }

    public class Node
    {
        public Node(string id)
        {
            this.Id = id;
        }

        public string Id;
        public Node Parent;
    }

    public class Edge
    {
        public Edge(Node a, Node b)
        {
            this.A = a;
            this.B = b;
        }

        public Node A;
        public Node B;
    }
}
