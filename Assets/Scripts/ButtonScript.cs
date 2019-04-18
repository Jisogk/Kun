using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

  public string nextLevelName;

	// Use this for initialization
	void Start () {
    Button btn = GetComponent<Button>();
    if(btn != null)
    {
      btn.onClick.AddListener(OnClick);
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnClick()
  {
    GameManager.instance.NextLevel(nextLevelName);
    if(nextLevelName == "Maingame")
    {
      GameManager.instance.posY += 1;
    }
  }
}
