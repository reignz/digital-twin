using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DropDownReqController : DBMySQLUtils
{
    public GameObject dropDownReqPanel;

    public TextMeshProUGUI Name;

    Requirement requirement;
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
        //StartCoroutine(ReadRequirementById(int.Parse(gameObject.tag)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ProcessRequirements(List<Requirement> requirementsList)
    {
        base.ProcessRequirements(requirementsList);
    }

    public void OnMouseEnter()
    {
        dropDownReqPanel.SetActive(true);
        //разместить в правильном положении

        ID = int.Parse(gameObject.tag);

        requirement = requirements.Where(x => x.ID == ID).First();

        Name.text = requirement.Name + " " + requirement.SerialNumber;
    }

    public void OnMouseExit()
    {
        dropDownReqPanel.SetActive(false);
    }
}
