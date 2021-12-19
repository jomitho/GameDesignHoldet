using UnityEngine;
using UnityEngine.UI;

/*
Det her er et debug scipt som placeres på vores WhiteTile
Skal visualisere/vise om AStar algoritmen fungerer som den skal
*/

public class Visualizer : MonoBehaviour
{
    //felter for værdierne skal vises på hver enkelt felt
    [SerializeField]
    private Text f;
    [SerializeField]
    private Text g;
    [SerializeField]
    private Text h;

    //constructor for at returnerer værdien af F
    public Text F
    {
        get
        {
            f.gameObject.SetActive(true);
            return f;
        }
        set
        {
            this.f = value;
        }
    }

    //constructor for at returnerer værdien af H
    public Text H
    {
        get
        {
            h.gameObject.SetActive(true);
            return h;
        }
        set
        {
            this.h = value;
        }
    }

    //constructor for at returnerer værdien af G
    public Text G
    {
        get
        {
            g.gameObject.SetActive(true);
            return g;
        }
        set
        {
            this.g = value;
        }
    }
}