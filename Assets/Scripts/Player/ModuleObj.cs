using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleObj : MonoBehaviour
{
    //本格坐标
    public int rawIndex;
    public int rolIndex;

    public int totalHp;
    public int currentHp;

    public bool IsCore;

    // Use this for initialization
    void Awake()
    {
    }

    private void OnDestroy()
    {
        PlayerManager.instance.ModuleMap[rawIndex, rolIndex] = Module.sample(ModuleType.None);
        if (IsCore)
        {
            EventCenter.Broadcast(EventCode.OnCoreDestory);
        }
        else
        {
            EventCenter.Broadcast(EventCode.OnModuleDestory);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            currentHp = 0;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" && collision.name != "playerbullet")
        {
            currentHp -= collision.transform.GetComponent<Bullet>().Damage;
            EventCenter.Broadcast<ModuleObj>(EventCode.OnModuleDamage,this);
        }
    }

    public float CalHpPercent()
    {
        float hpPercent = (float)(currentHp) / (float)(totalHp);
        return hpPercent;
    }

    public void Init()
    {
        totalHp = PlayerManager.instance.ModuleMap[rawIndex, rolIndex].hp;
        currentHp = totalHp;
    }
}
