using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Net_SendText : NetMsg
{
    public Net_SendText()
    {
        OP = NetOP.SendText;
    }
    
    public string Text { set; get; }
}