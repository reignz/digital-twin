using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewsPanelController : MonoBehaviour
{
    public GameObject panel;
    public GameObject ViewsButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel()
    {
        ViewsButton.SetActive(false);
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        ViewsButton.SetActive(true);
    }
}
