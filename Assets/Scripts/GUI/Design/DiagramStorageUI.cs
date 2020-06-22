using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DiagramStorageUI : MonoBehaviour {

    //下属的显示UI
    public GameObject diagramUI;
    public List<GameObject> diagramUIList;

    //读取创建新设计图的参数
    public InputField InputRaw;
    public InputField InputRol;
    //public InputField CheatCode;

    // Use this for initialization
    void Awake()
    {
        diagramUIList = new List<GameObject>();
        initDiagramUI();
        UpdateStorageUI();

        //注册ChangeDiagram事件
        EventCenter.AddListener(EventCode.ChangeDiagram, UpdateStorageUI);
    }

    private void OnDestroy()
    {
        //移除事件注册
        EventCenter.RemoveListener(EventCode.ChangeDiagram, UpdateStorageUI);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void initDiagramUI()
    {
        for (int i = 0; i < PlayerManager.instance.GetDiagramCount(); i++)
        {
            GameObject go = Instantiate<GameObject>(diagramUI, this.transform);
            diagramUIList.Add(go);
        }

        for (int i = 0; i < diagramUIList.Count; i++)
        {
            diagramUIList[i].GetComponent<DiagramShowUI>().DiagramIndex = i;
        }
    }

    private void expandStorageUI()
    {
        //数量增加
        for (int i = 0; i < (PlayerManager.instance.GetDiagramCount() - diagramUIList.Count); i++)
        {
            diagramUIList.Add(Instantiate(diagramUI, this.transform));
        }

        //重编码
        for (int i = 0; i < diagramUIList.Count; i++)
        {
            diagramUIList[i].GetComponent<DiagramShowUI>().DiagramIndex = i;
        }
    }

    public void UpdateStorageUI()
    {
        //Debug.Log(diagramUIList.Count);
        
        //调整UI数量
        if (diagramUIList.Count < PlayerManager.instance.GetDiagramCount())
        {
            expandStorageUI();
        }
        
        //调整UI显示
        foreach (GameObject go in diagramUIList)
        {
            go.GetComponent<DiagramShowUI>().UpdateSlots();
        }
    }

    public bool IsInt(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }

    public bool IsNumeric(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
    }

    //根据输入的参数创建新设计图
    public void CreateDiagram()
    {
        int raw, rol;
        float fillpercent;

        if (IsInt(InputRaw.text) && IsInt(InputRol.text))
        {
            raw = Convert.ToInt32(InputRaw.text);
            rol = Convert.ToInt32(InputRol.text);

            /*if (CheatCode.text == null)
            {
                PlayerManager.instance.AddDiagram(PlayerManager.instance.NewDiagram(raw, rol));
            }
            else if (CheatCode.text.Equals("Perfect"))
            {
                PlayerManager.instance.AddDiagram(PlayerManager.instance.NewDiagram(raw, rol, CheatCode.text));
            }
            else if (IsNumeric(CheatCode.text))
            {
                fillpercent = Convert.ToSingle(CheatCode.text);
                PlayerManager.instance.AddDiagram(PlayerManager.instance.NewDiagram(raw, rol, fillpercent));
            }
            else
            {
                PlayerManager.instance.AddDiagram(PlayerManager.instance.NewDiagram(raw, rol));
            }*/
            PlayerManager.instance.AddDiagram(PlayerManager.instance.NewDiagram(raw, rol));
            //return true;
        }
        else
        { //return false; 
        }
        UpdateStorageUI();
    }
}
