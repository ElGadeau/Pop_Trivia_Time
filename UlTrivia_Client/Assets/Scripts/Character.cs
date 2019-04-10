using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string m_name;

    public Client m_client;
    public bool m_isSelected;

    public void Selected()
    {
        m_isSelected = true;
        Net_CharacterSelection cs = new Net_CharacterSelection();

        cs.Name = m_name;
        cs.IsActive = m_isSelected;
        
        m_client.SendServer(cs);
    }
}