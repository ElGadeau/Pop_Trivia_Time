using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class AnswersManager : MonoBehaviour
{
    public bool m_isVoting;
    public List<TextMeshProUGUI> m_answerTextList;
    
    public static List<string> m_answerList;
    public static List<int> m_voteList;
    
    public QuestionsManager m_qstMng;
    public bool m_isSet;
    
    private string m_defaultVote = "player did not join the game";

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        m_answerList = new List<string>()
        {
            m_defaultVote, m_defaultVote, m_defaultVote, m_defaultVote, m_defaultVote
        };
        for (int i = 0; i < m_answerTextList.Count; i++)
        {
            m_answerTextList[i].text = m_answerList[i];
        }
    }

    private void Update()
    {
        if (!m_isVoting)
        {
            Init();
            return;
        }

        if (m_isSet)
            return;

        m_answerTextList[Random.Range(0,m_answerList.Count - 1)].text =
            m_qstMng.m_qDb.m_questions[m_qstMng.m_currentQuestionIndex].m_answers[Random.Range(0, 2)];
        
        AssignText();

        m_isSet = true;
    }

    private void AssignText()
    {
        for (int i = 0; i < m_answerTextList.Count - 1; i++)
        {
            int index = Random.Range(0, m_answerList.Count);
            if (m_answerTextList[i].text == m_defaultVote)
            {
                m_answerTextList[i].text = m_answerList[index];
                m_answerList.RemoveAt(index);
            }
        }
    }
}