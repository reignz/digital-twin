using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterTermometreModel
{
    public List<string> dateTime = new List<string>();

    public List<double> Temperature = new List<double>();

    public void AddCharacterTermometreModelData(DateTime time, double temperature)
    {
        dateTime.Add(time.ToString());
        Temperature.Add(temperature);
    }
}
