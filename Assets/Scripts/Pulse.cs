using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public PulseData pulseData;
    public List<PulseModel> deserializedPulseData;

    //public string lastMesage = "o";
    //public static string pulseWarningMessage = "o";

    public string pulseDataProjectFilePath;

    int pulseRedZone = 0;

    int j = 0;

    public static float pulseValue;

    float startTime, roundTime;

    bool warning;
    public static int zone;
    public static string parameter;
    public static string value;

    // Start is called before the first frame update
    void Start()
    {
        parameter = "Пульс";
        roundTime = 450.0f;
        startTime = Time.time;

        warning = false;

        //pulseDataProjectFilePath = "/StreamingAssets/FirstMenPulse.json";

        FileCheck();
    }

    // Update is called once per frame
    void Update()
    {
        //SerializingData();

        //MoveByHotkey();

        PulseChangeByDoc();

        //StateInfoOutput();
    }

    //public bool IsWarning()
    //{
    //    if (pulseWarningMessage != lastMesage)
    //    {
    //        lastMesage = pulseWarningMessage;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

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

    //        pulseRedZone = 0;
    //    }

    //    if (pulseRedZone >= 50 && !warning)
    //    {
    //        pulseWarningMessage = "Character pulse: WARNING! At " + System.DateTime.Now;
    //        Debug.Log("Character pulse: WARNING! At " + System.DateTime.Now);
    //        warning = true;
    //    }

    //}

    void PulseState()
    {
        if ((pulseValue >= 60) & (pulseValue <= 80))
        {
            //Debug.Log("Pulse: GreenZone");
            zone = 0;
        }

        if (((pulseValue < 60) && (pulseValue >= 50)) || ((pulseValue > 80) && (pulseValue <= 110)))
        {
            //Debug.Log("Pulse: YellowZone");
            //pulseRedZone++;
            zone = 1;
        }

        if ((pulseValue < 50) || (pulseValue > 110))
        {
            //Debug.Log("Pulse: RedZone");
            zone = 2;
        }

        value = pulseValue.ToString();
    }

    void PulseChangeByDoc()
    {
        if (j < deserializedPulseData.Count)
        {
            pulseValue = deserializedPulseData[j].Pulse[0];
            j++;

            //Debug.Log(pulseValue);

            PulseState();
        }
        else
        {
            j = 0;
        }
    }

    void MoveByHotkey()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            pulseValue += 1;
        }

        if (Input.GetKey(KeyCode.H))
        {
            pulseValue -= 1;
        }

        //pulseValueArea.text = pulseValue.ToString();
    }

    void FileCheck()
    {
        if (File.Exists(Application.dataPath + pulseDataProjectFilePath))
        {
            DeserializingData();
        }

        if (deserializedPulseData == null)
        {
            deserializedPulseData = new List<PulseModel>();
        }
    }

    public void SerializingData()
    {
        System.DateTime time;
        PulseModel savedData = new PulseModel();

        time = System.DateTime.Now;
        savedData.AddPulseModelData(time, pulseValue);

        pulseData.pulseData.Add(savedData);
        SaveTonometerData();
    }

    public void SaveTonometerData()
    {
        if (!Directory.Exists(Application.dataPath + pulseDataProjectFilePath))
        {
            string dataAsJson = JsonHelper.ToJson(pulseData.pulseData); //convert to JSON-file
            string filePath = Application.dataPath + pulseDataProjectFilePath; //path to JSON-file
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    public void DeserializingData()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + pulseDataProjectFilePath);
        deserializedPulseData = JsonHelper.FromJson<PulseModel>(dataAsJson);
    }
}
