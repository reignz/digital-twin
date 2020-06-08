using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelController : DBMySQLUtils {

    Thread updateRequirement;

    public GameObject currentInfoBlock;
    public GameObject personalInfoBlock;
    public GameObject editInfoBlock;

    public GameObject panel;
    string text;
    public new Camera camera;
    Ray ray;
    RaycastHit hit;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI SerialNumber;
    public TextMeshProUGUI ImplementationDate;
    public TextMeshProUGUI Lifetime;
    public TextMeshProUGUI Status;

    public TMP_InputField Name_Input;
    public TMP_InputField SerialNumber_Input;
    public TMP_InputField ImplDate_Input;
    public TMP_InputField Lifetime_Input;

    public TextMeshProUGUI pressureInputField;
    public TextMeshProUGUI temperatureInputField;

    public GameObject characterPanel;

    Requirement requirement;
    int ID;

    bool isUpdate;

    float pressure;
    float temperature; 

    public bool panelActive { get; set; }
    public bool currentStateActive { get; set; }

    protected override void Awake()
    {

    }

    protected override void OnApplicationQuit()
    {

    }

    // Use this for initialization
    void Start () {
        currentInfoBlock.SetActive(false);
        editInfoBlock.SetActive(false);

        personalInfoBlock.SetActive(true);

        ID = int.Parse(gameObject.tag);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSensorsIndicator();

        if (updateRequirement != null)
        {
            if (!updateRequirement.IsAlive && isUpdate)
            {
                isUpdate = false;

                timer = null;
                ReadAllInfoThread();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log(Input.mousePosition);
            ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
                foreach (var hit in hits)
                {
                    if (hit.transform.name == gameObject.name)
                    {
                        if (characterPanel.activeInHierarchy == true)
                        {
                            characterPanel.SetActive(false);
                        }

                        panel.SetActive(true);
                        panelActive = true;

                        currentInfoBlock.SetActive(false);
                        personalInfoBlock.SetActive(true);
                        editInfoBlock.SetActive(false);

                        //ID = int.Parse(GameObject.Find(hit.transform.name).tag);

                        requirement = requirements.Where(x => x.ID == ID).First();

                        Name.text = requirement.Name;

                        SerialNumber.text = requirement.SerialNumber;

                        ImplementationDate.text = requirement.ImplementationDate.ToString("dd.MM.yyyy");

                        if (requirement.Lifetime == 1)
                        {
                            Lifetime.text = requirement.Lifetime.ToString() + " год";
                        }
                        else
                        {
                            if ((requirement.Lifetime >= 2) && (requirement.Lifetime <= 4))
                            {
                                Lifetime.text = requirement.Lifetime.ToString() + " года";
                            }
                            else
                            {
                                Lifetime.text = requirement.Lifetime.ToString() + " лет";
                            }
                        }

                        Status.text = requirement.Status;

                        haveChangeRequirements = false;
                    }
                }
            }
        }

        //if (panel.activeInHierarchy && haveChangeRequirements)
        //{
        //    if (requirements.Where(x => x.ID == ID).First() != null)
        //    {
        //        requirement = requirements.Where(x => x.ID == ID).First();
        //    }

        //    Name.text = requirement.Name;

        //    SerialNumber.text = requirement.SerialNumber;

        //    ImplementationDate.text = requirement.ImplementationDate.ToString("dd.MM.yyyy");

        //    if (requirement.Lifetime == 1)
        //    {
        //        Lifetime.text = requirement.Lifetime.ToString() + " год";
        //    }
        //    else
        //    {
        //        if ((requirement.Lifetime >= 2) && (requirement.Lifetime <= 4))
        //        {
        //            Lifetime.text = requirement.Lifetime.ToString() + " года";
        //        }
        //        else
        //        {
        //            Lifetime.text = requirement.Lifetime.ToString() + " лет";
        //        }
        //    }

        //    Status.text = requirement.Status;

        //    haveChangeRequirements = true;
        //}
    }

    protected override void ProcessRequirements(List<Requirement> requirementsList)
    {
        base.ProcessRequirements(requirementsList);

    }

    public bool GetPanelActivity()
    {
        return panelActive;
    }
    public bool GetCurrentBlockActivity()
    {
        return currentStateActive;
    }

    void ChangeSensorsIndicator()
    {
        if (panelActive && currentStateActive)
        {
            pressure = float.Parse(gameObject.GetComponent<Barometer>().GetMessageInfo().Value);
            temperature = float.Parse(gameObject.GetComponent<Temprature>().GetMessageInfo().Value);

            if (((int)pressure) != -0)
                pressureInputField.text = ((int)pressure).ToString() + "Па";
            else
                pressureInputField.text = "0" + "Па";

            if (((int)temperature) != -0)
                temperatureInputField.text = ((int)temperature).ToString() + "°С";
            else
                temperatureInputField.text = "0" + "°С";
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        panelActive = false;
    }

    public void ChangePersonalToCurrentState()
    {
        personalInfoBlock.SetActive(false);
        currentInfoBlock.SetActive(true);
        currentStateActive = true;
    }

    public void ChangeCurrentStateToPersonal()
    {
        currentInfoBlock.SetActive(false);
        personalInfoBlock.SetActive(true);
        currentStateActive = false;
    }

    public void EditRequirementInfo()
    {
        currentInfoBlock.SetActive(false);
        personalInfoBlock.SetActive(false);
        currentStateActive = false;

        editInfoBlock.SetActive(true);

        Name_Input.text = requirement.Name;
        SerialNumber_Input.text = requirement.SerialNumber;
        ImplDate_Input.text = requirement.ImplementationDate.ToString("dd/MM/yyyy");

        if (requirement.Lifetime == 1)
        {
            Lifetime_Input.text = requirement.Lifetime.ToString() + " год";
        }
        else
        {
            if ((requirement.Lifetime >= 2) && (requirement.Lifetime <= 4))
            {
                Lifetime_Input.text = requirement.Lifetime.ToString() + " года";
            }
            else
            {
                Lifetime_Input.text = requirement.Lifetime.ToString() + " лет";
            }
        }
    }

    public void SaveRequirementInfo()
    {
        editInfoBlock.SetActive(false);
        personalInfoBlock.SetActive(true);

        Requirement newRequirement = new Requirement();

        newRequirement.ID = requirement.ID;
        newRequirement.Name = Name_Input.text;
        newRequirement.SerialNumber = SerialNumber_Input.text;
        newRequirement.Lifetime = int.Parse(Lifetime_Input.text.Split(' ')[0]);
        newRequirement.ImplementationDate = System.DateTime.Parse(ImplDate_Input.text);
        newRequirement.Status = requirement.Status;

        if (requirement != newRequirement)
        {
            StopTimerAndThread();
            updateRequirement = new Thread(delegate ()
            {
                UpdateRequirement(newRequirement);
                isUpdate = true;
            });
            updateRequirement.Start();
        }
    }
}
