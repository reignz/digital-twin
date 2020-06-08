using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SystemTimeController : MonoBehaviour
{
    string minute;
    string hour; 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        hour = DateTime.Now.Hour.ToString();
        if (DateTime.Now.Minute < 10)
        {
            minute = "0" + DateTime.Now.Minute.ToString();
        }
        else
        {
            minute = DateTime.Now.Minute.ToString();
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
    }
}
