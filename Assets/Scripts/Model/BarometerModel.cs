using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BarometerModel
{
    public List<string> dateTime = new List<string>();

    public List<float> Pressure = new List<float>();

    public void AddPipePressureModelData(DateTime time, float pressure)
    {
        dateTime.Add(time.ToString());
        Pressure.Add(pressure);
    }
}
