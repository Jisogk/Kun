using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleSelection : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler{

  private GameObject icon;

  public void OnBeginDrag(PointerEventData eventData)
  {
    icon = new GameObject("Icon", typeof(Image));
    Image iconImg = icon.GetComponent<Image>();
    iconImg.sprite = GetComponent<Image>().sprite;

    icon.transform.SetParent(GameObject.Find("Canvas").transform);
    icon.transform.SetAsLastSibling();

    SetDraggedPosition(eventData);
  }

  public void OnDrag(PointerEventData eventData)
  {
    SetDraggedPosition(eventData);
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    Destroy(icon);
    /*
    foreach(GameObject gobj in eventData.hovered)
    {
      Debug.Log(gobj.name);
    }
    Debug.Log(eventData.pointerDrag.name);
    if (eventData.pointerEnter.tag == "ModuleSlot")
    {
      
      Image slotImg = eventData.pointerEnter.GetComponent<Image>();
      if(slotImg.enabled)
      {
        slotImg.sprite = icon.GetComponent<Image>().sprite;
      }
    }*/
  }

  private void SetDraggedPosition(PointerEventData eventData)
  {
    icon.transform.position = eventData.position;
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
