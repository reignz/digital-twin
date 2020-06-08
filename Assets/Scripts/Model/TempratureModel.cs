using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TempratureModel
{

    public List<float> Temprature = new List<float>();

    public List<string> dateTime = new List<string>();

    public void AddPipeTempratureModelData(DateTime time, float temprature)
    {
        Temprature.Add(temprature);
        dateTime.Add(time.ToString());
    }
}
