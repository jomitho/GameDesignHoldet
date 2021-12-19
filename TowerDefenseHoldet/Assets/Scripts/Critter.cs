using System.Collections.Generic;
using UnityEngine;

/*
Det her script skal lægges ovenpå vores Critter i prefabs-folderen
*/

public class Critter : MonoBehaviour
{
    //indstil hvor hurtigt vores critters skal bevæge sig på Inspectoren til vores Critter-prefab 
   
    public float movementRate;
   
    //vi skal bruge stacken med nodes som indeholder den vej vores critters skal gå på 
    private Stack<Node> critterRoad;

    //metode som skal give den vej som critters skal gå i spillet
    //kalder denne nye vej for newRoad
    private void GetRoad(Stack<Node> newRoad)
    {
     
        //først skal der lige være en vej
        if (newRoad != null)
        {
            //sætter den nye vej som den critterens nuværende vej
            this.critterRoad = newRoad;
            //sætter den nye position på banen
            MapPosition = critterRoad.Peek().GrafPosition;
            //sætter det nye target
            target = critterRoad.Pop().MapPosition;
        }
    }

    //metode for at få critters til at bevæge sig
    public void CritterWalk()
    {
      
        //bevæger critteren imod (MoveTowards) det næste target
        transform.position = Vector2.MoveTowards(transform.position, target, movementRate * Time.deltaTime);

        //hvis det er Vector2 kan den ikke sammenligne position
        //tjekker om critterens position er den samme som målet (target)
        if (transform.position == target)
        {
            //hvis vi har en vej og vi har brug for flere nodes så skal critters blive ved med at bevæge sig 
            if (critterRoad != null && critterRoad.Count > 0)
            {
                //sætter den nye position på banen
                MapPosition = critterRoad.Peek().GrafPosition;
                //sætter det næste/nye target
                target = critterRoad.Pop().MapPosition;
            }
        }
    }

    //for at kunne få critterens position på banen
    public GraphPoint MapPosition { get; set; }

    //critterens næste mål (target)
    private Vector3 target;

    //bruger Update til hele tiden (eller every frame) at kalde på Move 
    private void Update()
    {
        CritterWalk();
        
    }

    //metode for at critters kan finde banens startpunkt
    public void FindStartPosition()
    {
             
        transform.position = MapGenerator.Instance.StartTileObject.transform.position;

        GetRoad(MapGenerator.Instance.Route);
    }

    //metoden eksekveres hver gang et "hit" bliver registreret i spillet
    //husk give objekterne Box Collider 2D og Rigid Body 2D 
    public void OnTriggerEnter2D (Collider2D hit)
    {     
        //hvis vores critters rammer slutpunktet på banen
        if(hit.tag == "EndTile")
        {
            //så skal de fjernes
            Destroy(gameObject);    
        }
    } 
}