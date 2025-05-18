using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_Mainmenu : MonoBehaviour
{
    private UI_Fade fade;
    public string firstlevelname;
    [SerializeField] private GameObject[] uiElement;
    [SerializeField] private GameObject continueButton;
    private void Awake()
    {
        fade = GetComponentInChildren<UI_Fade>();
    }
    private void Start()
    {
        Time.timeScale = 1f;
        if (HasLevelProgress())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
        fade.FadeEffect(0, 1.5f);
    }
    public void Newgame() { 
        fade.FadeEffect(1, 1.5f, LoadLevelScene);
        AudioManager.instance.PlaySFX(4);
    }
    public void SwitchUI(GameObject uiToEnable)
    {
        foreach(GameObject ui in uiElement)
        {
            ui.SetActive(false);
        }
        uiToEnable.SetActive(true);
        UI_SkinSelection skinSelector = uiToEnable.GetComponent<UI_SkinSelection>();
        if (skinSelector != null)
        {
            skinSelector.ResetSkinSelection();
        }
        AudioManager.instance.PlaySFX(4);
    }
    private void LoadLevelScene()
    {
        SceneManager.LoadScene(firstlevelname);
    }
    private bool HasLevelProgress()
    {
        bool hasProgress = PlayerPrefs.GetInt("ContinueLevelNumber", 0)>0;
        return hasProgress;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;
        int difficultIndex = PlayerPrefs.GetInt("GameDifficult", 0);
        DifficultManager.instance.LoadDifficult(difficultIndex);
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("ContinueLevelNumber", 0));
        AudioManager.instance.PlaySFX(4);
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
