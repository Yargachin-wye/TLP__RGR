using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    public void Init(string str)
    {
        m_TextMeshProUGUI.text = str;
    }
}
