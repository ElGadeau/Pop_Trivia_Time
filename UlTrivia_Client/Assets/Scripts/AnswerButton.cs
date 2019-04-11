using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public TextMeshProUGUI m_textMesh;
    
    public void SendAnswer()
    {
        Net_SendText st = new Net_SendText();

        st.Text = m_textMesh.text;
        Client.m_instance.SendServer(st);
    }
}