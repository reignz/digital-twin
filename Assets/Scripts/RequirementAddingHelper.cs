using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RequirementAddingHelper : DBMySQLUtils
{
    #region Inspector Parameters
    public GameObject AddedBlock;
    public GameObject PostfixBlock;

    public TMP_InputField Name_Input;
    public TMP_InputField SerialNumber_Input;
    public TMP_InputField ImplDate_Input;
    public TMP_InputField Lifetime_Input;

    public TMP_InputField TParamInput1;
    public TMP_InputField TParamInput2;
    public TMP_InputField TParamInput3;
    public TMP_InputField TParamInput4;

    public TMP_InputField PParamInput1;
    public TMP_InputField PParamInput2;
    public TMP_InputField PParamInput3;
    public TMP_InputField PParamInput4;

    public TextMeshProUGUI AddedMessage;

    public GameObject inventoryPanel;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        AddedBlock.SetActive(true);
        PostfixBlock.SetActive(false);

        #region Adding Default Values
        TParamInput1.text = DefaultValue.TemperatureParameter1.ToString();
        TParamInput2.text = DefaultValue.TemperatureParameter2.ToString();
        TParamInput3.text = DefaultValue.TemperatureParameter3.ToString();
        TParamInput4.text = DefaultValue.TemperatureParameter4.ToString();

        PParamInput1.text = DefaultValue.PressureParameter1.ToString();
        PParamInput2.text = DefaultValue.PressureParameter2.ToString();
        PParamInput3.text = DefaultValue.PressureParameter3.ToString();
        PParamInput4.text = DefaultValue.PressureParameter4.ToString();
        #endregion
    }

    public void OnSaveButtonClick()
    {
        Requirement req = new Requirement();

        System.DateTime dateTime = System.DateTime.Parse(ImplDate_Input.text);
        int lifetime = int.Parse(Lifetime_Input.text.Split(' ')[0]);

        req.Name = Name_Input.text;
        req.SerialNumber = SerialNumber_Input.text;
        req.Lifetime = lifetime;
        req.ImplementationDate = dateTime;
        
        if(dateTime.AddYears(dateTime.Year + lifetime) > System.DateTime.Now)
        {
            req.Status = DefaultValue.RequirementWorksProperly;
        }
        else
        {
            req.Status = DefaultValue.RequirementOutdated;
        }

        req.PressureParameter1 = int.Parse(PParamInput1.text);
        req.PressureParameter2 = int.Parse(PParamInput2.text);
        req.PressureParameter3 = int.Parse(PParamInput3.text);
        req.PressureParameter4 = int.Parse(PParamInput4.text);

        req.TemperatureParameter1 = int.Parse(TParamInput1.text);
        req.TemperatureParameter2 = int.Parse(TParamInput2.text);
        req.TemperatureParameter3 = int.Parse(TParamInput3.text);
        req.TemperatureParameter4 = int.Parse(TParamInput4.text);

        StopTimerAndThread();
        bool isExecute = InsertRequirementWithParameters(req);
        ReadAllInfoThread();

        AddedBlock.SetActive(false);
        PostfixBlock.SetActive(true);
        if (isExecute)
        {
            AddedMessage.text = MyUIMessage.RequrementExecuteAddingMessage;
        }
        else
        {
            AddedMessage.text = MyUIMessage.RequrementNonExecuteAddingMessage;
        }
        

    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void PlaceReqButtonClick()
    {
        inventoryPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
