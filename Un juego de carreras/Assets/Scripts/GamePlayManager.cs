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
    GameObject replayBtn;
    [SerializeField]
    GameObject countDownTxt;
    [SerializeField]
    GameObject miniMap;
    [SerializeField]
    List<GameObject> carsAI;
    [SerializeField]
    GameObject playerCar;
    [SerializeField]
    GameObject ghostCar;

    int countDown = 4;
    CarAIControl carCtrl;
    List<CarAIControl> carsAICtrl = new List<CarAIControl>();
    CarUserControl playerCarCtrl;
    GhostControl ghostCarCtrl;
    Time startTime;

    private void Awake()
    {
        playerCar.GetComponent<CheckpointManager>().gameOver += GameOver;
        playerCarCtrl = playerCar.GetComponent<CarUserControl>();
        ghostCarCtrl = ghostCar.GetComponent<GhostControl>();
        foreach (var carAI in carsAI)
        {
            //carAI.GetComponent<CheckpointManager>().gameOver += GameOver;
            carsAICtrl.Add(carAI.GetComponent<CarAIControl>());
        }
    }

    public void StartRace(bool replay)
    {
        replayBtn.SetActive(false);
        StartCoroutine(StartCountDown(replay));
    }

    IEnumerator StartCountDown(bool replay)
    {
        for (int i = 0; i < 4; i++)
        {
            if (countDown == 1)
            {
                startText.text = "GO!";
                countDownTxt.SetActive(true);
                //START THE RACE
                AwakeDrivers(replay);
            }
            else
            {
                countDown--;
                startText.text = countDown.ToString();
            }
            yield return new WaitForSeconds(1);
        }
    }

    void AwakeDrivers(bool replay)
    {
        miniMap.SetActive(true);
        playerCar.GetComponent<CheckpointManager>().CountDownInit();
        playerCarCtrl.replayRace = replay;
        playerCarCtrl.drive = true;
        ghostCarCtrl.drive = true;
        ghostCarCtrl.replayRace = replay;
        foreach (var carAI in carsAICtrl)
        {
            carAI.replayRace = replay;
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
            if (!playerCarCtrl.replayRace) playerCarCtrl.SaveRace();
            playerCarCtrl.drive = false;
            foreach (var carAI in carsAICtrl)
            {
                if (!carAI.replayRace) carAI.SaveRace();
                carAI.m_Driving = false;
            }
            print("Game Over");
            Time.timeScale = 0;
        }
        else
        {
            var cAI = carsAICtrl.Find(c => c.name == car.name);
            if (cAI != null)
            {
                cAI.SaveRace();
            }

            Destroy(car);
        }
    }
}
