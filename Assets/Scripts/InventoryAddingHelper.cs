using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryAddingHelper : MonoBehaviour, IPointerUpHandler
{
    public GameObject requirement;
    public Camera mainCamera;
    public float angleX;
    public float angleY;
    public float angleZ;

    //public void OnClick()
    //{
    //    Debug.Log("Click!");

    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Instantiate(requirement, hit.point, Quaternion.Euler(angleX, angleY, angleZ));
    //    }
    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");

        InstantiateObject();
    }

    private void InstantiateObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(requirement, hit.point, Quaternion.Euler(angleX, angleY, angleZ));
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{

    //}
}
