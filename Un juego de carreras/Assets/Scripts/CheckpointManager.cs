using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{

    public List<GameObject> checkpoints;
    public int nextCheckpoint = 0;
    public int currentLap = 1;
    int countDown = 5;
    public Text countDownText;

    public delegate void GameOver(GameObject car);
    public event GameOver gameOver;

    public void UpdateSeconds(int timeBonus)
    {
        countDown += timeBonus;
        countDownText.text = countDown + "s";
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
            countDownText.text = countDown + "s";
        }
        else
        {
            gameOver(gameObject);
        }
    }
}
