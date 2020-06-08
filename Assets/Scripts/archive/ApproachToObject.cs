using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApproachToObject : MonoBehaviour
{
    public GameObject cam;
    public new Camera camera;
    Ray ray;
    RaycastHit hit;
    float dist = 300;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                foreach (var hit in hits)
                {
                    if (hit.transform.name == gameObject.name)
                    {
                        //Vector3 hitPoint = hit.point;
                        //hitPoint.

                        cam.transform.position = hit.point - cam.transform.forward * dist * Time.deltaTime;
                        //cam.transform.LookAt(hit.transform.gameObject.transform.position);
                        Debug.Log("hit " + hit.transform.gameObject.name);
                    }
                }
            }
        }
    }
}
