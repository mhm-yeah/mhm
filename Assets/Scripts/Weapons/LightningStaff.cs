using System.Collections.Generic;
using UnityEngine;

public class LightningStaff : Weapon
{
    private ItemManager itemManager;

    public float stunDuration = 0.5f;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        projectilePrefab = itemManager.lightningStaffProjectilePrefab;
        id = 3;
        element = Utilities.Element.Lightning;
        projectileXRotation = 0f;

        Init();
    }

    public override Dictionary<string, object> WeaponInfo()
    {
        throw new System.NotImplementedException();
    }
}
