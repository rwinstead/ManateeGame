using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TMP_Text text;

    public void OnPointerEnter (PointerEventData eventData)
    {
        text.color = new Color32(173, 230, 255, 255);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        text.color = Color.white;
    }

    public void ChangeTextWhite() // called when menu is closed to ensure it's white when opened again
    {
        text.color = Color.white;
    }


}
