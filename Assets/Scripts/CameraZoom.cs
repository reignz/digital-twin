using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float speed = 15f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) //forward
        {
            gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f) //backwards
        {
            gameObject.transform.position -= gameObject.transform.forward * speed * Time.deltaTime;
        }
    }

}

