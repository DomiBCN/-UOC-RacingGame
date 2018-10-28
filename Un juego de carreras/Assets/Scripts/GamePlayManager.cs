using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject replayBtn;
    [SerializeField]
    Text initCountDownTxt;
    [SerializeField]
    GameObject countDownTxt;
    [SerializeField]
    Text gameOverTxt;
    [SerializeField]
    GameObject backToMenuBtn;
    [SerializeField]
    GameObject pauseMenu;
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
    List<CarAudio> audioSource = new List<CarAudio>();

    private void Awake()
    {
        if (!File.Exists(Application.dataPath + "/Saves/" + playerCar.name + ".txt")) replayBtn.SetActive(false);
        playerCar.GetComponent<CheckpointManager>().gameOver += GameOver;
        playerCarCtrl = playerCar.GetComponent<CarUserControl>();
        audioSource.Add(playerCar.GetComponent<CarAudio>());
        ghostCarCtrl = ghostCar.GetComponent<GhostControl>();
        foreach (var carAI in carsAI)
        {
            carAI.GetComponent<CheckpointManager>().gameOver += GameOver;
            carsAICtrl.Add(carAI.GetComponent<CarAIControl>());
            audioSource.Add(carAI.GetComponent<CarAudio>());
        }
    }

    void GetAudioSources(GameObject car)
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenu.activeInHierarchy)
        {
            Pause();
        }
    }

    public void StartRace(bool replay)
    {
        if(!replay)
        {
            ghostCar.SetActive(true);
        }
        mainMenu.SetActive(false);
        initCountDownTxt.gameObject.SetActive(true);
        StartCoroutine(StartCountDown(replay));
    }

    IEnumerator StartCountDown(bool replay)
    {
        for (int i = 0; i < 4; i++)
        {
            if (countDown == 1)
            {
                initCountDownTxt.text = "GO!";
                countDownTxt.SetActive(true);
                //START THE RACE
                AwakeDrivers(replay);
            }
            else
            {
                countDown--;
                initCountDownTxt.text = countDown.ToString();
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
        //we want to show the ghost car only if we are not in replay mode
        if (!replay)
        {
            ghostCarCtrl.drive = true;
            ghostCarCtrl.replayRace = replay;
        }
        foreach (var carAI in carsAICtrl)
        {
            carAI.gameObject.GetComponent<CheckpointManager>().CountDownInit();
            carAI.replayRace = replay;
            carAI.m_Driving = true;
        }
        StartCoroutine("HideCountDown");
    }

    IEnumerator HideCountDown()
    {
        while (initCountDownTxt.color.a > 0)
        {
            initCountDownTxt.color = new Color(initCountDownTxt.color.r, initCountDownTxt.color.g, initCountDownTxt.color.b, initCountDownTxt.color.a - (Time.deltaTime / 1f));
            yield return null;
        }
        initCountDownTxt.gameObject.SetActive(false);
    }

    void GameOver(GameObject car)
    {
        if (car.tag == "Player")
        {
            //if we are replaying last race, do not save
            if (!playerCarCtrl.replayRace) playerCarCtrl.SaveRace();
            playerCarCtrl.drive = false;
            //disable car sounds
            audioSource.ForEach(a => a.PauseSound(false));
            //if we are eliminated and there are still AI cars, save their data(if not replay)
            foreach (var carAI in carsAICtrl)
            {
                if (carAI != null)
                {
                    if (!carAI.replayRace) carAI.SaveRace();
                    carAI.m_Driving = false;
                }
            }
            backToMenuBtn.SetActive(true);
            gameOverTxt.gameObject.SetActive(true);
            StartCoroutine(FadeInGameOver());
        }
        else
        {
            //if the car that has run out of time is an AI, find it, save his race data(in case its not a replay) and destroy the car
            var cAI = carsAICtrl.Find(c => c != null && c.name == car.name);
            if (cAI != null)
            {
                if(!cAI.replayRace) cAI.SaveRace();
                Destroy(car);
            }

            
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Pause()
    {
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            audioSource.ForEach(a => a.PauseSound(true));
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            audioSource.ForEach(a => a.PauseSound(false));
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    IEnumerator FadeInGameOver()
    {
        while (gameOverTxt.color.a < 1)
        {
            gameOverTxt.color = new Color(gameOverTxt.color.r, gameOverTxt.color.g, gameOverTxt.color.b, gameOverTxt.color.a + (Time.deltaTime / 1f));
            yield return null;
        }
    }
}
