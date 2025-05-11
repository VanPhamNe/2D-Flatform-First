using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Difficult
{
    Easy,
    Normal,
    Hard,
}
public class DifficultManager : MonoBehaviour
{
   public static DifficultManager instance;
    public Difficult currentDifficult;  
    private void Awake()
    {
       
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetDiffcult(Difficult newdifficult)
    {
        currentDifficult = newdifficult;
    }
    public void LoadDifficult(int difficultIndex) {
        currentDifficult = (Difficult)difficultIndex;

    }


}
