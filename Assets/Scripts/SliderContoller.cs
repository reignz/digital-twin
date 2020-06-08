using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderContoller : MonoBehaviour
{
    Slider slider;
    public new GameObject camera;
    public float speed = 40f;
    float val;
    float newVal;
    float d;

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        val = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        //slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck(float newValue)
    {
        //newVal = slider.value;
        //d = newVal - val;
        camera.transform.localPosition = camera.transform.localPosition + camera.transform.forward * newValue * speed * Time.deltaTime;
        //Debug.Log(slider.value);
        //val = newVal;
    }
}
