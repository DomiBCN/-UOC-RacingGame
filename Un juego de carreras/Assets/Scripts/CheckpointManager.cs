using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{

    public List<GameObject> checkpoints;
    public int nextCheckpoint = 0;
    public int currentLap = 1;
    int countDown = 45;
    public Text countDownText;
    public Text addedTime;

    public delegate void GameOver(GameObject car);
    public event GameOver gameOver;

    public void UpdateSeconds(int timeBonus)
    {
        countDown += timeBonus;
        if (gameObject.tag == "Player")
        {
            countDownText.text = countDown + "s";
            StartCoroutine(FadeInOutPlusTime());
        }
    }

    public void CountDownInit()
    {
        InvokeRepeating("CountDown", 0, 1);
    }

    void CountDown()
    {
        if (countDown > 0)
        {
            countDown--;
            if (gameObject.tag == "Player")
            {
                //updates the countdown on top left of the screen
                countDownText.text = countDown + "s";
            }
        }
        else
        {
            //if the car is out of time, trigger the game over
            CancelInvoke("CountDown");
            gameOver(gameObject);
        }
    }

    //fade in && fade out effect of the time added when crossing a checkpoint
    IEnumerator FadeInOutPlusTime()
    {
        addedTime.gameObject.SetActive(true);
        while (addedTime.color.a < 1)
        {
            addedTime.color = new Color(addedTime.color.r, addedTime.color.g, addedTime.color.b, addedTime.color.a + (Time.deltaTime / 1f));
            yield return null;
        }
        while (addedTime.color.a > 0)
        {
            addedTime.color = new Color(addedTime.color.r, addedTime.color.g, addedTime.color.b, addedTime.color.a - (Time.deltaTime / 1f));
            yield return null;
        }
        addedTime.gameObject.SetActive(false);
    }
    
}
