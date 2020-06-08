using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class TonometerController : Tonometer
{
    public TextMeshProUGUI pressValueArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TonometerMovementByDoc();
    }

    void TonometerMovementByDoc()
    {
        pressValueArea.text = upperValue.ToString() + "/" + lowerValue.ToString() + " mm/hg";
    }
}
