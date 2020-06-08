using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SortNotifidcations : MonoBehaviour
{
    public GameObject[] line;
    public GameObject[] text;

    public Button reqButton;
    public Button empButton;

    public Sprite selected;
    public Sprite nonSelected;

    bool isReq;

    public GameObject[] reqNotificationGroups;
    public GameObject[] empNotificationGroups;

    // Start is called before the first frame update
    void Start()
    {
        isReq = true;

        reqButton.GetComponent<Image>().sprite = selected;
        empButton.GetComponent<Image>().sprite = nonSelected;

        for (int i = 0; i < reqNotificationGroups.Length; i++)
        {
            reqNotificationGroups[i].SetActive(false);
            empNotificationGroups[i].SetActive(false);
        }
        reqNotificationGroups[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRequirementNotButton()
    {
        isReq = true;
        SelectField(0, isReq);
        reqButton.GetComponent<Image>().sprite = selected;
        empButton.GetComponent<Image>().sprite = nonSelected;
    }

    public void OnEmployeeNotButton()
    {
        isReq = false;
        SelectField(0, isReq);
        reqButton.GetComponent<Image>().sprite = nonSelected;
        empButton.GetComponent<Image>().sprite = selected;
    }

    public void OnAllNotButton()
    {
        SelectField(0, isReq);
    }

    public void OnAcceptedNotButton()
    {
        SelectField(1, isReq);
    }

    public void OnRejectedNotButton()
    {
        SelectField(2, isReq);
    }

    public void OnLastNotButton()
    {
        SelectField(3, isReq);
    }

    void SelectField(int selected, bool isReq)
    {
        for (int i = 0; i < line.Length; i++)
        {
            line[i].SetActive(false);
            text[i].GetComponent<TextMeshProUGUI>().color = new Color(
                text[i].GetComponent<TextMeshProUGUI>().color.r,
                text[i].GetComponent<TextMeshProUGUI>().color.g,
                text[i].GetComponent<TextMeshProUGUI>().color.b,
                0.5f);
        }

        line[selected].SetActive(true);
        text[selected].GetComponent<TextMeshProUGUI>().color = new Color(
                text[selected].GetComponent<TextMeshProUGUI>().color.r,
                text[selected].GetComponent<TextMeshProUGUI>().color.g,
                text[selected].GetComponent<TextMeshProUGUI>().color.b,
                1f);

        if (isReq)
        {
            ShowNot(selected, reqNotificationGroups, empNotificationGroups);
        }
        else
        {
            ShowNot(selected, empNotificationGroups, reqNotificationGroups);
        }
    }

    void ShowNot(int selected, GameObject[] arr, GameObject[] arr2)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].SetActive(false);
            arr2[i].SetActive(false);
        }
        arr[selected].SetActive(true);
    }
}
