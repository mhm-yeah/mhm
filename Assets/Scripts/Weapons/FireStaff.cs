using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireStaff : Weapon
{
    private ItemManager itemManager;

    public float burnDamagePerSecond = 5f;
    public float burnDuration = 3f;


    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        projectilePrefab = itemManager.fireStaffProjectilePrefab;
        id = 2;
        element = Utilities.Element.Fire;
        projectileXRotation = 180f;

        Init();
    }

    public override Dictionary<string, object> WeaponInfo()
    {
        throw new System.NotImplementedException();
    }
}
