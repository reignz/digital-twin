using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RequirementController : DBMySQLUtils
{
    Thread addOrChangeNot;

    BaseHelper helper = new BaseHelper();

    public Image label;

    public Sprite redLabel;
    public Sprite yellowLabel;

    float startTimeN;
    float roundTimeN;

    int zone = 0;
    int nextZone = 0;

    bool isDone;
    bool inserting;

    public static List<Message> messages = new List<Message>();

    List<Notification> trimmedNotifications;

    protected override void Awake()
    {

    }

    protected override void OnApplicationQuit()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        startTimeN = Time.time;
        roundTimeN = 120.0f;

        //StartCoroutine(ReadActiveNotificationsByReqId(int.Parse(gameObject.tag)));
    }

    // Update is called once per frame
    void Update()
    {
        messages.Add(gameObject.GetComponent<Barometer>().GetMessageInfo());
        messages.Add(gameObject.GetComponent<Temprature>().GetMessageInfo());

        AddNot();
    }

    void AddNot()
    {
        if (addOrChangeNot != null)
        {
            if (!addOrChangeNot.IsAlive && isDone)
            {
                isDone = false;

                timer = null;
                ReadAllInfoThread();
            }
        }

        if (messages.Count != 0 && !inserting)
        {
            #region baromerter properties
            int barometerZone = messages[0].Zone;
            string barometerParameter = messages[0].Parameter;
            string barometerValue = messages[0].Value;
            #endregion

            #region temprature properties
            int tempratureZone = messages[1].Zone;
            string tempratureParameter = messages[1].Parameter;
            string tempratureValue = messages[1].Value;
            #endregion

            #region data analysis

            int sumZone = barometerZone + tempratureZone;

            if (sumZone == 0)
            {
                //green
                nextZone = 0;
            }
            else
            {
                if (sumZone == 1)
                {
                    //yellow
                    nextZone = 1;
                }
                else
                {
                    //red
                    nextZone = 2;
                }
            }

            if ((nextZone > zone) || ((Time.time - startTimeN >= roundTimeN) && (nextZone != 0)))
            {
                int ID = int.Parse(gameObject.tag);
                Requirement problemRequirement = requirements.Where(x => x.ID == ID).First();
                Notification n = new Notification();
                n.Placeholder = "Возникла проблема: " + problemRequirement.Name + " " + problemRequirement.SerialNumber + ".";
                n.Text = barometerParameter + " (" + helper.ZoneTranslation(barometerZone) + " зона) : " + (int)(float.Parse(barometerValue)) + ".\n" +
                    tempratureParameter + " (" + helper.ZoneTranslation(tempratureZone) + " зона) : " + (int)(float.Parse(tempratureValue)) + ".\n";

                if (nextZone == 1) n.Priority = false;
                if (nextZone == 2) n.Priority = true;

                n.CreationDate = System.DateTime.Now;
                n.Requirement_ID = ID;
                n.Active = true;

               trimmedNotifications = notifications.Where(x => x.Requirement_ID == ID).OrderBy(x => x.CreationDate).ToList();

                Notification lastNotification = new Notification();
                if (trimmedNotifications.Count != 0)
                {
                    bool isReplacement = NotificationReplacement(trimmedNotifications, n);

                    lastNotification = trimmedNotifications.Last();

                    if (!isReplacement)
                    {
                        if (n.Priority == false)
                        {
                            //if notification about yellow zone
                            if (lastNotification.Priority == false)
                            {
                                //if last notification about yellow zone, set nonActive lastNotification and insert newNotification (req)
                                //Debug.Log("if last notification about yellow zone, set nonActive lastNotification and insert newNotification (req)");
                                UpdateDatabase(n, ID, lastNotification, true);
                            }
                            else
                            {
                                //if last notification about red zone, set nonActive lastNotification and insert newNotification (req)
                                //Debug.Log("if last notification about red zone, set nonActive lastNotification and insert newNotification (req)");
                                UpdateDatabase(n, ID, lastNotification, false);
                            }
                        }
                        else
                        {
                            //if notification about red zone
                            if (lastNotification.Priority == false)
                            {
                                //if last notification about yellow zone, set nonActive lastNotification and insert newNotification (req)
                                UpdateDatabase(n, ID, lastNotification, true);
                            }
                            else
                            {
                                //if last notification about red zone, set nonActive lastNotification and insert newNotification (req)
                                Debug.Log("if last notification about red zone, set nonActive lastNotification and insert newNotification (req)");
                                UpdateDatabase(n, ID, lastNotification, true);
                            }
                        }
                    }
                }
                else
                {
                    //if notification about red/yellow zone and no notifications before, insert newNotification (req)
                    Debug.Log("if notification about red/yellow zone and no notifications before, insert newNotification (req)");
                    UpdateDatabase(n, ID, lastNotification, false);
                }

                zone = nextZone;
            }
            #endregion
        }

        if (trimmedNotifications == null)
        {
            label.gameObject.SetActive(false);
        }
        messages.Clear();
    }

    protected override void ProcessNotifications(List<Notification> notificationsList)
    {
        base.ProcessNotifications(notificationsList);
    }

    public bool NotificationReplacement(List<Notification> nList, Notification n)
    {
        int k = 0;
        for (int i = 0; i < nList.Count; i++)
        {
            if ((nList[i].Placeholder == n.Placeholder) && (nList[i].Priority == n.Priority) && (nList[i].Text == n.Text) &&
                (nList[i].Requirement_ID == n.Requirement_ID) && (nList[i].User_ID == n.User_ID) /*&& (nList[i].Active == n.Active)*/)
            {
                Debug.Log("replace not (req)");
                n.ID = nList[i].ID;

                StopTimerAndThread();
                startTimeN = Time.time;
                addOrChangeNot = new Thread(delegate ()
                {
                    inserting = true;
                    UpdateNotification(n);
                    isDone = true;
                    inserting = false;
                });
                addOrChangeNot.Start();
                k++;
            }
        }

        if (k == 0) return false;
        else return true;
    }

    void UpdateDatabase(Notification n, int ID, Notification lastNotification, bool isUpdateLastNot)
    {
        label.gameObject.SetActive(true);
        if (n.Priority == false)
        {
            label.sprite = yellowLabel;
        }
        else
        {
            label.sprite = redLabel;
        }

        StopTimerAndThread();
        startTimeN = Time.time;
        addOrChangeNot = new Thread(delegate ()
        {
            inserting = true;
            if (isUpdateLastNot)
            {
                InsertNotificationAboutRequirement(n, ID);

                lastNotification.Active = false;
                UpdateNotification(lastNotification);
            }
            else
            {
                InsertNotificationAboutRequirement(n, ID);
            }
            isDone = true;
            inserting = false;
        });
        addOrChangeNot.Start();
    }
}
