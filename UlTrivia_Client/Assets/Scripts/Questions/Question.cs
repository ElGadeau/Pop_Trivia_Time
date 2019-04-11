using System.Collections.Generic;
using System.Xml.Serialization;

public class Question
{
    [XmlAttribute("Question")]          public string m_question              = "none";
    [XmlElement("Fun_Fact")]            public string m_fact                  = "none";
    [XmlElement("Question_Sound_Clip")] public string m_questionSoundClipName = "none";
    [XmlElement("Fact_Sound_Clip")]     public string m_factSoundClipName     = "none";

    [XmlArray("Answers")] [XmlArrayItem("Answer")]
    public List<string> m_answers;

    [XmlArray("Illustrations")] [XmlArrayItem("llustration")]
    public List<string> m_illustrations = new List<string> {"none", "none"};
}