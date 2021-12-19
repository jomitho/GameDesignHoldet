using UnityEngine;
using UnityEngine.UI;

/*
Det her script l�gges ovenp� alle knapper i spillet 
*/

public class Button : MonoBehaviour
{
    //inds�t prefab for Tower i det her felt (Fire/Ice)
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

    //placer sprite-ikonet for det p�g�ldene Tower her
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

    //inds�tter manuelt den pris vi �nsker i Unity
    [SerializeField]
    private int moneyCost;

    //laver knappens pris til proberty for at kunne tilg� den
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

    //bruger start for at s�tte den valgte pris p� knapperne
    private void Start()
    {
        moneyCostText.text = "<color=green>$</color>" + moneyCost;
    }
}