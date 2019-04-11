using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoteButton : MonoBehaviour
{
    public int m_vote;

    public void SendVote()
    {
        Net_SendVote sv = new Net_SendVote();

        sv.Vote = m_vote;
        Client.m_instance.SendServer(sv);
        StartCoroutine(WaitChangeCanvas());
    }

    public void SetVoteValue(int p_value)
    {
        m_vote = p_value;
    }
    
    IEnumerator WaitChangeCanvas()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}