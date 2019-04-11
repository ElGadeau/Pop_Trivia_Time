using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class AnswersManager : MonoBehaviour
{
    public List<TextMeshProUGUI> m_answerTextList;

    private void Start()
    {
        for (int i = 0; i < m_answerTextList.Count; i++)
        {
            m_answerTextList[i].text = Server.m_answerList[i].answersText;
        }
    }
}