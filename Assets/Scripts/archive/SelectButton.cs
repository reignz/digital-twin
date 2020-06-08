using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButton : DropDownNotificationPanelController, IPointerEnterHandler, IPointerExitHandler
{
    Image img;
    public GameObject notBlock;

    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        notBlock.SetActive(true);
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        notBlock.SetActive(false);
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }
}
