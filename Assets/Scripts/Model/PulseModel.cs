using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PulseModel
{
    public List<string> dateTime = new List<string>();

    public List<float> Pulse = new List<float>();

    public void AddPulseModelData(DateTime time, float pulse)
    {
        dateTime.Add(time.ToString());
        Pulse.Add(pulse);
    }
}
