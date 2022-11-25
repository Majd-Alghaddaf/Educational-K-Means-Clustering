using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseOverEnabler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject targetGameObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetGameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetGameObject.SetActive(false);
    }
}
