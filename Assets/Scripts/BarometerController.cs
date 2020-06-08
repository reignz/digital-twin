using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;

public class BarometerController : Barometer
{
    private float Z;

    public TextMeshProUGUI pressInput;

    // Start is called before the first frame update
    void Start()
    {
        Z = gameObject.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        BarometerMovementByDoc();

    }

    void BarometerMovementByDoc()
    {
        //Z = (pressure * 90) / 760;
        //gameObject.transform.rotation = Quaternion.Euler(0, 0, Z);

        pressInput.text = Math.Truncate(pressure).ToString() + " Па";
    }

}
