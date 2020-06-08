using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterTermometre : MonoBehaviour
{
    public CharacterTermometreData characterTermometreData;
    public List<CharacterTermometreModel> deserializedTempratureData;

    //public string lastMessage = "o";
    //public static string tempratureWarningMessage = "o";

    public string tempratureDataProjectFilePath;

    int tempratureRedZone = 0;

    int j = 0;

    public static double tempratureValue;

    float startTime, roundTime;

    bool warning;
    public static int zone;
    public static string parameter;
    public static string value;

    // Start is called before the first frame update
    void Start()
    {
        parameter = "Температура";
        tempratureValue = 36.6f;

        roundTime = 450.0f;
        startTime = Time.time;

        warning = false;

        //tempratureDataProjectFilePath = "/StreamingAssets/FirstMenTemprature.json";

        FileCheck();
    }

    // Update is called once per frame
    void Update()
    {
        //SerializingData();

        //MoveByHotkey();

        TempratureChangeByDoc();

        //StateInfoOutput();
    }

    public Message GetMessageInfo()
    {
        Message mess = new Message(zone, parameter, value);
        return mess;
    }

    void TempratureState()
    {
        if ((tempratureValue >= 36.0) & (tempratureValue <= 36.6))
        {
            //Debug.Log("Temprature (character): GreenZone");
            zone = 0;
        }

        if (((tempratureValue >= 35.5) && (tempratureValue < 36.0)) || ((tempratureValue > 36.6) && (tempratureValue <= 36.9)))
        {
            //Debug.Log("Temprature (character): YellowZone");
            zone = 1;
        }

        if (((tempratureValue >= 34.0) && (tempratureValue < 35.5)) || ((tempratureValue > 36.9) && (tempratureValue <= 42)))
        {
            //Debug.Log("Temprature (character): YellowZone");
            zone = 2;
        }

        value = tempratureValue.ToString();
    }

    void TempratureChangeByDoc()
    {
        if (j < deserializedTempratureData.Count)
        {
            tempratureValue = deserializedTempratureData[j].Temperature[0];
            j++;

            //Debug.Log(tempratureValue);

            TempratureState();
        }
        else
        {
            j = 0;
        }
    }

    void MoveByHotkey()
    {
        if (Input.GetKey(KeyCode.B))
        {
            tempratureValue += 0.1f;
        }

        if (Input.GetKey(KeyCode.V))
        {
            tempratureValue -= 0.1f;
        }

        tempratureValue = Math.Round(tempratureValue, 1, MidpointRounding.AwayFromZero);
    }

    void FileCheck()
    {
        if (File.Exists(Application.dataPath + tempratureDataProjectFilePath))
        {
            DeserializingData();
        }

        if (deserializedTempratureData == null)
        {
            deserializedTempratureData = new List<CharacterTermometreModel>();
        }
    }

    public void SerializingData()
    {
        System.DateTime time;
        CharacterTermometreModel savedData = new CharacterTermometreModel();

        time = System.DateTime.Now;
        savedData.AddCharacterTermometreModelData(time, tempratureValue);

        characterTermometreData.characterTermometreData.Add(savedData);
        SaveCharacterTermometreData();
    }

    public void SaveCharacterTermometreData()
    {
        if (!Directory.Exists(Application.dataPath + tempratureDataProjectFilePath))
        {
            string dataAsJson = JsonHelper.ToJson(characterTermometreData.characterTermometreData); //convert to JSON-file
            string filePath = Application.dataPath + tempratureDataProjectFilePath; //path to JSON-file
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    public void DeserializingData()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + tempratureDataProjectFilePath);
        deserializedTempratureData = JsonHelper.FromJson<CharacterTermometreModel>(dataAsJson);
    }
}
