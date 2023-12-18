using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EdgeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    public void Init(string str)
    {
        m_TextMeshProUGUI.text = str;
        m_TextMeshProUGUI.rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
