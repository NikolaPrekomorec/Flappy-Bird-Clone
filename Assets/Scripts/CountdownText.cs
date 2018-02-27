using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour {

    public delegate void CountdownFinnished();
    public static event CountdownFinnished OnCountdownFinnished;
    

    Text countdown;

    void OnEnable ()
    {
        countdown = GetComponent<Text>();
        countdown.text = "3";
        StartCoroutine(StartCountdown());
    }

    //IEnumerator Countdown()
    //{
    //    int count = 3;
    //    for (int i = 0; i < count; i++)
    //    {
    //        countdown.text = (count - 1).ToString();
    //        yield return new WaitForSeconds(1);         
    //    }
    //    OnCountdownFinnished();
    //}



    float currCountdownValue;
    public IEnumerator StartCountdown()
    {
        currCountdownValue = 3;
        while (currCountdownValue > 0)
        {
            countdown.text = currCountdownValue.ToString();
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        OnCountdownFinnished();
    }

}
