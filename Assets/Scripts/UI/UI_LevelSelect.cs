using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_LevelSelect : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrelab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private bool[] levelIsUnclocked;
    private void Start()
    {
        LoadLevelInfo();
        CreateLevelButton();
    }

    private void CreateLevelButton() { 
        int levelamount=SceneManager.sceneCountInBuildSettings-1;
        for (int i = 1; i < levelamount; i++)
        {
            if (!IsLevelUnlocked(i))
            {
                return;
            }
            UI_LevelButton button = Instantiate(buttonPrelab, buttonParent);
            button.setupButton(i);
        }
    }
    private bool IsLevelUnlocked(int levelIndex) => levelIsUnclocked[levelIndex];
    private void LoadLevelInfo()
    {
        int levelamount = SceneManager.sceneCountInBuildSettings - 1;
        levelIsUnclocked = new bool[levelamount];
        for (int i = 0; i < levelamount; i++)
        {
            bool levelUnclocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;
            if(levelUnclocked)
            {
                levelIsUnclocked[i] = true;
            }
           
        }
        levelIsUnclocked[1] = true;
    }

}
