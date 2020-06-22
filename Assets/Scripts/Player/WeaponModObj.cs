using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModObj : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;

    public Player root;
    private ModuleObj moduleObj;
    private WeaponModule weapon;

    private float LastShootTime;

    // Use this for initialization
    void Awake()
    {
        LastShootTime = Time.time;
        root = GetComponentInParent<Player>();
        moduleObj = GetComponent<ModuleObj>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (root.IsOperating && PlayerManager.instance.GetCurrentPlan()[moduleObj.rawIndex, moduleObj.rolIndex]) //检查整个机甲是否正常运作，且本模块是否已供电
        {
            CheckShoot(weapon);
        }
    }

    public void GetWeapon(int raw, int rol)
    {
        if (PlayerManager.instance.ModuleMap[raw, rol].type.Equals(ModuleType.Weapon))
        {
            weapon = (WeaponModule)PlayerManager.instance.ModuleMap[raw, rol];
        }
    }

    void CheckShoot(WeaponModule wm)
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 bulletpos = this.GetComponent<BoxCollider2D>().bounds.center;
        mousepos.z = bulletpos.z = -1;
        Vector3 direction = (mousepos - bulletpos).normalized;

        Transform gunTransform = this.transform.GetChild(0);

        float gunAngle = gunTransform.eulerAngles.z;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        gunTransform.RotateAround(bulletpos, Vector3.back, gunAngle - angle);
        if (Input.GetMouseButton(0) && Time.time - LastShootTime >= wm.shootingSpeed)
        {
            GameObject bul = Instantiate(bullet, bulletpos, Quaternion.identity) as GameObject;
            bul.name = "playerbullet";
            Bullet bulscript = bul.GetComponent<Bullet>();
            bulscript.shooter = gameObject;
            bulscript.Damage = wm.singleDmg;
            Rigidbody2D bulrig = bul.GetComponent<Rigidbody2D>();
            bulrig.velocity = direction * bulletSpeed;
            LastShootTime = Time.time;
        }
    }
}
