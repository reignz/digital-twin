using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VecQuat
{
    public Vector3 vector3 { get; set; }
    public Quaternion quaternion { get; set; }

    public VecQuat(Vector3 vector3, Quaternion quaternion)
    {
        this.vector3 = vector3;
        this.quaternion = quaternion;
    }
}

public class PropertiesAndCourutines : MonoBehaviour {

    public float smoothing = 7.0f;
    private Quaternion quat;
    public VecQuat Target
    {
        get { return target; }
        set
        {
            target = value;

            StopCoroutine("Movement");
            StartCoroutine("Movement", target);
        }
    }

    private VecQuat target;

	IEnumerator Movement (VecQuat target)
    {
        quat = target.quaternion;
        while (Vector3.Distance(transform.position, target.vector3) > 0.005f)
        {
            transform.position = Vector3.Lerp(transform.position, target.vector3, smoothing * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, smoothing * Time.deltaTime);
            yield return null;
        }
    }
}
