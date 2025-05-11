using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_Ingame : MonoBehaviour
{
    public static UI_Ingame instance;
    public UI_Fade fade { get; private set; } //chi doc ko thay doi dc gia tri
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private TextMeshProUGUI heartText;
    private bool isPause;
    [SerializeField] private GameObject pauseUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        fade = GetComponentInChildren<UI_Fade>();


    }
    private void Start()
    {
        fade.FadeEffect(0, 1f);
    }
    public void UpdateFruitUI(int collectFruit,int totalFruits)
    {
        fruitText.text = collectFruit + "/" + totalFruits;
    }
    public void UpdateHeathUI(int currentHeart,int totalHeart)
    {
       heartText.text = currentHeart + "/" + totalHeart;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }
    }

    public void PauseButton() { 
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        else
        {
            isPause = true;
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }

    }
    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

}
