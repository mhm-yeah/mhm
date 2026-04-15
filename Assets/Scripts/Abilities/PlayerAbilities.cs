using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private ArrowVolley arrowVolley;
    private PlayerFireball fireball;
    private Thornmail thornmail;
    private LightningChain lightning;
    private IceBlast iceBlast;
    void Awake()
    {
        fireball = GetComponent<PlayerFireball>();
        arrowVolley = GetComponent<ArrowVolley>();
        thornmail = GetComponent<Thornmail>();
        lightning = GetComponent<LightningChain>();
        iceBlast = GetComponent<IceBlast>();
    }

    public void ApplyAbility(AbilityID id)
    {
        switch (id)
        {
            case AbilityID.ArrowVolley:
                arrowVolley.Activate();
                break;
            case AbilityID.Fireball:
                fireball.Activate();
                break;
            case AbilityID.Thornmail:
                //thornmail.Activate();
                break;
            case AbilityID.LightningChain:
                lightning.Activate();
                break;
            case AbilityID.IceBlast:
                iceBlast.Activate();
                break;
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
    public string name;
    public string description;
    public AbilityID abilityID;
}