using System.Collections.Generic;
using System.Xml.Serialization;

public class Question
{
    [XmlAttribute("Question")] public string m_question;
    [XmlElement("Fun_Fact")]   public string m_fact;

    [XmlArray("Answers")] [XmlArrayItem("Answer")]
    public List<string> m_answers;
}