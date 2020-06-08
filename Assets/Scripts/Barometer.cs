using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Barometer : MonoBehaviour
{
    public TextMeshProUGUI test;

    public GameObject arrow;

    public BarometerData barometerData;
    public List<BarometerModel> deserializedBarometerData;

    public string barometerDataProjectFilePath;

    public static float pressure;

    private float z;
    private float stride = 5f;
    private float littleStride = 2f;

    int j = 0;

    float startTime, roundTime;

    public int zone { get; set; }
    public string parameter { get; set; }
    public string value { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        pressure = float.Parse(test.text);

        parameter = "Давление";

        z = arrow.transform.rotation.eulerAngles.z;

        roundTime = 30.0f;
        startTime = Time.time;

        FileCheck();
    }

    // Update is called once per frame
    void Update()
    {
        //SerializingData();

        //MoveByHotkey();

        BarometerChangeByDoc();
    }

    public Message GetMessageInfo()
    {
        Message mess = new Message(zone, parameter, value);
        return mess;
    }

    void BarometerState()
    {
        if ((pressure <= 770) && (pressure >= 750))
        {
            zone = 0;
        }

        if (((pressure <= 790) && (pressure > 770)) || ((pressure < 750) && (pressure >= 730)))
        {
            zone = 1;
        }

        if (((pressure > 790) && (pressure < 730)))
        {
            zone = 2;
        }

        value = pressure.ToString();
    }

    void BarometerChangeByDoc()
    {
        if (j < deserializedBarometerData.Count)
        {
            pressure = deserializedBarometerData[j].Pressure[0]; 

            j++;
            
            BarometerState();
        }
        else
        {
            j = 0;
        }
    }

    void MoveByHotkey()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            pressure -= stride * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.X))
        {
            pressure += stride * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.N))
        {
            pressure -= littleStride * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.M))
        {
            pressure += littleStride * Time.deltaTime;
        }

        test.text = pressure.ToString();
        //arrow.transform.rotation = Quaternion.Euler(0, 0, z);
        //pressure = (z * 760) / 90;
    }

    void FileCheck()
    {
        if (File.Exists(Application.dataPath + barometerDataProjectFilePath))
        {
            DeserializingData();
        }

        if (deserializedBarometerData == null)
        {
            deserializedBarometerData = new List<BarometerModel>();
        }
    }

    public void SerializingData()
    {
        System.DateTime time;
        BarometerModel savedData = new BarometerModel();
        
        time = System.DateTime.Now;
        savedData.AddPipePressureModelData(time, pressure);

        barometerData.barometerData.Add(savedData);
        SavePressureData();
    }

    public void SavePressureData()
    {
        if (!Directory.Exists(Application.dataPath + barometerDataProjectFilePath))
        {
            string dataAsJson = JsonHelper.ToJson(barometerData.barometerData); //convert to JSON-file
            string filePath = Application.dataPath + barometerDataProjectFilePath; //path to JSON-file
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    public void DeserializingData()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + barometerDataProjectFilePath);
        deserializedBarometerData = JsonHelper.FromJson<BarometerModel>(dataAsJson);
    }
}
