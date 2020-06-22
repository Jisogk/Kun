using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMsg : MonoBehaviour
{
    public Text text;
    //public float FadeTime;
    //private float countTime;

    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
        this.gameObject.SetActive(false);
        //注册ShowErrMsg事件
        EventCenter.AddListener<string>(EventCode.ShowErrMsg, ShowMsg);
        EventCenter.AddListener(EventCode.HideErrMsg, HideMsg);
    }

    private void OnDestroy()
    {
        //取消注册
        EventCenter.RemoveListener<string>(EventCode.ShowErrMsg, ShowMsg);
        EventCenter.RemoveListener(EventCode.HideErrMsg, HideMsg);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMsg(string msg)
    {
        text.text = msg;
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void HideMsg()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
