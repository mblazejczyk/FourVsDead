using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnBadgeHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prompt;
    public void OnPointerEnter(PointerEventData eventData)
    {
        prompt.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        prompt.SetActive(false);
    }
}
