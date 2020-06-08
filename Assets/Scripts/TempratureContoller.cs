using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;

public class TempratureContoller : Temprature
{
    //float ownY;
    public TextMeshProUGUI tempInput;
    float x, y, z;

    // Start is called before the first frame update
    void Start()
    {
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.y;
        z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        TempMovementByDoc();
        
    }

    void TempMovementByDoc()
    {
        //y = (des * 131) / 325;
        //gameObject.transform.position = new Vector3(x, y, z);
        tempInput.text = Math.Truncate(temprature).ToString() + "°С";
    }
}
