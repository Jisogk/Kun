using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler{

  public Sprite defaultSprite;
  public ModuleType moduleType;

  private Image img;
  private GameObject icon;

  public void OnDrag(PointerEventData eventData)
  {
    icon.transform.position = eventData.position;
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
    if(hit.collider != null)
    {
      GameObject target = hit.collider.gameObject;
      Image targetImg = target.GetComponent<Image>();
      if(targetImg != null && targetImg.enabled)
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
  }

  // Use this for initialization
  void Awake () {
    img = GetComponent<Image>();
	}

  
	
	// Update is called once per frame
	void Update () {
		
	}
}
