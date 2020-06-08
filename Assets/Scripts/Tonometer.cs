using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tonometer : MonoBehaviour
{
    public TonometerData tonometerData;
    public List<TonometerModel> deserializedTonometerData;

    //public string lastMessage = "o";
    //public static string tonometerWarningMessage = "o";

    public string tonometerDataProjectFilePath;

    public static float upperValue;
    public static float lowerValue;

    int j = 0;

    float startTime, roundTime;

    //bool warning;
    public int zone { get; set; }
    public string parameter { get; set; }
    public string value { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        parameter = "Давление";

        roundTime = 450.0f;
        startTime = Time.time;

        //warning = false;

        //tonometerDataProjectFilePath = "/StreamingAssets/FirstMenPressure.json";

        FileCheck();
    }

    // Update is called once per frame
    void Update()
    {
        //SerializingData();

        //MoveByHotkey();

        TonometerChangeByDoc();

        //StateInfoOutput();
    }

    public Message GetMessageInfo()
    {
        Message mess = new Message(zone, parameter, value);
        return mess;
    }

    //void StateInfoOutput()
    //{
    //    if (Time.time - startTime >= roundTime)
    //    {
    //        startTime = Time.time;
    //        warning = false;

    //        tonometerRedZone = 0;
    //    }

    //    if (tonometerRedZone >= 50 && !warning)
    //    {
    //        tonometerWarningMessage = "Character pressure: WARNING! At " + System.DateTime.Now;
    //        Debug.Log("Character pressure: WARNING! At " + System.DateTime.Now);
    //        warning = true;
    //    }

    //}

    void TonometerState()
    {
        if (((upperValue >= 115) & (upperValue <= 140)) && ((lowerValue >= 60)) && (lowerValue <= 85))
        {
            //Debug.Log("Tonometer: GreenZone");
            zone = 0;
        }

        if (((upperValue >= 90) && (upperValue < 115)) || ((upperValue > 140) && (upperValue <= 160)) || ((lowerValue < 60) && (lowerValue >= 50)) || ((lowerValue > 85) && (lowerValue <= 95)))
        {
            //Debug.Log("Tonometer: YellowZone");
            //tonometerRedZone++;
            zone = 1;
        }

        if ((upperValue < 90) || (upperValue > 160) || (lowerValue < 50) || (lowerValue > 95))
        {
            //Debug.Log("Tonometer: RedZone");
            zone = 2;
        }

        value = upperValue.ToString() + "/" + lowerValue.ToString();

    }

    void TonometerChangeByDoc()
    {
        if (j < deserializedTonometerData.Count)
        {
            upperValue = deserializedTonometerData[j].UpperValue[0];
            lowerValue = deserializedTonometerData[j].LowerValue[0];

            //Debug.Log(upperValue);
            //Debug.Log(lowerValue);

            j++;

            TonometerState();
        }
        else
        {
            j = 0;
        }
    }

    void MoveByHotkey()
    {
        if (Input.GetKey(KeyCode.T))
        {
            upperValue += 1;
        }

        if (Input.GetKey(KeyCode.G))
        {
            upperValue -= 1;
        }

        if (Input.GetKey(KeyCode.I))
        {
            lowerValue += 1;
        }

        if (Input.GetKey(KeyCode.J))
        {
            lowerValue -= 1;
        }

        //upperValueArea.text = upperValue.ToString();
        //lowerValueArea.text = lowerValue.ToString();
    }

    void FileCheck()
    {
        if (File.Exists(Application.dataPath + tonometerDataProjectFilePath))
        {
            DeserializingData();
        }

        if (deserializedTonometerData == null)
        {
            deserializedTonometerData = new List<TonometerModel>();
        }
    }

    public void SerializingData()
    {
        System.DateTime time;
        TonometerModel savedData = new TonometerModel();

        time = System.DateTime.Now;
        savedData.AddTonometerModelData(time, upperValue, lowerValue);

        tonometerData.tonometerData.Add(savedData);
        SaveTonometerData();
    }

    public void SaveTonometerData()
    {
        if (!Directory.Exists(Application.dataPath + tonometerDataProjectFilePath))
        {
            string dataAsJson = JsonHelper.ToJson(tonometerData.tonometerData); //convert to JSON-file
            string filePath = Application.dataPath + tonometerDataProjectFilePath; //path to JSON-file
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    public void DeserializingData()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + tonometerDataProjectFilePath);
        deserializedTonometerData = JsonHelper.FromJson<TonometerModel>(dataAsJson);
    }
}
