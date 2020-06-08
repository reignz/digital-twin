using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

    public new Transform camera;
    Vector3 dir;
	
	// Update is called once per frame
	void Update () {
        dir.z = camera.transform.eulerAngles.y;
        transform.localEulerAngles = dir;

	}
}
