using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    public int CurrentHearts;

    private void Awake()
    {
        CurrentHearts = maxHearts;
       
    }
    public void LoseHeart(int amount)
    {
       
        CurrentHearts = Mathf.Clamp(CurrentHearts - amount, 0, maxHearts);
        UI_Ingame.instance.UpdateHeathUI(CurrentHearts, maxHearts);

    }
    public bool IsEmpty() => CurrentHearts <= 0;
    public void ResetHearts()
    {
        CurrentHearts = maxHearts;
        UI_Ingame.instance.UpdateHeathUI(CurrentHearts, maxHearts);
    }

}
