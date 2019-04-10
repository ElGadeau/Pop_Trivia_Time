using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("QuestionDataBase")]
public class QuestionDataBase
{
    [XmlArray("Questions"), XmlArrayItem("Question")]
    public List<Question> m_questions = new List<Question>();

    public void Save(string p_path)
    {
        var serializer = new XmlSerializer(typeof(QuestionDataBase));
        using (var stream = new FileStream(p_path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static QuestionDataBase Load(string p_path)
    {
        var serializer = new XmlSerializer(typeof(QuestionDataBase));
        using (var stream = new FileStream(p_path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as QuestionDataBase;
        }
    }
}