using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMainMenuClick : MonoBehaviour
{
    public GameObject addingPanel;
    private bool panelState;

    private void Awake()
    {
        addingPanel.SetActive(false);
        panelState = false;
    }

    public void OpenOrCloseAddingPanel()
    {
        addingPanel.SetActive(!panelState);
    }
}
