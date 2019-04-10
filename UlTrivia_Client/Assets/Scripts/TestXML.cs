using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestXML : MonoBehaviour
{
    // Use this for initialization


    private void Start()
    {
        QuestionDataBase qb = new QuestionDataBase();
        var testQuestion = new Question
        {
            m_question = "what is love ?",
            m_fact     = "baby don't hurt me.",
            m_answers  = new List<string> {"don't hurt me", "don't hurt me", "no more"}
        };

        var q1 = new Question
        {
            m_question = "Qui a prédit l'existence des Protons?",
            m_fact     = "Mr. Prout en 1815",
            m_answers = new List<string>
                    {"Albert Einstein", "Mr Fladislas IVth, Duc de Durambour, Esqu.", "MR PROUT!!!!!!!"}
        };

        qb.m_questions.Add(testQuestion);
        qb.m_questions.Add(q1);

        qb.Save(Path.Combine(Application.dataPath, "questions.xml"));
    }

    // Update is called once per frame
    void Update()
    {
    }
}