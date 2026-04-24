using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private Dictionary<AbilityID, Ability> abilities;

    void Awake()
    {
        abilities = new Dictionary<AbilityID, Ability>();

        foreach (var ability in GetComponents<Ability>())
        {
            abilities.Add(ability.ID, ability);
        }
    }

    public void ApplyAbility(AbilityID id)
    {
        if (abilities.TryGetValue(id, out var ability))
        {
            ability.Activate();
        }
    }
}
public enum AbilityID
{
    Fireball,
    LightningChain,
    ArrowVolley,
    Thornmail,
    IceBlast
}
[System.Serializable]
public class CardData
{
    public AbilityID abilityID;
    public int level;
    public string name;
    public string description;
}