using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonToDistribution : MonoBehaviour {

  public Text warningText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void Click()
  {
    if(ModuleInfo.instance.checkModules())
    {
      SceneManager.LoadScene("Distribution");
    }
    else
    {
      warningText.enabled = true;
    }
  }
}
