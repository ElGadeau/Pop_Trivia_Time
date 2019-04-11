using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public TextMeshProUGUI m_textMesh;
    public Client m_client;
    
    public void SendAnswer()
    {
        Net_SendText st = new Net_SendText();

        st.Text = m_textMesh.text;
        m_client.SendServer(st);
    }
}