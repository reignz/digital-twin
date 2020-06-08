using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Threading;

public class EngineerController : DBMySQLUtils
{
    Thread addOrChangeNot;
    
    BaseHelper helper = new BaseHelper();

    //List<Notification> nList;

    int j = 0;

    public CoordinatesData coordinatesData;
    public List<Model> deserializedCoordinatesData;

    Animator animator;

    //public new Camera camera;
    public NavMeshAgent agent;

    public Image label;

    public Sprite redLabel;
    public Sprite yellowLabel;

    public Transform[] destPoints = new Transform[3];

    bool walk1;
    bool walk2;
    bool walk3;
    bool walk4;

    float startTime;
    float roundTime;

    float startTimeN;
    float roundTimeN;

    int zone = 0;
    int nextZone = 0;

    bool isDone;
    bool inserting;

    public static List<Message> messages = new List<Message>();

    private static string characterDataProjectFilePath;

    protected override void Awake()
    {

    }

    protected override void OnApplicationQuit()
    {

    }

    void Start()
    {
        startTimeN = Time.time;
        roundTimeN = 120.0f;

        roundTime = UnityEngine.Random.Range(5f, 10f);
        startTime = Time.time;

        walk1 = true;
        walk2 = false;
        walk3 = false;
        walk4 = false;

        animator = GetComponent<Animator>();

        characterDataProjectFilePath = "/StreamingAssets/" + gameObject.name + "Path.json";

        FileCheck();
    }

