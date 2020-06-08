using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPanelController : DBMySQLUtils
{
    Thread updateUser;

    public GameObject personalInfoBlock;
    public GameObject currentInfoBlock;
    public GameObject editInfoBlock;

    public GameObject panel;
    string text;
    public new Camera camera;
    Ray ray;
    RaycastHit hit;

    public TextMeshProUGUI FIO;
    public TextMeshProUGUI Sex;
    public TextMeshProUGUI Birthday;
    public TextMeshProUGUI Position;
    public TextMeshProUGUI Status;

    public TMP_InputField FIO_Input;
    public TMP_InputField Sex_Input;
    public TMP_InputField Birthday_Input;
    public TMP_InputField Position_Input;
    public TMP_InputField Experience_Input;

    public GameObject requirementPanel;

    User user;
    int ID;

    bool isUpdate;

    protected override void Awake()
    {

    }

    protected override void OnApplicationQuit()
    {

    }

    // Use this for initialization
    void Start()
    {
        currentInfoBlock.SetActive(false);
        editInfoBlock.SetActive(false);

        personalInfoBlock.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateUser != null)
        {
            if (!updateUser.IsAlive && isUpdate)
            {
                isUpdate = false;

                timer = null;
                ReadAllInfoThread();
            }
        }

        //Debug.Log(thread0.ThreadState + " - thread0");
        //if (updateUser != null)
        //{
        //    Debug.Log(updateUser.ThreadState + " - updateUser");
        //}

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
                foreach (var hit in hits)
                {
                    if (hit.transform.name == gameObject.name)
                    {
                        if (requirementPanel.activeInHierarchy == true)
                        {
                            requirementPanel.SetActive(false);
                        }

                        panel.SetActive(true);

                        currentInfoBlock.SetActive(false);
                        personalInfoBlock.SetActive(true);
                        editInfoBlock.SetActive(false);

                        ID = int.Parse(GameObject.Find(hit.transform.name).tag);
                        
                        user = users.Where(x => x.ID == ID).First();

                        FIO.text = user.Surname + " " + user.Name.ToCharArray()[0] + "." + user.Patronymic.ToCharArray()[0] + ".";

                        if (user.Sex == true)
                        {
                            Sex.text = "мужской";
                        }
                        else
                        {
                            Sex.text = "женский";
                        }

                        Birthday.text = user.Birthday.ToString("dd/MM/yyyy");

                        Position.text = user.Position;

                        Status.text = user.Status;

                        haveChangeUsers = false;
                    }
                }
            }
        }

        if (panel.activeInHierarchy && haveChangeUsers)
        {
            if (users != null)
            {
                user = users.Where(x => x.ID == ID).First();

                FIO.text = user.Surname + " " + user.Name.ToCharArray()[0] + "." + user.Patronymic.ToCharArray()[0] + ".";

                if (user.Sex == true)
                {
                    Sex.text = "мужской";
                }
                else
                {
                    Sex.text = "женский";
                }

                Birthday.text = user.Birthday.ToString("dd/MM/yyyy");

                Position.text = user.Position;

                Status.text = user.Status;

                haveChangeUsers = false;
            }
        }
    }

    protected override void ProcessUsers(List<User> usersList)
    {
        base.ProcessUsers(usersList);

    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void ChangePersonalToCurrentState()
    {
        personalInfoBlock.SetActive(false);
        currentInfoBlock.SetActive(true);
    }

    public void ChangeCurrentStateToPersonal()
    {
        currentInfoBlock.SetActive(false);
        personalInfoBlock.SetActive(true);
    }

    public void EditEmployeeInfo()
    {
        currentInfoBlock.SetActive(false);
        personalInfoBlock.SetActive(false);

        editInfoBlock.SetActive(true);

        FIO_Input.text = user.Surname + " " + user.Name + " " + user.Patronymic;

        if (user.Sex == true)
        {
            Sex_Input.text = "мужской";
        }
        else
        {
            Sex_Input.text = "женский";
        }

        Birthday_Input.text = user.Birthday.ToString("dd/MM/yyyy");
        Position_Input.text = user.Position;
        Experience_Input.text = "7 лет";
    }

    public void SaveEmployeeInfo()
    {
        editInfoBlock.SetActive(false);
        personalInfoBlock.SetActive(true);

        User newUser = new User();

        newUser.ID = user.ID;
        newUser.Surname = FIO_Input.text.Split(' ')[0];
        newUser.Name = FIO_Input.text.Split(' ')[1];
        newUser.Patronymic = FIO_Input.text.Split(' ')[2];

        if (Sex_Input.text == "мужской")
            newUser.Sex = true;
        else
            newUser.Sex = false;

        newUser.Birthday = System.DateTime.Parse(Birthday_Input.text);
        newUser.Position = Position_Input.text;
        newUser.Status = user.Status;
        newUser.Email = user.Email;
        newUser.Password = user.Password;

        if (user != newUser)
        {
            StopTimerAndThread();
            updateUser = new Thread(delegate ()
            {
                UpdateUser(newUser);
                isUpdate = true;
            });
            updateUser.Start();
        }
    }
}
