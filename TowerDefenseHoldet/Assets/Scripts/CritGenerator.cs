using UnityEngine;

/*
Skal indeholde prefabs for vores Critters så vi kan genbruge dem
Lige nu har vi kun 1 rød critter prefab
*/

public class CritGenerator : MonoBehaviour
{
    //her kan vi smide de objekter vi ønsker at genere
    [SerializeField]
    public GameObject[] critterPrefabs;

    //henter en critter fra vores CritGenerator
    public GameObject GetCritter(string namePrefab)
    {
      
        //hvis vores CritGenerator ikke har den critter vi har brug for så skal vi lave en ny
        for (int i = 0; i < critterPrefabs.Length; i++)
        {
         
            //hvis vi har navnet på vores critterPrefab
            if (critterPrefabs[i].name == namePrefab)
            {   
                //så instantiator vi denne prefab
                GameObject newCritter = Instantiate(critterPrefabs[i]);
                newCritter.name = namePrefab;
             
                return newCritter;
            }
        }

        return null;

    }
}