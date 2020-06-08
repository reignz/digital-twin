using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownNotificationPanelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject notPanel;

    public Sprite rectangleTop;

    float time;
    float roundTime;
    bool isTimeout;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = rectangleTop;
        roundTime = 2f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        notPanel.SetActive(true);
        gameObject.GetComponent<Image>().sprite = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        notPanel.SetActive(false);
        gameObject.GetComponent<Image>().sprite = rectangleTop;
    }

    //void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    //{
    //    notPanel.SetActive(true);

    //}

    //void IPointerExitHandler.OnPointerExit(PointerEventData e)
    //{
    //    notPanel.SetActive(false);
    //}
}
