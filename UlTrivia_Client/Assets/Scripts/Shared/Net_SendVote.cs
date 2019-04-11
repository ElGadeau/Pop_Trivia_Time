using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Net_SendVote : NetMsg
{
    public Net_SendVote()
    {
        OP = NetOP.SendVote;
    }
    
    public int Vote { set; get; }
}