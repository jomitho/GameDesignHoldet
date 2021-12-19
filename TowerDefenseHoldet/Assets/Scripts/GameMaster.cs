using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
Det her script skal tage sig af spillets mekanismer;
towers
penge
spawning af critters 
*/

public class GameMaster : Singleton<GameMaster>
{
   
    float move = 1.0f;
    int critCounter = 1;
    //metode for at spawne critters...
    public void SpawnCritter()
    {         
        //StartCoroutine g�r det muligt at pause vores eksekvering og forts�tte fra det samme punkt     
        int i = 0;
        while(i < critCounter)
        {
          StartCoroutine(ISpawnCritter());    
            i++;
        }
        if(critCounter == 1)
        {
            Debug.Log("Generated: " + critCounter + " critter");
        }
        else { 
        Debug.Log("Generated: " + critCounter + " critterz");
        }
        critCounter++;
        
        /*
        //det her til stress-testing
        int i = 0;
        while(i< 1000) { 
        StartCoroutine(ISpawnCritter());
            i++;
        }
        */
    }

    //laver property for knapper til vores towers
    public Button ButtonPressed { get; set; }

    //penge til at k�be towers
    private int money;

    //laver reference til vores moneyText
    [SerializeField] 
    private Text moneyText;

    //laver propberty for at kunne tilg� vores critters
    public CritGenerator CritterObjects { get; set; }

    //property for pengene s� vi kan tilg� dem
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            //s�tter v�rdien
            this.money = value;
            //g�r ind og s�tter et dollartegn foran denne v�rdi
            this.moneyText.text = "<color=green>$</color>" + value.ToString();
        }
    }

    //bruger Awake for at instantiate vores critters f�r spillet starter og skal bruge dem
    private void Awake()
    {
        CritterObjects = GetComponent<CritGenerator>();   
    }

    void Start()
    {
        //for at kunne teste s�tter vi lige Money lidt h�jt s� vi har r�d til towers
        Money = 150;
    }

    //bruger update til hele tiden tjekke om der bliver trykket escape
    void Update()
    {
        EscapePressed();
    }

    //metode for at v�lge
    public void SelectTower(Button buttonForTower)
    {
        //hvis der er r�d til det p�g�ldene tower
        if (money >= buttonForTower.MoneyCost)
        {
            this.ButtonPressed = buttonForTower;
            //acktiverer vores "skygge"-ikon for at vise at vi holder og er ved at placere et tower
            TowerPlacement.Instance.Clicked(buttonForTower.SpriteForTower);
        }
    }

    //metode for at k�be, dvs. at k�bet sker s� snart vores tower er placeret
    public void Purchase()
    {
        //hvis der er r�d til det p�g�ldene tower
        if (Money >= ButtonPressed.MoneyCost)
        {
            //s� skal pengene tr�kkes
            Money -= ButtonPressed.MoneyCost;
            //og vores tower skal placeres
            TowerPlacement.Instance.Placed();
        }
    }

    //metode for n�r vi trykker p� escape-knappen p� tastaturet
    private void EscapePressed()

    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerPlacement.Instance.Placed();
        }
    }

    //bruger IEnumerator-typen for at kunne lave pauser imellem vores critter-spawning 
    public IEnumerator ISpawnCritter()
    {
        //f�rst skal vi bruge critterens rute
        MapGenerator.Instance.CreateRoute();
        //skal bruges til at lave mellemrum (1,5 sekunder) imellem spawns hvis der skal komme flere af dem
        
        //laver variabel p� det navn vi har kaldt vores Critter i prefabs-folderen
        string namePrefab = "Critter";
       
        //laver foresp�rgsel til at f� vores Critter prefab fra vores critGenerator
        Critter critter = CritterObjects.GetCritter(namePrefab).GetComponent<Critter>();
        //finder Critterens startpunkt
    
        critter.FindStartPosition();
        yield return new WaitForSeconds(4.5f);
    }
}