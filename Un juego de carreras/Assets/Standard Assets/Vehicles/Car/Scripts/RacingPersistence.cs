using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class RacingPersistence
{
    static string savePath = Application.dataPath + "/Saves/";

    public static void SaveRaceData(float time, List<RacingData.CarStatus> carTransforms, string fileName)
    {
        CheckIfDirectoryExists();
        RacingData.CarStatusContainer cont = new RacingData.CarStatusContainer();
        cont.Time = time;
        cont.carMovement = carTransforms;
        string jsonRace = JsonUtility.ToJson(cont);
        File.WriteAllText(savePath + fileName + ".txt", jsonRace);
    }

    public static RacingData.CarStatusContainer LoadRaceData(string fileName)
    {
        RacingData.CarStatusContainer cont = new RacingData.CarStatusContainer();
        if (File.Exists(savePath + fileName + ".txt"))
        {
            string loadedData = File.ReadAllText(savePath + fileName + ".txt");
            cont = JsonUtility.FromJson<RacingData.CarStatusContainer>(loadedData);
        }
        return cont;
    }

    static void CheckIfDirectoryExists()
    {
        //Check if directory Saves exists, otherwise create it
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    public static bool CheckIfFileExists(string fileName)
    {
        //Check if directory Saves exists, otherwise create it
        if (!File.Exists(savePath + fileName + ".txt"))
        {
            return false;
        }
        else return true;
    }
}
