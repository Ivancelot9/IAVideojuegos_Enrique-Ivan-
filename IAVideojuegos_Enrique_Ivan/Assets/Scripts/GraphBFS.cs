using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<string, GameObject> nodeObjects;
    private Dictionary<string, Node> nodes;
    private List<Edge> edges;

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

        Debug.Log("Iniciando BFS");
        Node startNode = nodes["H"];
        Node goalNode = nodes["D"];
        StartCoroutine(BreadthFirstSearch(startNode, goalNode));
    }

    private IEnumerator BreadthFirstSearch(Node startNode, Node goalNode)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);
        ChangeNodeColor(startNode, Color.red);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if (currentNode == goalNode)
            {
                Debug.Log("Camino encontrado desde " + startNode.Id + " hasta " + goalNode.Id + " usando BFS:");
                PrintPath(goalNode);
                ChangeNodeColor(goalNode, Color.green);
                yield break;
            }

            List<Node> neighbors = FindNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    neighbor.Parent = currentNode;
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    ChangeNodeColor(neighbor, Color.red);
                    yield return new WaitForSeconds(1f); // Espera 1 segundo entre visitas
                }
            }

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

    private void PrintPath(Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        foreach (Node node in path)
        {
            Debug.Log(node.Id);
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
