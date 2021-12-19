using UnityEngine;
using UnityEngine.UI;

/*
Det her script lægges ovenpå alle knapper i spillet 
*/

public class Button : MonoBehaviour
{
    //indsæt prefab for Tower i det her felt (Fire/Ice)
    [SerializeField] 
    private GameObject tower;

    //constructor for at returnerer det rigtige tower
    public GameObject Tower
    {
        get
        {
            return tower;
        }
    }

    //placer sprite-ikonet for det pågældene Tower her
    [SerializeField]
    private Sprite spriteForTower;

    //contructor for at returnerer den rigtige sprite
    public Sprite SpriteForTower
    {
        get
        {
            return spriteForTower;

        }
    }

    //indsætter manuelt den pris vi ønsker i Unity
    [SerializeField]
    private int moneyCost;

    //laver knappens pris til proberty for at kunne tilgå den
    public int MoneyCost
    {
        get
        {
            return moneyCost;
        }
    }

    //felt for tekst til prisen i Unity 
    [SerializeField]
    private Text moneyCostText;

    //bruger start for at sætte den valgte pris på knapperne
    private void Start()
    {
        moneyCostText.text = "<color=green>$</color>" + moneyCost;
    }
}