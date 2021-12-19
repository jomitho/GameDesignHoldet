using System.Collections.Generic;
using UnityEngine;

/*
Det her script skal l�gges ovenp� vores Critter i prefabs-folderen
*/

public class Critter : MonoBehaviour
{
    //indstil hvor hurtigt vores critters skal bev�ge sig p� Inspectoren til vores Critter-prefab 
   
    public float movementRate;
   
    //vi skal bruge stacken med nodes som indeholder den vej vores critters skal g� p� 
    private Stack<Node> critterRoad;

    //metode som skal give den vej som critters skal g� i spillet
    //kalder denne nye vej for newRoad
    private void GetRoad(Stack<Node> newRoad)
    {
     
        //f�rst skal der lige v�re en vej
        if (newRoad != null)
        {
            //s�tter den nye vej som den critterens nuv�rende vej
            this.critterRoad = newRoad;
            //s�tter den nye position p� banen
            MapPosition = critterRoad.Peek().GrafPosition;
            //s�tter det nye target
            target = critterRoad.Pop().MapPosition;
        }
    }

    //metode for at f� critters til at bev�ge sig
    public void CritterWalk()
    {
      
        //bev�ger critteren imod (MoveTowards) det n�ste target
        transform.position = Vector2.MoveTowards(transform.position, target, movementRate * Time.deltaTime);

        //hvis det er Vector2 kan den ikke sammenligne position
        //tjekker om critterens position er den samme som m�let (target)
        if (transform.position == target)
        {
            //hvis vi har en vej og vi har brug for flere nodes s� skal critters blive ved med at bev�ge sig 
            if (critterRoad != null && critterRoad.Count > 0)
            {
                //s�tter den nye position p� banen
                MapPosition = critterRoad.Peek().GrafPosition;
                //s�tter det n�ste/nye target
                target = critterRoad.Pop().MapPosition;
            }
        }
    }

    //for at kunne f� critterens position p� banen
    public GraphPoint MapPosition { get; set; }

    //critterens n�ste m�l (target)
    private Vector3 target;

    //bruger Update til hele tiden (eller every frame) at kalde p� Move 
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
        //hvis vores critters rammer slutpunktet p� banen
        if(hit.tag == "EndTile")
        {
            //s� skal de fjernes
            Destroy(gameObject);    
        }
    } 
}