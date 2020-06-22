using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonToDistribution : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        if (!PlayerManager.instance.CheckModNum())
        {
            EventCenter.Broadcast<string>(EventCode.ShowErrMsg, "未装备模块");
            return;
        }
        if (!PlayerManager.instance.CheckCore())
        {
            return;
        }
        SceneManager.LoadScene("Distribution");
    }
}
