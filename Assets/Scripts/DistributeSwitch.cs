using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DistributeSwitch : MonoBehaviour, IPointerClickHandler {

  [HideInInspector]
  public bool isOpen;
  [HideInInspector]
  public int position;
  private Color origColor;
  private Color darkColor;

  // Use this for initialization
  void Awake () {
    isOpen = true;
    origColor = GetComponent<Image>().color;
    float h, s, v;
    Color.RGBToHSV(origColor, out h, out s, out v);
    v = v / 2.0f;
    darkColor = Color.HSVToRGB(h, s, v);
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void Click()
  {

  }

  public void OnPointerClick(PointerEventData eventData)
  {
    ModuleType modType = ModuleInfo.instance.moduleList[position].type;
    if (modType == ModuleType.Weapon || modType == ModuleType.Power)
    {

      if (isOpen)
      {
        GetComponent<Image>().color = darkColor;
      }
      else
        GetComponent<Image>().color = origColor;
      isOpen = !isOpen;
      transform.parent.gameObject.GetComponent<DistributionPanel>().UpdateStatusText();
    }
  }
}
