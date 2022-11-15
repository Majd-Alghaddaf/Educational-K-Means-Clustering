using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject menuContainer;

    public void ToggleMainMenu()
    {
        menuContainer.SetActive(!menuContainer.activeInHierarchy);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
