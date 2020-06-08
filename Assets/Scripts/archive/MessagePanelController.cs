using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : DBMySQLUtils
{
    //DBMySQLUtils db = new DBMySQLUtils();

    public TextMeshProUGUI noInfo;
    public TextMeshProUGUI messagesCount;
    public Button messageButton;

    public GameObject scrollview;
    public TextMeshProUGUI noNotText;
    public GameObject gridPrefab;
    public GameObject content;

    public Sprite yellowTriangle;
    public Sprite redTriangle;

    private Image label;
    private TextMeshProUGUI placeholder;
    private TextMeshProUGUI creationDate;
    private TextMeshProUGUI textInfo;

    GameObject[] gridPrefs = new GameObject[0];

    int count;

    List<Notification> nList;

    List<Notification> lastnList = new List<Notification>();

    protected override void Awake()
    {

    }

    protected override void OnApplicationQuit()
    {

    }

    //Start is called before the first frame update
    void Start()
    {
        nList = new List<Notification>();

        //scrollview.SetActive(false);
        //gameObject.GetComponent<RectTransform>().offsetMin = new Vector2
                //(gameObject.GetComponent<RectTransform>().offsetMin.x, 65.7f);

        messageButton.image.color = new Color(messageButton.image.color.r, messageButton.image.color.g, messageButton.image.color.b, 0.5f);
        messagesCount.color = new Color(messagesCount.color.r, messagesCount.color.g, messagesCount.color.b, 0.5f);
        count = int.Parse(messagesCount.text.Trim(new Char[] { '(', ')' }));
    }

    // Update is called once per frame
    void Update()
    {
        //if (count > 0)
        //{
        //    messageButton.image.color = new Color(messageButton.image.color.r, messageButton.image.color.g, messageButton.image.color.b, 1f);
        //    messagesCount.color = new Color(messagesCount.color.r, messagesCount.color.g, messagesCount.color.b, 1f);
        //}

        //if (scrollview.activeInHierarchy) //activity check
        //{
        //    if (nList != lastnList && gridPrefs != null)
        //    {
        //        for (int i = 0; i < gridPrefs.Count(); i++)
        //        {
        //            Destroy(gridPrefs[i]);
        //        }

        //        for (int i = 0; i < count; i++)
        //        {
        //            gridPrefs[i] = Instantiate(gridPrefab);
        //            gridPrefs[i].transform.SetParent(content.transform);

        //            label = gridPrefab.transform.Find("Label").GetComponent<Image>();
        //            placeholder = gridPrefab.transform.Find("Placeholder").GetComponent<TextMeshProUGUI>();
        //            creationDate = gridPrefab.transform.Find("CreationDate").GetComponent<TextMeshProUGUI>();

        //            if (nList[i].Priority == false)
        //                label.sprite = yellowTriangle;
        //            if (nList[i].Priority == true)
        //                label.sprite = redTriangle;

        //            placeholder.text = nList[i].Placeholder;
        //            creationDate.text = nList[i].CreationDate.ToString();

        //            lastnList = nList;
        //        }
        //    }
        //}
    }

    protected override void ProcessNotifications(List<Notification> notificationsList)
    {
        base.ProcessNotifications(notificationsList);

        List<Notification> trimmedNotifications = new List<Notification>();
        trimmedNotifications = notificationsList.Where(z => z.Active == true).OrderBy(x => x.Priority).ThenByDescending(y => y.CreationDate).ToList();

        count = trimmedNotifications.Count;
        messagesCount.text = '(' + count.ToString() + ')';

        if (count > 0)
        {
            messageButton.image.color = new Color(messageButton.image.color.r, messageButton.image.color.g, messageButton.image.color.b, 1f);
            messagesCount.color = new Color(messagesCount.color.r, messagesCount.color.g, messagesCount.color.b, 1f);
        }

        //if (scrollview.activeInHierarchy) //activity check
        {
            if (nList != lastnList)
            {
                for (int i = 0; i < gridPrefs.Count(); i++)
                {
                    Destroy(gridPrefs[i]);
                }

                gridPrefs = new GameObject[count];
                for (int i = 0; i < count; i++)
                {
                    gridPrefs[i] = Instantiate(gridPrefab);
                    gridPrefs[i].transform.SetParent(content.transform);

                    label = gridPrefab.transform.Find("Label").GetComponent<Image>();
                    placeholder = gridPrefab.transform.Find("Placeholder").GetComponent<TextMeshProUGUI>();
                    creationDate = gridPrefab.transform.Find("CreationDate").GetComponent<TextMeshProUGUI>();
                    textInfo = gridPrefab.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                    if (trimmedNotifications[i].Priority == false)
                        label.sprite = yellowTriangle;
                    if (trimmedNotifications[i].Priority == true)
                        label.sprite = redTriangle;

                    placeholder.text = trimmedNotifications[i].Placeholder;
                    creationDate.text = trimmedNotifications[i].CreationDate.ToString();
                    textInfo.text = trimmedNotifications[i].Text;

                    lastnList = trimmedNotifications;
                }
            }
        }
    }

    //public void SortNotificationsFromDatabase(List<Notification> notificationsList)
    //{
    //    nList = nList.Where(z => z.Active == true).OrderBy(x => x.Priority).ThenByDescending(y => y.CreationDate).ToList();

    //    count = nList.Count;
    //    messagesCount.text = '(' + count.ToString() + ')';
    //}

    public void OnMessageButton()
    {
        scrollview.SetActive(!scrollview.activeInHierarchy);

        if (gameObject.GetComponent<RectTransform>().offsetMin.y == 65.7f)
        {
            //opening panel
            gameObject.GetComponent<RectTransform>().offsetMin = new Vector2
                (gameObject.GetComponent<RectTransform>().offsetMin.x, 283.52f);
        }
        else
        {
            gameObject.GetComponent<RectTransform>().offsetMin = new Vector2
                (gameObject.GetComponent<RectTransform>().offsetMin.x, 65.7f);
        }

        //if (gameObject.GetComponent<RectTransform>().offsetMin.y == 339)
        //{
        //    //opening panel
        //    gameObject.GetComponent<RectTransform>().offsetMin = new Vector2
        //        (gameObject.GetComponent<RectTransform>().offsetMin.x, 110);

        //    scrollview.SetActive(true);

        //    if (count > 0)
        //    {
        //        gridPrefs = new GameObject[count];
        //        //there is some notes
        //        for (int i = 0; i < count; i++)
        //        {
        //            gridPrefs[i] = Instantiate(gridPrefab);
        //            gridPrefs[i].transform.SetParent(content.transform);

        //            label = gridPrefab.transform.Find("Label").GetComponent<Image>();
        //            placeholder = gridPrefab.transform.Find("Placeholder").GetComponent<TextMeshProUGUI>();
        //            creationDate = gridPrefab.transform.Find("CreationDate").GetComponent<TextMeshProUGUI>();
        //            Button btn_gotoBreakdown = gridPrefab.transform.Find("GoToBreakdown").GetComponent<Button>();

        //            if (nList[i].Priority == false)
        //                label.sprite = yellowTriangle;
        //            else
        //                label.sprite = redTriangle;

        //            placeholder.text = nList[i].Placeholder;
        //            creationDate.text = nList[i].CreationDate.ToString();

        //            int tag = 0;

        //            if (nList[i].User_ID != null)
        //                tag = (int)nList[i].User_ID;
        //            if (nList[i].Requirement_ID != null)
        //                tag = (int)nList[i].Requirement_ID;

        //            btn_gotoBreakdown.onClick.AddListener(() => OnBreakdownButton(tag));
        //        }
        //    }
        //    else
        //    {
        //        //there no notifications
        //        noNotText.enabled = true;
        //    }
        //}
        //else
        //{
        //    gameObject.GetComponent<RectTransform>().offsetMin = new Vector2
        //        (gameObject.GetComponent<RectTransform>().offsetMin.x, 339);

        //    foreach (Transform child in content.transform)
        //    {
        //        if (!child.GetComponent<TextMeshProUGUI>())
        //        Destroy(child.gameObject);
        //    }
        //    scrollview.SetActive(false);

        //    noNotText.enabled = false;
        //}
    }
}
