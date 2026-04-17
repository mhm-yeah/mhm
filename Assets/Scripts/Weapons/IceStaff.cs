using System.Collections.Generic;
using UnityEngine;

public class IceStaff : Weapon
{
    private ItemManager itemManager;

    public float slowDuration = 2f;
    public float slowPercentage = 0.5f;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        projectilePrefab = itemManager.iceStaffProjectilePrefab;
        id = 4;
        element = Utilities.Element.Ice;
        projectileXRotation = 0f;

        Init();
    }

    public override Dictionary<string, object> WeaponInfo()
    {
        throw new System.NotImplementedException();
    }
}
