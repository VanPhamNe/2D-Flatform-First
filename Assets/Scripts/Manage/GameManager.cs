using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Fruit Manager")]
    public bool fruitAreRandom;
    public int fruitCollect;
    public int totalFruits;
    public Transform fruitParent;



    [Header("Level Manager")]
    [SerializeField] private int currentLevelIndex;
    private int nextLevelIndex;

    [Header("Manager")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private DifficultManager difficultManager;
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
    }
    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;   
        nextLevelIndex = currentLevelIndex + 1;
        CollectFruits();
        CreateManagerIfNeed();
    }
    private void CreateManagerIfNeed()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
        if (PlayerManager.instance == null)
        {
            Instantiate(playerManager);
        }
        if(DifficultManager.instance == null)
        {
            Instantiate(difficultManager);
        }
    }
    private void CollectFruits()
    {
        Fruit[] allfruits = FindObjectsOfType<Fruit>();
        totalFruits = allfruits.Length;
        UI_Ingame.instance.UpdateFruitUI(fruitCollect, totalFruits);
       
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
    }
    [ContextMenu("ParentALLFruit")]
    private void ParentAllFruit()
    {
        if (fruitParent == null) return;
        Fruit[] allfruits = FindObjectsOfType<Fruit>();
        foreach (Fruit f in allfruits)
        {
            f.transform.parent = fruitParent;
        }

    }

    public void AddFruit()
    {
        fruitCollect++;
        UI_Ingame.instance.UpdateFruitUI(fruitCollect, totalFruits);
    }
    public bool FruitHaveRandomLook()
    {
        return fruitAreRandom;
    }
    public void RestartLevel()
    {
        UI_Fade fade = UI_Ingame.instance.fade;
        fade.FadeEffect(1f, 1.5f, LoadCurrentScene);
    }
    public void LoadCurrentScene() {
        //currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Level"+currentLevelIndex);
    }
    private void LoadTheEndScene() 
    {
        SceneManager.LoadScene("TheEnd");
    }
    public void LevelFinish()
    {
        SaveLevelProgess();
        SaveFruitInfo();
        LoadNextScene();
    }

    private void SaveLevelProgess()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);
        if (NoMoreLevel() == false)
        {
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
        }
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene("Level"+ nextLevelIndex);
    }
    
    private void LoadNextScene() {
        UI_Fade fade = UI_Ingame.instance.fade;
    
        if (NoMoreLevel())
        {
            fade.FadeEffect(1f, 1.5f, LoadTheEndScene);

        }
        else
        {
            fade.FadeEffect(1f, 1.5f, LoadNextLevel);
        }
    }
    private bool NoMoreLevel() {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2; //vi co main menu va the end nen -2;
        bool noMoreLevel = currentLevelIndex == lastLevelIndex;
        return noMoreLevel;
    }
    private void SaveFruitInfo() { 
        int fruitCollectBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollect");
        if (fruitCollectBefore < fruitCollect)
        {
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollect", fruitCollect);
        }
        int totalFruitsBank=PlayerPrefs.GetInt("TotalFruitsAmount");
        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsBank + fruitCollect);
    }
    public void ResetFruitsInLevel()
    {
        fruitCollect = 0;
        UI_Ingame.instance.UpdateFruitUI(fruitCollect, totalFruits);

        if (fruitParent == null) return;

        foreach (Transform fruit in fruitParent)
        {
            fruit.gameObject.SetActive(true);
        }
    }
}
