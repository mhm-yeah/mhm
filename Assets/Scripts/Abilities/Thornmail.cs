using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Thornmail : MonoBehaviour
{

    public bool hasThornmail = false;
    public float thornmailDamage = 10f;
    public GameObject thornmailPrefab;
    public void EquipThornmail()
    {
            hasThornmail = true;
            thornmailPrefab.SetActive(true);
    }
}
