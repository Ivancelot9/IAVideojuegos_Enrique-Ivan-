using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

// Clase para representar un nodo en el grafo
public class Node
{
    public Node(string id)
    {
        this.Id = id; // Identificador único del nodo
    }

    public string Id; // Identificador del nodo
    public Node Parent; // Nodo padre en el camino encontrado
}


public class Edge
{
    public Edge(Node a, Node b)
    {
        this.A = a; // Nodo A de la arista
        this.B = b; // Nodo B de la arista
    }

    public Node A; // Nodo A de la arista
    public Node B; // Nodo B de la arista
}


public class Graft : MonoBehaviour
{
    //Lista de Nodos y Aristas
    public List<Edge> edges = new List<Edge>();
    public List<Node> nodes = new List<Node>();

    void Start()
    {
       

        Node A = new Node("A");
        Node B = new Node("B");
        Node C = new Node("C");
        Node D = new Node("D");
        Node E = new Node("E");
        Node F = new Node("F");
        Node G = new Node("G");
        Node H = new Node("H");

        nodes.Add(A);
        nodes.Add(B);
        nodes.Add(C);
        nodes.Add(D);
        nodes.Add(E);
        nodes.Add(F);
        nodes.Add(G);
        nodes.Add(H);

        Edge AB = new Edge(A, B);
        Edge BC = new Edge(B, C);
        Edge BD = new Edge(B, D);
        Edge AE = new Edge(A, E);
        Edge EF = new Edge(E, F);
        Edge EG = new Edge(E, G);
        Edge EH = new Edge(E, H);

        edges.Add(AB);
        edges.Add(BC);
        edges.Add(BD);
        edges.Add(AE);
        edges.Add(EF);
        edges.Add(EG);
        edges.Add(EH);

        Debug.Log("Iniciando DFS");
        DepthFirstSearch(H, D);
        Debug.Log("Terminó DFS");

        //Tarea
        Debug.Log("Iniciando BFS");

        Node origin = H;
        Node target = D;
  
        // Ejecutar BFS e imprimir el camino si existe
        bool pathFoundBFS = BreadthFirstSearch(origin, target);

        if (pathFoundBFS)
        {
            Debug.Log("Camino encontrado desde " + origin.Id + " hasta " + target.Id + " usando BFS:");
            PrintPath(target);
        }
        else
        {
            Debug.Log("No se encontró camino desde " + origin.Id + " hasta " + target.Id + " usando BFS");
        }

        Debug.Log("Terminó BFS");
    }

    public List<Node> FindNeighbors(Node node)
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


    public bool DepthFirstSearch(Node startNode, Node goalNode)
    {
        startNode.Parent = null;

        Stack<Node> openList = new Stack<Node>();
        openList.Push(startNode);
        List<Node> closedList = new List<Node>();

        Node currentNode = null;

        while (openList.Count > 0)
        {
            currentNode = openList.Pop();
            closedList.Add(currentNode);
            if (currentNode == goalNode)
            {
                while (currentNode != null)
                {
                    Debug.Log("El nodo: " + currentNode.Id + " fue parte del camino a la meta.");
                    currentNode = currentNode.Parent; 
                }

                return true;
            }

            List<Node> neighbors = FindNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            { 
                if (openList.Contains(neighbor) || closedList.Contains(neighbor))
                    continue;

                neighbor.Parent = currentNode;
                openList.Push(neighbor);
            }
        }
        return false;
    }

    public bool DepthFirstSearchRecursive(Node currentNode, Node goalNode)
    {
        Debug.Log("Nodo actualmente visitado: " + currentNode.Id);
        if (currentNode == goalNode)
        {
            return true;
        }
        List<Node> neighbors = FindNeighbors(currentNode);
        bool DFS_Result = false;
        foreach (Node neighbor in neighbors)
        {
            if (neighbor.Parent == null) 
            {
                neighbor.Parent = currentNode;
                DFS_Result = DepthFirstSearchRecursive(neighbor, goalNode);
            }


            if (DFS_Result)
            {
                Debug.Log("El nodo: " + currentNode.Id + " fue parte del camino a la meta.");
                return true;
            }
        }
        return false;
    }
    //Tarea BFS
    public bool BreadthFirstSearch(Node startNode, Node goalNode)
    {
        Queue<Node> queue = new Queue<Node>(); // Cola para almacenar nodos a visitar
        HashSet<Node> visited = new HashSet<Node>(); // Conjunto para almacenar nodos visitados

        queue.Enqueue(startNode); // Agregar nodo inicial a la cola
        visited.Add(startNode); // Marcar nodo inicial como visitado

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue(); // Obtener y remover nodo actual de la cola

            if (currentNode == goalNode)
            {
                return true; // Si se encuentra el nodo objetivo, retornar true
            }

            List<Node> neighbors = FindNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    neighbor.Parent = currentNode; // Establecer el nodo actual como padre del vecino
                    queue.Enqueue(neighbor); // Agregar vecino a la cola
                    visited.Add(neighbor); // Marcar vecino como visitado
                }
            }
        }

        return false; // Retornar false si no se encontró camino
    }


    // Método para imprimir el camino desde el origen hasta el nodo actual
    public void PrintPath(Node currentNode)
    {
        List<Node> path = new List<Node>();

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse(); // Revertir la lista para imprimir en orden correcto

        foreach (Node node in path)
        {
            Debug.Log(node.Id);
        }
    }


}

