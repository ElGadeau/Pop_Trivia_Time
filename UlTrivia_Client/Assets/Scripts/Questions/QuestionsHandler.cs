using System.IO;
using TMPro;
using UnityEngine;

public class QuestionsHandler : MonoBehaviour
{
    public string m_questionDataBasePath;
    private QuestionDataBase m_qDb= new QuestionDataBase();

    public TextMeshProUGUI m_questionText;
    
    private void Start()
    {
        LoadQuestions();
        ChooseRandomQuestion();
    }

    private void LoadQuestions()
    {
        m_qDb = QuestionDataBase.Load(Path.Combine(Application.dataPath, m_questionDataBasePath));
    }

    private void ChooseRandomQuestion()
    {
        if (m_qDb.m_questions.Count > 0)
            DisplayQuestion(m_qDb.m_questions[Random.Range(0,m_qDb.m_questions.Count - 1)]);
    }

    private void DisplayQuestion(Question p_question)
    {
        m_questionText.text = p_question.m_question;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ChooseRandomQuestion();
    }
}