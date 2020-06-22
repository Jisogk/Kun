using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DistributionPageButton : MonoBehaviour
{

    public Text planNumberText;
    public GameObject distributionPanel;
    public bool IsDelButton;

    // Use this for initialization
    void Awake()
    {
        if (IsDelButton)
        {
            gameObject.SetActive(false);
        }

        EventCenter.AddListener(EventCode.SwitchModule, SetActive);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventCode.SwitchModule, SetActive);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewPlanClick()
    {
        PlayerManager.instance.NewPlan();
    }

    public void DeletePlanClick()
    {
        PlayerManager.instance.DeletePlan(PlayerManager.instance.currentPlanIndex);
    }

    public void PrevPlanClick()
    {
        PlayerManager.instance.SwitchToPrevPlan();
    }

    public void NextPlanClick()
    {
        PlayerManager.instance.SwitchToNextPlan();
    }

    public void SetActive()
    {
        if (IsDelButton)
        {
            if (this.gameObject.activeInHierarchy)
            {
                if (PlayerManager.instance.GetPlanCount() <= 1)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if (PlayerManager.instance.GetPlanCount() > 1)
                {
                    this.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ReturnClick()
    {
        PlayerManager.instance.SwitchToFristPlan();
        GameManager.instance.NextLevel("Design");
    }
}
