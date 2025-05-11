using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private TextMeshProUGUI fruitsText;
    public string sceneName;
    private int levelIndex;
    public void LoadLevel() {
        AudioManager.instance.PlaySFX(4);
        int difficultIndex = ((int)DifficultManager.instance.currentDifficult) ;
        PlayerPrefs.SetInt("GameDifficult", difficultIndex);
        SceneManager.LoadScene(sceneName);
    }
    public void setupButton(int newLevelIndex)
    {
        levelIndex = newLevelIndex;
        levelNumberText.text = "Level "+levelIndex;
        sceneName = "Level" + levelIndex;
        fruitsText.text = FruitInfoText();
    }
    private string FruitInfoText() { 
        int totalFruitsOnLevel = PlayerPrefs.GetInt("Level" + levelIndex + "TotalFruits", 0);
        string totalFruitsText = totalFruitsOnLevel == 0 ? "?" : totalFruitsOnLevel.ToString();
        int fruitsCollect= PlayerPrefs.GetInt("Level" + levelIndex + "FruitsCollect");
        return "Fruits: " + fruitsCollect + "/" + totalFruitsText;
    }
}
