using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class CharacterTermometreController : CharacterTermometre
{ 
    public TextMeshProUGUI tempratureValueArea;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TempratureMovementByDoc();
    }

    void TempratureMovementByDoc()
    {
        tempratureValueArea.text = tempratureValue.ToString() + "°С";
    }

}
