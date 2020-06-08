using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Temprature : MonoBehaviour
{
    public TextMeshProUGUI test;

    public TempratureData tempratureData;
    public List<TempratureModel> deserializedTempratureData;

    public string tempratureDataProjectFilePath;

    public float stride = 15f;
    public float littleStride = 5f;

    float startTime, roundTime;

    int j = 0;

    public static float temprature;
    public static float des;

    public int zone { get; set; }
    public string parameter { get; set; }
    public string value { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        temprature = float.Parse(test.text);

        parameter = "Температура";

        roundTime = 30.0f;
        startTime = Time.time;

        //tempratureDataProjectFilePath = "/StreamingAssets/FirstPipeTemprature.json";

        FileCheck();
    }

    // Update is called once per frame
    void Update()
    {
        //SerializingData();

        //MoveByHotkey();

        TempChangeByDoc();
    }
    
    void TempState()
    {
        if ((temprature >= 10) && (temprature <= 30))
        {
            zone = 0;
        }

        if (((temprature < 10) && (temprature >= -10)) || ((temprature > 30) && (temprature <= 50)))
        {
            zone = 1;
        }

        if ((temprature > 50) || (temprature < -10))
        {
            zone = 2;
        }

        value = temprature.ToString();
    }
    public Message GetMessageInfo()
    {
        Message mess = new Message(zone, parameter, value);
        return mess;
    }

    void TempChangeByDoc()
    {
        if (j < deserializedTempratureData.Count)
        {
            temprature = deserializedTempratureData[j].Temprature[0];
            
            j++;
            
            TempState();
        }
        else
        {
            j = 0;
        }
    }

    void MoveByHotkey()
    {
        if (Input.GetKey(KeyCode.O))
        {
            temprature += stride * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.L))
        {
            temprature -= stride * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R))
        {
            temprature += littleStride * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.F))
        {
            temprature -= littleStride * Time.deltaTime;
        }

        test.text = temprature.ToString();
    }

    void FileCheck()
    {
        if (File.Exists(Application.dataPath + tempratureDataProjectFilePath))
        {
            DeserializingData();
        }

        if (deserializedTempratureData == null)
        {
            deserializedTempratureData = new List<TempratureModel>();
        }
    }

    public void SerializingData()
    {
        System.DateTime time;
        TempratureModel savedData = new TempratureModel();
        
        time = System.DateTime.Now;
        savedData.AddPipeTempratureModelData(time, temprature);

        tempratureData.pipeTempratureData.Add(savedData);
        SaveTempratureData();
    }

    public void SaveTempratureData()
    {
        if (!Directory.Exists(Application.dataPath + tempratureDataProjectFilePath))
        {
            string dataAsJson = JsonHelper.ToJson(tempratureData.pipeTempratureData); //convert to JSON-file
            string filePath = Application.dataPath + tempratureDataProjectFilePath; //path to JSON-file
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    public void DeserializingData()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + tempratureDataProjectFilePath);
        deserializedTempratureData = JsonHelper.FromJson<TempratureModel>(dataAsJson);
    }
}
