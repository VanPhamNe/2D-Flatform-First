using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonDifficult : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI difficultInfo;
    [TextArea]
    [SerializeField] private string description;
    public void OnPointerEnter(PointerEventData eventData)
    {
        difficultInfo.text = description;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        difficultInfo.text = "";
    }
}
