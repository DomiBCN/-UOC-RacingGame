using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField]
    Text startText;
    [SerializeField]
    List<GameObject> carsAI;
    [SerializeField]
    GameObject playerCar;

    int countDown = 4;
    CarAIControl carCtrl;
    List<CarAIControl> carsAICtrl = new List<CarAIControl>();
    CarUserControl playerCarCtrl;

    private void Awake()
    {
        playerCar.GetComponent<CheckpointManager>().gameOver += GameOver;
        playerCarCtrl = playerCar.GetComponent<CarUserControl>();
        foreach (var carAI in carsAI)
        {
            //carAI.GetComponent<CheckpointManager>().gameOver += GameOver;
            carsAICtrl.Add(carAI.GetComponent<CarAIControl>());
        }
    }

    public void StartRace()
    {
        InvokeRepeating("StartCountDown", 0, 1);
    }

    void StartCountDown()
    {
        if (countDown == 1)
        {
            CancelInvoke();
            startText.text = "GO!";
            //START THE RACE
            AwakeDrivers();
        }
        else
        {
            countDown--;
            startText.text = countDown.ToString();
        }
    }

    void AwakeDrivers()
    {
        playerCar.GetComponent<CheckpointManager>().CountDownInit();
        playerCarCtrl.drive = true;
        foreach (var carAI in carsAICtrl)
        {
            carAI.m_Driving = true;
        }
        StartCoroutine("HideCountDown");
    }

    IEnumerator HideCountDown()
    {
        yield return new WaitForSeconds(2f);
        startText.gameObject.SetActive(false);
    }

    void GameOver(GameObject car)
    {
        if (car.tag == "Player")
        {
            print("Game Over");
            Destroy(car);
        }
    }
}
