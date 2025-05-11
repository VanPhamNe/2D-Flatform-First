using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credit : MonoBehaviour
{
    private UI_Fade fade;
    [SerializeField] private RectTransform recT;
    [SerializeField] private float speed = 200f;
    private float offScreen = 1800;
    private bool skip;
    private void Awake()
    {
        fade = GetComponentInChildren<UI_Fade>();
        fade.FadeEffect(0, 2f);
    }
    private void Update()
    {
        recT.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        if (recT.anchoredPosition.y > offScreen)
        {
           GotoMainMenu();
        }
    }
    public void SkipCredits() {
        if (skip == false)
        {
            speed *= 10;
            skip = true;
        }
        else { 
            GotoMainMenu();
        }
    }
    private void GotoMainMenu()
    {
        fade.FadeEffect(1, 1.5f, gotoMainMenuScene);
    }
    private void gotoMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
