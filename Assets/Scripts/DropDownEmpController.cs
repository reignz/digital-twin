using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DropDownEmpController : DBMySQLUtils
{
    public GameObject dropDownEmpPanel;

    public TextMeshProUGUI Name;

    User user;
    int ID;

    protected override void Awake()
    {

    }

    protected override void OnApplicationQuit()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ReadUserByID(int.Parse(gameObject.tag)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ProcessUsers(List<User> usersList)
    {
        base.ProcessUsers(usersList);
    }

    public void OnMouseEnter()
    {
        dropDownEmpPanel.SetActive(true);
        //разместить в правильном положении

        ID = int.Parse(gameObject.tag);

        user = users.Where(x => x.ID == ID).First();

        Name.text = user.Surname + " " + user.Name.ToCharArray()[0] + "." + user.Patronymic.ToCharArray()[0] + ".";
    }

    public void OnMouseExit()
    {
        dropDownEmpPanel.SetActive(false);
    }
}
