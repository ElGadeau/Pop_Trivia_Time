using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Net_CharacterSelection : NetMsg
{
    public Net_CharacterSelection()
    {
        OP = NetOP.SelectChara;
    }
    
    public string Name { set; get; }
    public bool IsActive { set; get; }
}