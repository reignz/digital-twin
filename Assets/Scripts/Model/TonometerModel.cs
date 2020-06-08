using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TonometerModel
{
    public List<string> dateTime = new List<string>();

    public List<float> UpperValue = new List<float>();

    public List<float> LowerValue = new List<float>();

    public void AddTonometerModelData(DateTime time, float upperValue, float lowerValue)
    {
        dateTime.Add(time.ToString());
        UpperValue.Add(upperValue);
        LowerValue.Add(lowerValue);
    }
}
