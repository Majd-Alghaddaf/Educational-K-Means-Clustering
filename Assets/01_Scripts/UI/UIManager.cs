using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

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
