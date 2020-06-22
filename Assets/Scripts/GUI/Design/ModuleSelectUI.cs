using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Image))]
public class ModuleSelectUI : MonoBehaviour {

    public int Index;
    public Text InfoText;
    public Image Img;

    // Use this for initialization
    void Awake () {
        //img = GetComponent<Image>();
        UpdateUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateUI()
    {
        if (Index < PlayerManager.instance.GetModuleCount())
        {
            if (PlayerManager.instance.GetModule(Index).Icon == null)
            {
                Debug.Log("null");
            }

            Img.sprite = PlayerManager.instance.GetModule(Index).Icon;
            InfoText.text = ShowInfo(PlayerManager.instance.GetModule(Index));
        }
    }

    private string ShowInfo(Module module)
    {
        switch (module.type)
        {
            case ModuleType.Core:
                return "Core";
            case ModuleType.Energy:
                return "Energy";
            case ModuleType.Armor:
                return "Armor";
            case ModuleType.Computation:
                return "Computation";
            case ModuleType.Power:
                return "Power";
            case ModuleType.Weapon:
                return "Weapon";
            case ModuleType.Repair:
                return "Repair";
            case ModuleType.Battery:
                return "Battery:";
            default:
                return "TypeError";
        }
    }
}
