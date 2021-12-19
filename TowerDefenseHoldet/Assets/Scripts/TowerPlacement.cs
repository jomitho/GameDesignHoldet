using UnityEngine;

/*
Bruger det her script til at vise hvorvidt et tower holdes eller ej
Classen arver fra singleton for netop at gøre den til en singleton
*/

public class TowerPlacement : Singleton<TowerPlacement>
   
{
    //laver reference til vores sprite-ikoner
    private SpriteRenderer spriteVisualization;
    
    void Start()
    {
        //skaber vores reference til sprite-ikonet i start-metoden
        this.spriteVisualization = GetComponent<SpriteRenderer>();
    }

    //metode for at vores sprite-ikon af et tower følger musens placering
    private void HoldTower()
    {
        if (spriteVisualization.enabled)
        {
            //oversætter musens placering til dens position i spillet
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //nulstiller vores z-værdi
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    //bruger update-metoden til HoldTower-metoden (så den hele tiden opdateres (every frame)
    void Update()
    {
        HoldTower();
    }

    //når der klikkes på et tower skal visualiseringen af vores sprite tower-ikon vises
    public void Clicked(Sprite sprite)
    {
        this.spriteVisualization.sprite = sprite;
        spriteVisualization.enabled = true;
    }
    
    //vores sprite tower-ikon skal forsvinde igen så snart vores tower er placeret
    public void Placed()
    {
       
            spriteVisualization.enabled = false;
            GameMaster.Instance.ButtonPressed = null;
        
    }
}