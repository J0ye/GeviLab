using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MenuState { Main, Setting }

public class MenuManager : MonoBehaviour
{
    [Header("Menu Elements")]
    public GameObject main;
    public GameObject settings;

    public MenuState state = MenuState.Main;

    public virtual void Start()
    {
        UpdateUI();
    }

    public void SwitchMenuState()
    {
        _ = state == MenuState.Setting ? state = MenuState.Main : state = MenuState.Setting;
        UpdateUI();
    }

    protected void UpdateUI()
    {
        switch (state)
        {
            case MenuState.Main:
                main.SetActive(true);
                settings.SetActive(false);
                break;
            case MenuState.Setting:
                main.SetActive(false);
                settings.SetActive(true);
                break;
        }
    }
}

static class ExtensionMethods
{
    public static bool EqualTo(this Resolution a, Resolution b)
    {
        if (a.width == b.width && a.height == b.height)
        {
            return true;
        }

        return false;
    }
}
