using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnButton : MonoBehaviour {

    public PropertiesAndCourutines coroutineScript;

    Vector3 vec;
    Quaternion quat;

    public void OnClickToFrontButton()
    {
        float x = 31.85189f;
        float y = 157.6588f;
        float z = -169.3615f;

        vec = new Vector3(x, y, z);
        quat = Quaternion.Euler(48.152f, -0.285f, 0f);

        coroutineScript.Target = new VecQuat(vec, quat);
    }

    public void OnClickToLeftButton()
    {
        float x = -265.7179f;
        float y = 54.68348f;
        float z = -36.45551f;

        vec = new Vector3(x, y, z);
        quat = Quaternion.Euler(20.261f, 70.92001f, 0f);
        coroutineScript.Target = new VecQuat(vec, quat);
    }

    public void OnClickToRightButton()
    {
        float x = 218.1031f;
        float y = 126.2106f;
        float z = -101.8188f;

        vec = new Vector3(x, y, z);
        quat = Quaternion.Euler(36.981f, -49.91f, 0f);
        coroutineScript.Target = new VecQuat(vec, quat);
    }

}
