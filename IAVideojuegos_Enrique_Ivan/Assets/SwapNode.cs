using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapNode : MonoBehaviour
{
    public GraphBFS graphBFS; // Referencia al script GraphBFS
    public Button swapButton; // Referencia al bot�n en el Canvas

    private void Start()
    {
        if (graphBFS == null || swapButton == null)
        {
            Debug.LogError("GraphBFS o SwapButton no est�n asignados en el Inspector.");
            return;
        }

        swapButton.onClick.AddListener(SwapNodes);
    }

    private void SwapNodes()
    {
        // Verifica si las esferas est�n correctamente asignadas
        if (graphBFS.nodeA == null || graphBFS.nodeB == null || graphBFS.nodeC == null ||
            graphBFS.nodeD == null || graphBFS.nodeE == null || graphBFS.nodeF == null ||
            graphBFS.nodeG == null || graphBFS.nodeH == null)
        {
            Debug.LogError("Una o m�s esferas no est�n asignadas.");
            return;
        }

        // Lista de esferas
        List<GameObject> nodes = new List<GameObject>
        {
            graphBFS.nodeA,
            graphBFS.nodeB,
            graphBFS.nodeC,
            graphBFS.nodeD,
            graphBFS.nodeE,
            graphBFS.nodeF,
            graphBFS.nodeG,
            graphBFS.nodeH
        };

        // Selecciona dos esferas aleatorias de la lista
        GameObject node1 = nodes[Random.Range(0, nodes.Count)];
        GameObject node2 = nodes[Random.Range(0, nodes.Count)];

        // Aseg�rate de que los dos nodos seleccionados sean diferentes
        while (node1 == node2)
        {
            node2 = nodes[Random.Range(0, nodes.Count)];
        }

        // Intercambia las posiciones
        SwapPositions(node1, node2);
    }

    private void SwapPositions(GameObject obj1, GameObject obj2)
    {
        Vector3 tempPosition = obj1.transform.position;
        obj1.transform.position = obj2.transform.position;
        obj2.transform.position = tempPosition;
    }
}
