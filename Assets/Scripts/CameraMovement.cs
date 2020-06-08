using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed = 10; //velocity 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (/*Input.GetKey(KeyCode.W) || */Input.GetKey(KeyCode.UpArrow)) /*(Input.GetAxis("Mouse ScrollWheel") > 0f) //forward*/
        {
            gameObject.transform.position += gameObject.transform.up * speed * Time.deltaTime;
        }

        if (/*Input.GetKey (KeyCode.A) || */Input.GetKey (KeyCode.LeftArrow)) //left
        {
            gameObject.transform.position -= gameObject.transform.right * speed * Time.deltaTime;
        }

        if (/*Input.GetKey(KeyCode.S) || */Input.GetKey(KeyCode.DownArrow)) /*(Input.GetAxis("Mouse ScrollWheel") < 0f) //backwards*/
        {
            gameObject.transform.position -= gameObject.transform.up * speed * Time.deltaTime;
        }

        if (/*Input.GetKey (KeyCode.D) || */Input.GetKey (KeyCode.RightArrow))
        {
            gameObject.transform.position += gameObject.transform.right * speed * Time.deltaTime;
        }

	}
}
