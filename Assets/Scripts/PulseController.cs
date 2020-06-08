using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class PulseController : Pulse
{ 
    public TextMeshProUGUI pulseValueArea;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PulseMovementByDoc();
    }

    void PulseMovementByDoc()
    {
        pulseValueArea.text = pulseValue.ToString() + "bpm";
    }
}
