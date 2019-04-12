using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public TextMeshProUGUI m_textMesh;
    [SerializeField] private Canvas m_activeCanvas;
    [SerializeField] private Canvas m_nextCanvas;


    public void SendAnswer()
    {
        if (Client.m_instance != null)
        {
            Net_SendText st = new Net_SendText();
            st.Text = m_textMesh.text;
            Client.m_instance.SendServer(st);
        }

        StartCoroutine(WaitChangeCanvas());
    }

    IEnumerator WaitChangeCanvas()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}