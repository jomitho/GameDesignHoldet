using UnityEngine;

/*
Abstrakt generisk klasse n�r vi har brug for en singleton
Laver en unique instans af objekter
*/

public abstract class Singleton <T> : MonoBehaviour where T : MonoBehaviour
{
    //laver instansen af vores singleton
    private static T instance;

    //laver propterty for at kunne tilg� den
    public static T Instance
    {
        get
        {
            //hvis instansen er null
            if (instance == null)
            {
                //s� skal objektet findes
                instance = FindObjectOfType<T>();
            }
            //og returneres
            return instance;
        }
    }
}