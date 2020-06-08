using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDraggingHelper : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    Camera mainCamera;
    public float angleX;
    public float angleY;
    public float angleZ;
    public float yPos;

    void Start()
    {
        mainCamera = GameObject.Find("/Sphere/Main Camera").GetComponent<Camera>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BEGINDRAG");

        //InstantiateObject();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        InstantiateObject();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ENDDRAG");

        //InstantiateObject();
    }

    private void InstantiateObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Instantiate(gameObject, hit.point, Quaternion.Euler(angleX, angleY, angleZ));
            
            gameObject.transform.position = hit.point;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, yPos, gameObject.transform.position.z);
        }
    }
}
