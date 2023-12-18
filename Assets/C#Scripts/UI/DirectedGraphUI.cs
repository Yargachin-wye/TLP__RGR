using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DirectedGraphUI : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup grop;
    [SerializeField] private GameObject Panel1;
    [SerializeField] private GameObject Panel2;
    [SerializeField] private TMP_InputField InputField1;
    [SerializeField] private TMP_InputField InputField2;
    [SerializeField] private TMP_InputField InputField3;
    [SerializeField] private TMP_InputField InputField4;
    [SerializeField] private TMP_InputField InputField5;
    [SerializeField] private TextMeshProUGUI TableText;
    [SerializeField] private TextMeshProUGUI CheckText;
    [SerializeField] private RectTransform nodePrefab; // Префаб узла графа
    [SerializeField] private RectTransform edgePrefab; // Префаб ребра графа
    [SerializeField] private RectTransform edgeInPrefab; // Префаб ребра графа
    [SerializeField] private RectTransform canvas; // Ссылка на объект Canvas
    Dictionary<string, RectTransform> nodes = new Dictionary<string, RectTransform>();
    InterpreterDKA interpreterDKA;
    private void Start()
    {
        Panel2.SetActive(false);
    }
    public void TryGenerateNewDKA()
    {
        TableText.text = "";
        for(int i = 0;i < canvas.childCount; i++)
        {
            Destroy(canvas.GetChild(i).gameObject);
        }
        nodes = new Dictionary<string, RectTransform>();
        List<char> alphabet = new List<char>();
        string finalSubchain;
        char countSymbol;
        int count;
        foreach (char c in InputField1.text)
        {
            alphabet.Add(c);
        }
        finalSubchain = InputField2.text;
        if (InputField3.text.Length <= 0)
        {
            return;
        }
        countSymbol = InputField3.text[0];
        if (!int.TryParse(InputField4.text, out count))
        {
            return;
        }
        interpreterDKA = new InterpreterDKA(alphabet, finalSubchain, countSymbol, count, CheckText);
        CreateNode("q0", Vector2.zero);
        foreach (var transition in interpreterDKA.rules)
        {
            string str = ($"{transition.Key}: ");
            Dictionary<string, string> edges = new Dictionary<string, string>();
            foreach (var pair in transition.Value)
            {
                str += ($"({pair.Item1}, {pair.Item2}) ");
                if (edges.ContainsKey(pair.Item2))
                {
                    edges[pair.Item2] += pair.Item1;
                }
                else
                {
                    edges.Add(pair.Item2, "");
                    edges[pair.Item2] += pair.Item1;
                }
            }
            TableText.text += str + "\n";
            int i = 0;
            foreach (var edge in edges)
            {
                CreateNodeFromAnother(transition.Key, edge.Key, i, edge.Value);
                i++;
            }
        }

        RefreshNodes();
    }
    public void Swap()
    {
        Panel1.SetActive(!Panel1.active);
        Panel2.SetActive(!Panel2.active);
    }
    public void CheckDKA()
    {
        CheckText.text = "";
        if (interpreterDKA == null)
        {
            return;
        }
        interpreterDKA.CheckDKA(InputField5.text);
        StartCoroutine(Grop());
    }
    private void CreateNode(string name, Vector2 v2)
    {
        RectTransform nodeA = Instantiate(nodePrefab, canvas.transform);
        nodeA.anchoredPosition = v2;
        nodeA.GetComponent<NodeUI>().Init(name);

        nodes.Add(name, nodeA);
    }
    private void CreateNodeFromAnother(string nameOld, string nameNew, int num, string str)
    {
        if (nameOld == nameNew)
        {
            CreateEdge(nodes[nameOld], nodes[nameNew], str);
            return;
        }
        RectTransform nodeA = Instantiate(nodePrefab, canvas.transform);
        Vector2 v2 = nodes[nameOld].anchoredPosition;

        nodeA.anchoredPosition = new Vector2(v2.x + 120 * num, v2.y - 150);

        nodeA.GetComponent<NodeUI>().Init(nameNew);
        string abstractAd = "";
        int i = 1;
        while (nodes.ContainsKey(nameNew + abstractAd))
        {
            abstractAd = i.ToString();
            i++;
        }
        nodes.Add(nameNew + abstractAd, nodeA);
        CreateEdge(nodes[nameOld], nodes[nameNew + abstractAd], str);

    }
    // Метод для создания ребра между узлами
    void CreateEdge(RectTransform startNode, RectTransform endNode, string name)
    {
        RectTransform newEdge;
        if (startNode == endNode)
        {
            newEdge = Instantiate(edgeInPrefab, startNode);
            newEdge.GetComponent<EdgeUI>().Init(name);
        }
        else
        {
            Vector3 startPosition = startNode.position;
            Vector3 endPosition = endNode.position;

            newEdge = Instantiate(edgePrefab, canvas.transform);

            float distance = Vector3.Distance(startPosition, endPosition);
            newEdge.sizeDelta = new Vector2(distance, 6f);
            float angle = Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg;
            newEdge.rotation = Quaternion.Euler(0f, 0f, angle);

            newEdge.position = startPosition + (endPosition - startPosition) / 2f;
        }

        newEdge.GetComponent<EdgeUI>().Init(name);
    }

    private void RefreshNodes()
    {
        foreach (var node in nodes)
        {
            node.Value.parent = transform;
            node.Value.parent = canvas.transform;
        }
    }
    IEnumerator Grop()
    {
        yield return new WaitForSeconds(0.2f);
        grop.SetLayoutVertical();
    }
}
