using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public Node(string id)
    {
        this.Id = id;
    }
    public string Id;
    

    //List<Edge> vecinos;

    //un nodo "Parent"
    //public Node Parent;
}

public class Edge
{

    public Edge(Node a, Node b)
    {
          this.A = a;
          this.B = b;
    }
    //Guardar los dos nodos que estan conectados (A y B)
    public Node A;
    public Node B;

    //public float Weight;

}
public class Graft : MonoBehaviour
{
    public List<Edge> edges = new List<Edge>();
    public List<Node> nodes = new List<Node>();
    void Start()
    {
        Node A = new Node(id:"A");
        Node B = new Node(id:"B");
        Node C = new Node(id:"C");
        Node D = new Node(id:"D");
        Node E = new Node(id:"E");
        Node F = new Node(id:"F");
        Node G = new Node(id:"G");
        Node H = new Node(id:"h");

        nodes.Add(A);
        nodes.Add(B);
        nodes.Add(C);
        nodes.Add(D);   
        nodes.Add(E);
        nodes.Add(F);
        nodes.Add(G);
        nodes.Add(H);

        Edge AB = new Edge(A, B);
        Edge BC = new Edge(a:B, b:C);
        Edge BD = new Edge(a: B, b: D);
        Edge AE = new Edge(A, b: E);
        Edge EF = new Edge(a:E, b: F);
        Edge EG = new Edge(a: E, b: G);
        Edge EH = new Edge(a: E, b:H);

        edges.Add(AB);
        edges.Add(BC);
        edges.Add(BD);
        edges.Add(AE);
        edges.Add(EF);
        edges.Add(EG);
        edges.Add(EH);

        Debug.Log(message: "Iniciando DFS");
        DepthFirstSearchRecursive(currentNode: A, goalNode: H);
        Debug.Log(message: "Terminé DFS");

    }

    public List<Node> FindNeighbords(Node node)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Edge edge in edges) 
        {
           if(edge.A == node)
            {
                neighbors.Add(edge.B);
            }
           if(edge.B == node) 
            { 
               neighbors.Add(edge.A); 
            }
        
        }
        return neighbors;
    }
    public bool DepthFirstSearchRecursive(Node currentNode, Node goalNode)
    {
        Debug.Log(message: "Nodo actualmente visitado: " + currentNode.Id);
        if (currentNode == goalNode)
        {
            return true;
        }
        List<Node> neighbors = FindNeighbords(currentNode);
        foreach(Node neighbor in neighbors) 
        {
          bool DFS_Result = DepthFirstSearchRecursive(currentNode: neighbor, goalNode);
            if (DFS_Result) 
            {
                return true;
            }
        }
        return false;
    }
  
    void Update()
    {
        
    }
}
