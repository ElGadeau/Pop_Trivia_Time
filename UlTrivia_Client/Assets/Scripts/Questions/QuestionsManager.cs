using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour
{
    public  string           m_questionDataBasePath;
    private QuestionDataBase m_qDb = new QuestionDataBase();

    private int m_currentQuestionIndex;

    public  TextMeshProUGUI m_currentAnnouncement;
    public  TextMeshProUGUI m_questionText;
    public  countDown       m_countDownRef;
    private float           m_countdownValue;
    public  Image           m_image;
    public  List<Sprite>    m_illustrations;
    public  List<AudioClip> m_questionAudioClips;
    public  List<AudioClip> m_factAudioClips;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        LoadQuestions();
        ChooseRandomQuestion();
        m_countdownValue = 30;
        //m_image.sprite = m_illustrations.Find(sprite => sprite.name == "eddy");
    }

    private void LoadQuestions()
    {
        m_qDb = QuestionDataBase.Load(Path.Combine(Application.dataPath, m_questionDataBasePath));
        m_qDb.Save(Path.Combine(Application.dataPath, "questions.xml"));
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
        PlayQuestionClip();
        LoadIllustrations();
        m_countDownRef.StartCoroutine(m_countDownRef.StartCountdown());
    }

    private void PlayQuestionClip()
    {
        m_audioSource.clip = m_questionAudioClips.Find(audioClips => audioClips.name == m_qDb
                                                                             .m_questions[m_currentQuestionIndex]
                                                                             .m_questionSoundClipName);
        if (m_audioSource.clip != null)
            m_audioSource.Play();
    }

    private void LoadIllustrations()
    {
        m_image.sprite = m_illustrations.Find(sprite => sprite.name == m_qDb.m_questions[m_currentQuestionIndex]
                                                                .m_illustrations[0]);
        if (m_image.sprite == null)
            m_image.sprite = m_illustrations.Find(sprite => sprite.name == "eddy");
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
        PlayFactClip();
        yield return new WaitForSeconds(5);
        m_qDb.m_questions.RemoveAt(m_currentQuestionIndex);
        ChooseRandomQuestion();
    }
    
    private void PlayFactClip()
    {
        m_audioSource.clip = m_factAudioClips.Find(audioClips => audioClips.name == m_qDb
                                                                             .m_questions[m_currentQuestionIndex]
                                                                             .m_factSoundClipName);
        if (m_audioSource.clip != null)
            m_audioSource.Play();
    }
}