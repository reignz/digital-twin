using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnReqAddingButtonClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject reqEditDataPanel;

    private void Awake()
    {
        reqEditDataPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        reqEditDataPanel.SetActive(true);
    }

}
