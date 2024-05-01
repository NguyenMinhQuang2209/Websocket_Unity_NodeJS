using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponConfig
{
    [SerializeField] private float timeBwtAttack = 1f;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float bulletDamage = 1f;
    [SerializeField] private Sprite bulletTexture;
    [SerializeField] private Sprite weaponTexture;
    [SerializeField] private string bulletURL;
    [SerializeField] private string weaponURL;
    [SerializeField] private float bulletDelayDieTime = 1f;
    [SerializeField] private string itemName;
    public WeaponConfig(float timeBwtAttack, float bulletSpeed, float bulletDamage, Sprite bulletTexture, Sprite weaponTexture, string bulletURL, string weaponURL, float bulletDelayDieTime, string itemName)
    {
        this.timeBwtAttack = timeBwtAttack;
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.bulletTexture = bulletTexture;
        this.weaponTexture = weaponTexture;
        this.bulletURL = bulletURL;
        this.weaponURL = weaponURL;
        this.bulletDelayDieTime = bulletDelayDieTime;
        this.itemName = itemName;
    }

    public float TimeBwtAttack { get { return timeBwtAttack; } }
    public float BulletSpeed { get { return bulletSpeed; } }
    public float BulletDamage { get { return bulletDamage; } }
    public Sprite WeaponTexture { get { return weaponTexture; } set { weaponTexture = value; } }
    public float BulletDelayDieTime { get { return bulletDelayDieTime; } }
    public Sprite BulletTexture { get { return bulletTexture; } set { bulletTexture = value; } }
    public string ItemName { get { return itemName; } }
    public string BulletURL { get { return bulletURL; } }
    public string WeaponURL { get { return weaponURL; } }
}
