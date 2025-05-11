using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public int chooseSkinId;
    public static SkinManager instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetSkinId(int id)
    {
        chooseSkinId = id;
    }
    public int GetSkinId()=> chooseSkinId;
}
