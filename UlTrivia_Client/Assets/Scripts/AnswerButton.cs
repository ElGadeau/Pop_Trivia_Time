using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public TextMeshProUGUI m_textMesh;
    [SerializeField] private Canvas m_activeCanvas;
    [SerializeField] private Canvas m_nextCanvas;
    
    
    public void SendAnswer()
    {
        Net_SendText st = new Net_SendText();

        st.Text = m_textMesh.text;
        Client.m_instance.SendServer(st);
        StartCoroutine(WaitChangeCanvas());
    }

    IEnumerator WaitChangeCanvas()
    {
        yield return new WaitForSeconds(3.0f);
        m_activeCanvas.gameObject.SetActive(false);
        m_nextCanvas.gameObject.SetActive(true);
    }
}