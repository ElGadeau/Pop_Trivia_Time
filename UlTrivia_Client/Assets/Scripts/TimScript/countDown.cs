using System.Collections;
using UnityEngine;
using TMPro;

public class countDown : MonoBehaviour
{
    TextMeshProUGUI m_text;
    public float    countdownValue = 30;
    public float    currCountdownValue { get; private set; }

    public void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.text = countdownValue.ToString();
    }

    public IEnumerator StartCountdown()
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue >= 0)
        {
            m_text.text = Mathf.Round(currCountdownValue).ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }
}