using UnityEngine;

/*
Skal indeholde prefabs for vores Critters s� vi kan genbruge dem
Lige nu har vi kun 1 r�d critter prefab
*/

public class CritGenerator : MonoBehaviour
{
    //her kan vi smide de objekter vi �nsker at genere
    [SerializeField]
    public GameObject[] critterPrefabs;

    //henter en critter fra vores CritGenerator
    public GameObject GetCritter(string namePrefab)
    {
      
        //hvis vores CritGenerator ikke har den critter vi har brug for s� skal vi lave en ny
        for (int i = 0; i < critterPrefabs.Length; i++)
        {
         
            //hvis vi har navnet p� vores critterPrefab
            if (critterPrefabs[i].name == namePrefab)
            {   
                //s� instantiator vi denne prefab
                GameObject newCritter = Instantiate(critterPrefabs[i]);
                newCritter.name = namePrefab;
             
                return newCritter;
            }
        }

        return null;

    }
}