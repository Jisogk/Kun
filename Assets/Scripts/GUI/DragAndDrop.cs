using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Sprite defaultSprite;
    public ModuleType moduleType;

    //private Image img;
    //private GameObject icon;

    /*public void OnDrag(PointerEventData eventData)
    {
        //icon.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (!img.enabled) return;
        icon = new GameObject("Icon", typeof(Image));
        icon.GetComponent<Image>().sprite = img.sprite;
        icon.transform.SetParent(GameObject.Find("Canvas").transform);
        icon.transform.SetAsLastSibling();
        icon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(icon);

        RaycastHit2D hit = Physics2D.Raycast(eventData.position, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject target = hit.collider.gameObject;
            Image targetImg = target.GetComponent<Image>();
            if (targetImg != null && targetImg.enabled)
            {
                if (target.tag == "ModuleSlot")
                {
                    targetImg.sprite = icon.GetComponent<Image>().sprite;
                    target.GetComponent<DragAndDrop>().moduleType = moduleType;
                }
            }
        }

        if (tag == "ModuleSlot")
        {
            img.sprite = defaultSprite;
            moduleType = ModuleType.None;
        }
    }*/
    public static Action<PointerEventData> OnLeftDrag;

    public static Action<Transform> OnEnter;

    public static Action OnExit;

    public static Action<Transform> OnLeftBeginDrag;

    public static Action<Transform, Transform> OnLeftEndDrag;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.tag == "Grid")
        {
            if (OnEnter != null)
            {
                OnEnter(this.transform);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExit != null)
        {
            OnExit();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftBeginDrag != null)
            {
                OnLeftBeginDrag(transform);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnLeftDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftBeginDrag != null)
            {
                if (eventData.pointerEnter == null)
                {
                    OnLeftEndDrag(transform, null);
                }
                else
                {
                    OnLeftEndDrag(transform, eventData.pointerEnter.transform);
                }
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        //img = GetComponent<Image>();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
