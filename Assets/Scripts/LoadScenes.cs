using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public void OnNotificationsButtonClick()
    {
        SceneManager.LoadScene("NotificationMenu");
    }

    public void OnBackToMainSceneButtonClick()
    {
        SceneManager.LoadScene("SampleScene111");
    }
}
