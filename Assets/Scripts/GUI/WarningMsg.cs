using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningMsg : MonoBehaviour
{
    public Text WarningText;

    public delegate void TargetMethod();

    private TargetMethod tar;
    
    // Use this for initialization
    void Awake()
    {
        EventCenter.AddListener<string, TargetMethod>(EventCode.ShowWarningMsg, ShowWarningMsg);

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string, TargetMethod>(EventCode.ShowWarningMsg, ShowWarningMsg);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowWarningMsg(string text, TargetMethod targetMethod)
    {
        WarningText.text = text;
        if (!this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(true);
        }
        tar = targetMethod;
    }

    public void OnYesClick()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }

        if (tar != null)
        {
            tar();
        }
    }

    public void OnNoClick()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }

        if (tar != null)
        {
            tar = null;
        }
    }
}
