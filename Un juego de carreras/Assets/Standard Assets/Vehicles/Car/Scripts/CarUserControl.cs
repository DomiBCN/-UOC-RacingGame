using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        [HideInInspector]
        public bool drive;

        private CarController m_Car; // the car controller we want to use

        bool recording = false;
        bool replaying = false;
        public bool replayRace = false;
        RacingData.CarStatusContainer raceData = new RacingData.CarStatusContainer();
        RacingData.CarStatusContainer bestRaceData = new RacingData.CarStatusContainer();

        float time;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            raceData = RacingPersistence.LoadRaceData(gameObject.name);
            bestRaceData = RacingPersistence.LoadRaceData("bestRace");
        }

        private void FixedUpdate()
        {
            if (drive)
            {
                if (!replayRace)
                {
                    time += Time.deltaTime;
                    float h = CrossPlatformInputManager.GetAxis("Horizontal");
                    float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
                    float handbrake = CrossPlatformInputManager.GetAxis("Jump");
                   
                    //if we are not in replay mode, start recording player data
                    if (!recording)
                    {
                        recording = true;
                        m_Car.StartRecording();
                    }
                    m_Car.Move(h, v, v, handbrake);

#else
            m_Car.Move(h, v, v, 0f);
#endif
                }
                else
                {
                    //if we haven't started the replay yet, start it
                    if (!replaying)
                    {
                        replaying = true;
                        m_Car.StartReplay(raceData);
                    }
                }
            }
        }
        
        public void SaveRace()
        {
            m_Car.SaveRace(time, gameObject.name);
            //check if this race has been the best one, if positive, update the preovious one with this race
            if (RacingPersistence.CheckIfFileExists("bestRace"))
            {
                if (bestRaceData.Time < time)
                {
                    m_Car.SaveRace(time, "bestRace");
                }
            }
            else
            {
                m_Car.SaveRace(time, "bestRace");
            }
            
        }
    }
}
