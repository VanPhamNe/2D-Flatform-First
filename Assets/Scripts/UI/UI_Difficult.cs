using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Difficult : MonoBehaviour
{
    public void SetDifficultEasy()
    {
        DifficultManager.instance.SetDiffcult(Difficult.Easy);
    }
    public void SetDifficultNormal()
    {
        DifficultManager.instance.SetDiffcult(Difficult.Normal);
    }
    public void SetDifficultHard()
    {
        DifficultManager.instance.SetDiffcult(Difficult.Hard);
    }
}
