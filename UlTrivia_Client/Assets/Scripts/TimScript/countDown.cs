using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class countDown : MonoBehaviour {

    TextMeshProUGUI text; 

    public void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartCountdown());
    }

    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue = 30)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue >= 0)
        {
            text.text = Mathf.Round(currCountdownValue).ToString();
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }
}
