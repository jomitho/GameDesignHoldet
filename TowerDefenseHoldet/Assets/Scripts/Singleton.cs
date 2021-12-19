using UnityEngine;

/*
Abstrakt generisk klasse når vi har brug for en singleton
Laver en unique instans af objekter
*/

public abstract class Singleton <T> : MonoBehaviour where T : MonoBehaviour
{
    //laver instansen af vores singleton
    private static T instance;

    //laver propterty for at kunne tilgå den
    public static T Instance
    {
        get
        {
            //hvis instansen er null
            if (instance == null)
            {
                //så skal objektet findes
                instance = FindObjectOfType<T>();
            }
            //og returneres
            return instance;
        }
    }
}