using System.Collections;
using UnityEngine;
using TMPro;

public class countDown : MonoBehaviour
{
    TextMeshProUGUI text;
    public float    countdownValue = 30;
    public float    currCountdownValue { get; private set; }

    public void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator StartCountdown()
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue >= 0)
        {
            text.text = Mathf.Round(currCountdownValue).ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }
}