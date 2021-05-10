using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject InfoText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoText.SetActive(true);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoText.SetActive(false);
    }

    private void OnDisable()
    {
        InfoText.SetActive(false);
    }
}
