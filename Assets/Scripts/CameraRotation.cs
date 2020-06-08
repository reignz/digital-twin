using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {

    private Vector3 mousePos;
    public GameObject go; //an object that rotates
    public Camera goCamera; //MainCamera
    private float myAngle;
    private float sensitivity = 1f;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        mousePos = Input.mousePosition;


        if (transform.rotation.eulerAngles.z != 0f)
        {
            go.transform.rotation = Quaternion.Euler(go.transform.rotation.eulerAngles.x, go.transform.rotation.eulerAngles.y, 0f);
        }

    }

    void FixedUpdate() {

        if (Input.GetMouseButton(1))
        {

            myAngle = 0;
            myAngle = sensitivity * ((mousePos.x - (Screen.width / 2)) / Screen.width);
            go.transform.RotateAround(go.transform.position, go.transform.up, myAngle);


            myAngle = -sensitivity * ((mousePos.y - (Screen.height / 2)) / Screen.height);
            go.transform.RotateAround(go.transform.position, go.transform.right, myAngle);

        }
    }
}
