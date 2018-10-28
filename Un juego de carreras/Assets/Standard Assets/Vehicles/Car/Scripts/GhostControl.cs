using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[RequireComponent(typeof(CarController))]
public class GhostControl : MonoBehaviour
{

    [HideInInspector]
    public bool drive;

    private CarController m_Car; // the car controller we want to use
    
    bool replaying = false;
    public bool replayRace = false;
    RacingData.CarStatusContainer raceData = new RacingData.CarStatusContainer();

    private void Awake()
    {
        // get the car controller
        m_Car = GetComponent<CarController>();
        raceData = RacingPersistence.LoadRaceData("bestRace");
        if (raceData.carMovement == null) gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (drive)
        {
            if (!replaying && !replayRace && raceData.carMovement != null && raceData.carMovement.Count > 0)
            {
                replaying = true;
                m_Car.StartReplay(raceData);
            }
        }
    }
}
