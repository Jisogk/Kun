using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleStorageUI : MonoBehaviour {

    public GameObject moduleUI;

    private List<GameObject> moduleUIList;

	// Use this for initialization
	void Awake () {
        moduleUIList = new List<GameObject>();
        initStorageUI();
        UpdateStorageUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void initStorageUI()
    {
        for(int i=0;i<9;i++)
        {
            GameObject go = Instantiate<GameObject>(moduleUI, this.transform);
            moduleUIList.Add(go);
        }

        for (int i = 0; i < 9; i++)
        {
            moduleUIList[i].GetComponent<ModuleSelectUI>().Index = i;
        }
    }

    private void expandStorageUI()
    {
        //数量加倍
        for (int i = 0; i < moduleUIList.Count; i++)
        {
            moduleUIList.Add(Instantiate(moduleUI, this.transform));
        }

        //重编码
        for (int i = 0; i < moduleUIList.Count; i++)
        {
            moduleUIList[i].GetComponent<ModuleSelectUI>().Index = i;
        }
    }

    public void UpdateStorageUI()
    {
        //调整UI数量
        if (moduleUIList.Count < PlayerManager.instance.GetModuleCount())
        {
            expandStorageUI();
        }

        //调整UI显示
        foreach (GameObject go in moduleUIList)
        {
            go.GetComponent<ModuleSelectUI>().UpdateUI();
        }

        //调整UI激活情况
        for (int i=0;i< moduleUIList.Count; i++)
        {
            if (i < PlayerManager.instance.GetModuleCount())
            {
                moduleUIList[i].SetActive(true);
            }
            else
            {
                moduleUIList[i].SetActive(false);
            }
        }
    }
}
