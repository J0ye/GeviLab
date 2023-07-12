using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MenuState { Main, Setting }

public class MenuManager : MonoBehaviour
{
    [Header("Menu Elements")]
    public GameObject main;
    public GameObject settings;

    public List<Button> buttons = new List<Button>();

    public MenuState state = MenuState.Main;

    [Header("XR Menu Settings")]
    public Transform targetAnchor;
    public Vector3 xrScale = Vector3.zero;

    protected RectTransform rect;
    protected Canvas canvas;
    protected Vector3 startScale = Vector3.zero;

    public virtual void Start()
    {
        startScale = transform.localScale;
        canvas = GetComponent<Canvas>();
        rect = GetComponent<RectTransform>();
        UpdateUI();
    }

    public void SwitchMenuState()
    {
        _ = state == MenuState.Setting ? state = MenuState.Main : state = MenuState.Setting;
        UpdateUI();
    }

    public void ToggleFullScreen(bool screen)
    {
        Screen.fullScreen = screen;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    public void ConvertToDesktopUI()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        transform.parent = null;
        transform.localScale = startScale;

    }

    public void ConvertToXRUI()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        transform.parent = targetAnchor;
        transform.localScale = xrScale;
        rect.anchoredPosition3D = Vector3.zero;
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
