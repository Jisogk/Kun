using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ModuleSelectUI : MonoBehaviour {

    public int Index;

    private Image img;

    // Use this for initialization
    void Awake () {
        img = GetComponent<Image>();
        UpdateUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateUI()
    {
        if (Index < StorageManager.instance.GetModuleCount())
        {
            if (StorageManager.instance.GetModule(Index).Icon == null)
            {
                Debug.Log("null");
            }

            img.sprite = StorageManager.instance.GetModule(Index).Icon;
        }
    }
}
