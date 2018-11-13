using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleSlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

  Image img;

  public void OnPointerDown(PointerEventData eventData)
  {
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    //Debug.Log(eventData.pointerDrag.name);
    Debug.Log("OPU");
    if(img.enabled && eventData.pointerDrag.tag == "ModuleSelection")
    {
      Image moduleImg = eventData.pointerDrag.GetComponent<Image>();
      img.sprite = moduleImg.sprite;
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
