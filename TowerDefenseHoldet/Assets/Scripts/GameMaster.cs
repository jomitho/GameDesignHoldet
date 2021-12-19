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
        //StartCoroutine gør det muligt at pause vores eksekvering og fortsætte fra det samme punkt     
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

    //penge til at købe towers
    private int money;

    //laver reference til vores moneyText
    [SerializeField] 
    private Text moneyText;

    //laver propberty for at kunne tilgå vores critters
    public CritGenerator CritterObjects { get; set; }

    //property for pengene så vi kan tilgå dem
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            //sætter værdien
            this.money = value;
            //går ind og sætter et dollartegn foran denne værdi
            this.moneyText.text = "<color=green>$</color>" + value.ToString();
        }
    }

    //bruger Awake for at instantiate vores critters før spillet starter og skal bruge dem
    private void Awake()
    {
        CritterObjects = GetComponent<CritGenerator>();   
    }

    void Start()
    {
        //for at kunne teste sætter vi lige Money lidt højt så vi har råd til towers
        Money = 150;
    }

    //bruger update til hele tiden tjekke om der bliver trykket escape
    void Update()
    {
        EscapePressed();
    }

    //metode for at vælge
    public void SelectTower(Button buttonForTower)
    {
        //hvis der er råd til det pågældene tower
        if (money >= buttonForTower.MoneyCost)
        {
            this.ButtonPressed = buttonForTower;
            //acktiverer vores "skygge"-ikon for at vise at vi holder og er ved at placere et tower
            TowerPlacement.Instance.Clicked(buttonForTower.SpriteForTower);
        }
    }

    //metode for at købe, dvs. at købet sker så snart vores tower er placeret
    public void Purchase()
    {
        //hvis der er råd til det pågældene tower
        if (Money >= ButtonPressed.MoneyCost)
        {
            //så skal pengene trækkes
            Money -= ButtonPressed.MoneyCost;
            //og vores tower skal placeres
            TowerPlacement.Instance.Placed();
        }
    }

    //metode for når vi trykker på escape-knappen på tastaturet
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
        //først skal vi bruge critterens rute
        MapGenerator.Instance.CreateRoute();
        //skal bruges til at lave mellemrum (1,5 sekunder) imellem spawns hvis der skal komme flere af dem
        
        //laver variabel på det navn vi har kaldt vores Critter i prefabs-folderen
        string namePrefab = "Critter";
       
        //laver forespørgsel til at få vores Critter prefab fra vores critGenerator
        Critter critter = CritterObjects.GetCritter(namePrefab).GetComponent<Critter>();
        //finder Critterens startpunkt
    
        critter.FindStartPosition();
        yield return new WaitForSeconds(4.5f);
    }
}