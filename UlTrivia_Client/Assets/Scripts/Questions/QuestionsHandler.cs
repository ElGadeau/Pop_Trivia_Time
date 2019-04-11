using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class QuestionsHandler : MonoBehaviour
{
    public  string           m_questionDataBasePath;
    private QuestionDataBase m_qDb = new QuestionDataBase();

    private int m_currentQuestionIndex;

    public  TextMeshProUGUI m_currentAnnouncement;
    public  TextMeshProUGUI m_questionText;
    public  countDown       m_countDownRef;
    private float           m_countdownValue;

    private void Start()
    {
        LoadQuestions();
        ChooseRandomQuestion();
        m_countdownValue = 30;
    }

    private void LoadQuestions()
    {
        m_qDb = QuestionDataBase.Load(Path.Combine(Application.dataPath, m_questionDataBasePath));
    }

    private void ChooseRandomQuestion()
    {
        if (m_qDb.m_questions.Count <= 0)
        {
            Question ending = new Question();
            ending.m_question = "Congratulations ! you finished the quizz ! Results will be displayed soon";
            DisplayQuestion(ending);
            return;
        }

        m_currentQuestionIndex = Random.Range(0, m_qDb.m_questions.Count - 1);
        DisplayQuestion(m_qDb.m_questions[m_currentQuestionIndex]);
    }

    private void DisplayQuestion(Question p_question)
    {
        m_currentAnnouncement.text = "Question:";
        m_questionText.text        = p_question.m_question;
        m_countdownValue           = 30;
        StartCoroutine(m_countDownRef.StartCountdown());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ChooseRandomQuestion();

        if (m_countdownValue != -1)
            m_countdownValue = m_countDownRef.currCountdownValue;

        if (m_countdownValue == 0)
        {
            m_countdownValue = -1;
            StartCoroutine(ShowAnswer());
        }

        Debug.Log(m_currentQuestionIndex);
    }

    IEnumerator ShowVote()
    {
        yield return new WaitForSeconds(1);
    }
    
    IEnumerator ShowAnswer()
    {
        m_currentAnnouncement.text = "Fun Fact:";
        m_questionText.text        = m_qDb.m_questions[m_currentQuestionIndex].m_fact;
        yield return new WaitForSeconds(5);
        m_qDb.m_questions.RemoveAt(m_currentQuestionIndex);
        ChooseRandomQuestion();
    }
}