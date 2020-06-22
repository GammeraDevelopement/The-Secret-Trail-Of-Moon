using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionEscena2 : MonoBehaviour
{

    public OneAnimationController oac;
    public MoviAnimationController mac;

    public MessageManager mm;

    public NavMeshController navMeshOne;
    public NavMeshController navMeshMovi;

    public GameObject one;
    public GameObject movi;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("escena2", 0);

        if (PlayerPrefs.GetInt("escena2") == 0) {

            StartCoroutine(Guion());
        }
        else
        {
            StartCoroutine(Guion2());
        }
        
    }

    IEnumerator Guion() {

       // mm.escena = 1;
        //mm.message();
        yield return new WaitForSeconds(8);
    }

    IEnumerator Guion2()
    {

        yield return new WaitForSeconds(1);
    }

}
