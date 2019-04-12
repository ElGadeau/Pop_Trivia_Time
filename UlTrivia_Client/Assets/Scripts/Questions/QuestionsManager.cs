using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour
{
    public  string           m_questionDataBasePath;
    public QuestionDataBase m_qDb = new QuestionDataBase();

    public int m_currentQuestionIndex;

    public  TextMeshProUGUI m_questionText;
    public  TextMeshProUGUI m_factQuestion;
    public  TextMeshProUGUI m_factText;
    public  countDown       m_countDownRef;
    private float           m_countdownValue;
    public  GameObject      m_QuestionScreen;
    public  GameObject      m_voteScreen;
    public  GameObject      m_factScreen;
    public  GameObject      m_illustationLeft;
    public  GameObject      m_illustationRight;
    public  List<Sprite>    m_illustrations;
    public  List<AudioClip> m_questionAudioClips;
    public  List<AudioClip> m_factAudioClips;

    public AnswersManager m_ansMng;
    
    private AudioSource m_audioSource;

    enum QuestionStates
    {
        QUESTION,
        VOTE,
        FACT
    };

    private QuestionStates m_state;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        LoadQuestions();
        ChooseRandomQuestion();
        m_countdownValue = 30;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ChooseRandomQuestion();

        UpdateState();
    }

    private void UpdateState()
    {
        if (m_countdownValue != -1)
            m_countdownValue = m_countDownRef.currCountdownValue;

        if (m_countdownValue == 0)
        {
            switch (m_state)
            {
                case QuestionStates.QUESTION:
                    ShowVote();
                    m_QuestionScreen.GetComponent<Animator>().enabled = true;
                    break;
                case QuestionStates.VOTE:
                    StartCoroutine(ShowFact());
                    m_voteScreen.SetActive(false);
                    break;
                case QuestionStates.FACT:
                    ChooseRandomQuestion();
                    StartCoroutine(ResetIntroAnim());
                    m_factScreen.SetActive(false);
                    break;
            }
        }
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
        m_questionText.text           = p_question.m_question;
        m_countDownRef.countdownValue = 10;
        m_state                       = QuestionStates.QUESTION;
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
        Debug.Log(m_qDb.m_questions[m_currentQuestionIndex].m_illustrations[0]);
        m_illustationLeft.GetComponent<Image>().sprite = m_illustrations.Find(sprite =>
                sprite.name == m_qDb.m_questions[m_currentQuestionIndex].m_illustrations[0]);
        m_illustationRight.GetComponent<Image>().sprite = m_illustrations.Find(sprite =>
                sprite.name == m_qDb.m_questions[m_currentQuestionIndex].m_illustrations[1]);

        if (m_illustationLeft.GetComponent<Image>().sprite == null)
            m_illustationLeft.GetComponent<Image>().sprite = m_illustrations.Find(sprite => sprite.name == "eddy");
        if (m_illustationRight.GetComponent<Image>().sprite == null)
            m_illustationRight.GetComponent<Image>().sprite = m_illustrations.Find(sprite => sprite.name == "eddy");
    }


    void ShowVote()
    {
        m_ansMng.m_isVoting = true;
        m_state = QuestionStates.VOTE;
        m_voteScreen.SetActive(true);
        m_countdownValue              = 10;
        m_countDownRef.countdownValue = 10;
        m_countDownRef.StartCoroutine(m_countDownRef.StartCountdown());
    }

    IEnumerator ShowFact()
    {
        m_ansMng.m_isVoting = false;
        m_ansMng.Init();
        m_state             = QuestionStates.FACT;
        m_factQuestion.text = m_questionText.text;
        m_factText.text     = m_qDb.m_questions[m_currentQuestionIndex].m_fact;

        m_countDownRef.countdownValue = 5;
        m_countDownRef.StartCoroutine(m_countDownRef.StartCountdown());

        m_factScreen.SetActive(true);

        PlayFactClip();
        yield return new WaitForSeconds(5);
        m_qDb.m_questions.RemoveAt(m_currentQuestionIndex);
    }

    IEnumerator ResetIntroAnim()
    {
        m_QuestionScreen.GetComponent<Animator>().Play("Display", -1, 0);
        yield return new WaitForSeconds(0.01f);
        m_QuestionScreen.GetComponent<Animator>().enabled = false;
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