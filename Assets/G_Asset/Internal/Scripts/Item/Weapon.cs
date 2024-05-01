using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponConfig weaponConfig;
    [SerializeField] private SpriteRenderer weaponSpriteRender;
    private float currentTBWAttack = 0f;
    private void Start()
    {
        EquipmentWeapon(null);
    }

    public void EquipmentWeapon(WeaponConfig newConfig)
    {
        weaponConfig = newConfig;
        if (weaponConfig != null)
        {
            weaponSpriteRender.sprite = newConfig.WeaponTexture;
        }
        else
        {
            weaponSpriteRender.sprite = null;
        }
        currentTBWAttack = 0f;
    }
    private void Update()
    {
        if (weaponConfig != null)
        {
            currentTBWAttack += Time.deltaTime;
            if (currentTBWAttack >= weaponConfig.TimeBwtAttack)
            {
                Attack();
            }
        }
    }
    public void Attack()
    {

    }
}