    void Update()
    {
        //SerializingData();

        //RouteByDestinationPoints();

        WalkByDoc();

        messages.Add(gameObject.GetComponent<Tonometer>().GetMessageInfo());/*[0]*/
        messages.Add(gameObject.GetComponent<Pulse>().GetMessageInfo());/*[1]*/
        messages.Add(gameObject.GetComponent<CharacterTermometre>().GetMessageInfo());/*[2]*/

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
            #region tonometer properties
            int tonometerZone = messages[0].Zone;
            string tonometerParameter = messages[0].Parameter;
            string tonometerValue = messages[0].Value;
            #endregion

            #region pulse properties
            int pulseZone = messages[1].Zone;
            string pulseParameter = messages[1].Parameter;
            string pulseValue = messages[1].Value;
            #endregion

            #region temprature properties
            int tempratureZone = messages[2].Zone;
            string tempratureParameter = messages[2].Parameter;
            string tempratureValue = messages[2].Value;
            #endregion

            #region data analysis
            int sum_zone = tonometerZone + pulseZone + tempratureZone;

            if (sum_zone == 0)
            {
                //green
                nextZone = 0;
            }

            if ((sum_zone >= 1) && (sum_zone <= 2))
            {
                if ((tonometerZone == 2) || (pulseZone == 2) || (tempratureZone == 2))
                {
                    //red
                    nextZone = 2;
                }
                else
                {
                    //yellow
                    nextZone = 1;
                }
            }

            if (sum_zone >= 3)
            {
                //red
                nextZone = 2;
            }

            if ((nextZone > zone) || ((Time.time - startTimeN >= roundTimeN) && (nextZone != 0)))
            {
                int ID = int.Parse(gameObject.tag);
                User problemUser = users.Where(x => x.ID == ID).First();
                Notification n = new Notification();
                n.Placeholder = "Возникла проблема: " + problemUser.Surname + " " + problemUser.Name.ToCharArray()[0] + "." + problemUser.Patronymic.ToCharArray()[0] + ".";
                n.Text = tonometerParameter + " (" + helper.ZoneTranslation(tonometerZone) + " зона) : " + tonometerValue + ".\n" +
                         pulseParameter + " (" + helper.ZoneTranslation(pulseZone) + " зона) : " + pulseValue + ".\n" +
                         tempratureParameter + " (" + helper.ZoneTranslation(tempratureZone) + " зона) : " + tempratureValue + ".\n";

                if (nextZone == 1) n.Priority = false;
                if (nextZone == 2) n.Priority = true;

                n.CreationDate = System.DateTime.Now;
                n.User_ID = ID;
                n.Active = true;

                List<Notification> trimmedNotifications = notifications.Where(x => x.User_ID == ID).OrderBy(x => x.CreationDate).ToList();

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
                                //if last notification about yellow zone, set nonActive lastNotification and insert newNotification
                                Debug.Log("last notification about yellow zone, set nonActive lastNotification and insert newNotification");
                                UpdateDatabase(n, ID, lastNotification, true);
                            }
                            else
                            {
                                //if last notification about red zone, set nonActive lastNotification and insert newNotification
                                Debug.Log("last notification about red zone, set nonActive lastNotification and insert newNotification");
                                UpdateDatabase(n, ID, lastNotification, false);
                            }
                        }
                        else
                        {
                            //if notification about red zone
                            if (lastNotification.Priority == false)
                            {
                                //if last notification about yellow zone, set nonActive lastNotification and insert newNotification
                                Debug.Log("last notification about yellow zone, set nonActive lastNotification and insert newNotification");
                                UpdateDatabase(n, ID, lastNotification, true);
                            }
                            else
                            {
                                //if last notification about red zone, set nonActive lastNotification and insert newNotification
                                Debug.Log("last notification about red zone, set nonActive lastNotification and insert newNotification");
                                UpdateDatabase(n, ID, lastNotification, true);
                            }
                        }
                    }
                }
                else
                {
                    //if notification about red/yellow zone and no notifications before, insert newNotification
                    Debug.Log("notification about red/yellow zone and no notifications before, insert newNotification");
                    UpdateDatabase(n, ID, lastNotification, false);
                }

                zone = nextZone;
            }
            #endregion
        }
        if (notifications != null)
        {
            if (notifications.Count != 0)
            {
                List<Notification> sortedByDateList = notifications.OrderBy(x => x.CreationDate).ToList();

                Notification n = sortedByDateList.Last();

                label.gameObject.SetActive(true);

                if (n.Priority == false)
                {
                    label.sprite = yellowLabel;
                }
                else
                {
                    label.sprite = redLabel;
                }
            }
            else
            {
                label.gameObject.SetActive(false);
            }
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
                //Debug.Log("replace not");
                n.ID = nList[i].ID;

                StopTimerAndThread();
                startTimeN = Time.time;
                addOrChangeNot = new Thread(delegate ()
                {
                   UpdateNotification(n);
                   isDone = true;
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
        StopTimerAndThread();
        startTimeN = Time.time;
        addOrChangeNot = new Thread(delegate ()
        {
            inserting = true;
            if (isUpdateLastNot)
            {
                InsertNotificationAboutUser(n, ID);

                lastNotification.Active = false;
                UpdateNotification(lastNotification);
            }
            else
            {
                InsertNotificationAboutUser(n, ID);
            }
            isDone = true;
            inserting = false;
        });
        addOrChangeNot.Start();
    }

    void WalkByDoc()
    {
        if (j < deserializedCoordinatesData.Count)
        {
            WalkByRouteFromDataBase();
            j++;
        }
        else
        {
            if (deserializedCoordinatesData[0].Rotation[0] != gameObject.transform.rotation)
            {
                gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 0.5f, transform.rotation.eulerAngles.z);

                if (transform.rotation.eulerAngles.y < (deserializedCoordinatesData[0].Rotation[0].eulerAngles.y + 1f))
                {
                    gameObject.transform.rotation = deserializedCoordinatesData[0].Rotation[0];
                }

                if (transform.rotation.eulerAngles.y >= 360)
                {
                    gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 360f, transform.rotation.eulerAngles.z);
                }
            }
            else
            {
                j = 0;
            }

        }
    }

    void WalkByRouteFromDataBase()
    {
        Vector3 newPos = deserializedCoordinatesData[j].Position[0];
        Quaternion newRot = deserializedCoordinatesData[j].Rotation[0];
        gameObject.transform.position = newPos;
        gameObject.transform.rotation = newRot;

        if (j > 0)
        {
            if (deserializedCoordinatesData[j].Position[0] == deserializedCoordinatesData[j - 1].Position[0])
            {
                animator.SetBool("walk", false);
            }
            else
            {
                animator.SetBool("walk", true);
            }
        }
    }

    void RouteByDestinationPoints()
    {
        if (walk1 && (Time.time - startTime >= roundTime))
        {
            walk1 = Walk(walk1, destPoints[0], walk2).walk;
            walk2 = Walk(walk1, destPoints[0], walk2).nextwalk;
        }

        if (walk2 && (Time.time - startTime >= roundTime))
        {
            walk2 = Walk(walk2, destPoints[1], walk3).walk;
            walk3 = Walk(walk2, destPoints[1], walk3).nextwalk;
        }

        if (walk3 && (Time.time - startTime >= roundTime))
        {
            walk3 = Walk(walk3, destPoints[2], walk4).walk;
            walk4 = Walk(walk3, destPoints[2], walk4).nextwalk;
        }

        if (walk4 && (Time.time - startTime >= roundTime))
        {
            walk4 = Walk(walk4, destPoints[3], walk1).walk;
            walk1 = Walk(walk4, destPoints[3], walk1).nextwalk;
        }

    }

    ChangeWalks Walk(bool walk, Transform destinationPoint, bool nextWalk)
    {

        ChangeWalks changeWalks = new ChangeWalks();
        if (walk)
        {
            animator.SetBool("walk", true);
            agent.SetDestination(destinationPoint.transform.position);
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= 1)
            {
                if (agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("walk", false);
                    walk = false;
                    nextWalk = true;
                    startTime = Time.time;
                    roundTime = UnityEngine.Random.Range(5f, 10f);
                    //Debug.Log("start time:" + startTime);
                    //Debug.Log("round time:" + roundTime);
                    //Debug.Log("change dest");
                }
            }
        }
        changeWalks.walk = walk;
        changeWalks.nextwalk = nextWalk;
        return changeWalks;
    }

    void FileCheck()
    {
        if (File.Exists(Application.dataPath + characterDataProjectFilePath))
        {
            DeserializingData();
        }

        if (deserializedCoordinatesData == null)
        {
            deserializedCoordinatesData = new List<Model>();
        }
    }

    public void SerializingData()
    {
        Quaternion rotation;
        Vector3 position;
        System.DateTime time;
        Model savedData = new Model();

        rotation = gameObject.transform.rotation;
        position = gameObject.transform.position;
        time = System.DateTime.Now;
        savedData.AddModelData(gameObject.name, position, rotation, time);

        coordinatesData.coordinatesData.Add(savedData);
        SaveCoordinateData();
    }

    public void SaveCoordinateData()
    {
        if (!Directory.Exists(Application.dataPath + characterDataProjectFilePath))
        {
            string dataAsJson = JsonHelper.ToJson(coordinatesData.coordinatesData); //convert to JSON-file
            string filePath = Application.dataPath + characterDataProjectFilePath; //path to JSON-file
            File.WriteAllText(filePath, dataAsJson);
        }

    }

    public void DeserializingData()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + characterDataProjectFilePath);
        deserializedCoordinatesData = JsonHelper.FromJson<Model>(dataAsJson);
        for (int i = 0; i < deserializedCoordinatesData.Count; i++)
        {
            //Debug.Log(deserializedCoordinatesData[i].Position[0]);
        }
        //Debug.Log(deserializedCoordinatesData.Count);
    }
}