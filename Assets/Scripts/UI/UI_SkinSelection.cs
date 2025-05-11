using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public struct Skin
{
    public string name;
    public int skinPrice;
    public bool unlocked;
}
public class UI_SkinSelection : MonoBehaviour
{
    private UI_LevelSelect uiLevelSelect;
    private UI_Mainmenu uiMainmenu;
    [SerializeField] private int currentIndex;
    [SerializeField] private Animator display;
    [SerializeField] private int maxIndex;
    [Header("UI details")]
    [SerializeField] private Skin[] skins;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI bankText;
    [SerializeField] private TextMeshProUGUI buySelectText;
    private void Start()
    {
       LoadSkinUnlock();
        currentIndex = PlayerPrefs.GetInt("SelectedSkinIndex", 0);
        UpdateDisplay();
        uiMainmenu= GetComponentInParent<UI_Mainmenu>();
        uiLevelSelect = uiMainmenu.GetComponentInChildren<UI_LevelSelect>(true);


    }
    private void LoadSkinUnlock() {
        for (int i = 0; i < skins.Length; i++)
        {
            bool skinUnlocked = PlayerPrefs.GetInt(skins[i].name + "Unlocked", 0) == 1;
            if (skinUnlocked || i == 0)
            {
                skins[i].unlocked = true;
            }
        }
      
    }
    public void NextSkin() { 
        currentIndex++;
        if (currentIndex >= maxIndex) currentIndex = 0;
        PlayerPrefs.SetInt("SelectedSkinIndex", currentIndex);
        AudioManager.instance.PlaySFX(4);
        UpdateDisplay();
    }
    public void PrevSkin() { 
        currentIndex--;
        if (currentIndex <0)
        {
            currentIndex = maxIndex-1;
        }
        PlayerPrefs.SetInt("SelectedSkinIndex", currentIndex);
        AudioManager.instance.PlaySFX(4);
        UpdateDisplay();
    }
    private void UpdateDisplay() { 
        bankText.text = "Bank: " + FruitInBank();
        for (int i = 0; i < display.layerCount; i++)
        {
           display.SetLayerWeight(i, 0);
        }
        display.SetLayerWeight(currentIndex, 1);
        if (skins[currentIndex].unlocked)
        {
            priceText.transform.parent.gameObject.SetActive(false);
            buySelectText.text = "Select";
        }
        else {
            priceText.transform.parent.gameObject.SetActive(true);
            priceText.text ="Price: " + skins[currentIndex].skinPrice;
            buySelectText.text = "Buy";
        }
    }
    private int FruitInBank()
    {
        return PlayerPrefs.GetInt("TotalFruitsAmount");
    }
    public void SelectSkin()
    {
        if (skins[currentIndex].unlocked == false)
        {
            BuySkin(currentIndex);
        }
        else {
            SkinManager.instance.SetSkinId(currentIndex);
            uiMainmenu.SwitchUI(uiLevelSelect.gameObject);
        }
        AudioManager.instance.PlaySFX(4);
        UpdateDisplay();
    }
    private void BuySkin(int index)
    {
        if(HaveEnoughFruit(skins[index].skinPrice)==false)
        {
            AudioManager.instance.PlaySFX(6);
            Debug.Log("Not enough fruit");
            return;
        }
        AudioManager.instance.PlaySFX(10);
        string skinName = skins[index].name;
        skins[index].unlocked = true;
        PlayerPrefs.SetInt(skinName + "Unlocked", 1);

    }
    public void ResetSkinSelection()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedSkinIndex", 0);
        UpdateDisplay();
    }
    private bool HaveEnoughFruit(int price)
    {
        if (price <= FruitInBank())
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitInBank() - price);

            return true;
        }
        else
        {
            return false;
        }
    }
}
