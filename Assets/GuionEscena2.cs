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

        if (PlayerPrefs.GetInt("escena2") == 0) {

            StartCoroutine(Guion());
        }
        else
        {
            StartCoroutine(Guion2());
        }
        
    }

    IEnumerator Guion() {

        yield return new WaitForSeconds(2);
        oac.Talk();
        mac.Think();
        mm.message();                   //0
        yield return new WaitForSeconds(1.5f);
        oac.NoTalk();
        oac.Walk();
        yield return new WaitForSeconds(2);
        navMeshOne.enabled = true;
        yield return new WaitForSeconds(2);
        navMeshOne.enabled = false;
        yield return new WaitForSeconds(2);
        oac.NoWalk();
        mac.NoThink();
        yield return new WaitForSeconds(1);
        mac.Talk();
        mm.message();                   //1
        yield return new WaitForSeconds(3.5f);
        mac.noTalk();
        mac.Think();
        yield return new WaitForSeconds(1);
        mm.id = 2;  //Hay un error por el cual no se pasa a id 2  en mm y no se porque, asi que fuerzo
        oac.Talk();
        mm.message();                   //2
        yield return new WaitForSeconds(2.5f);
        oac.NoTalk();
        mac.NoThink();
        yield return new WaitForSeconds(3);
        mac.Talk();
        mm.message();                   //3
        yield return new WaitForSeconds(6);
        mac.noTalk();
        oac.Walk();
        yield return new WaitForSeconds(1);
        navMeshOne.enabled = true;
        /* while (!navMeshOne.finish)
         {
             yield return null;
             //yield return new WaitForSeconds(0.5f);
         }*/
        yield return new WaitForSeconds(1.5f);
        navMeshOne.enabled = false;
        yield return new WaitForSeconds(1);
        oac.NoWalk();
        oac.Talk();
        yield return new WaitForSeconds(1.5f);
        mm.message();                   //4
        yield return new WaitForSeconds(5);
        oac.NoTalk();
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("escena2", 1);
    }

    IEnumerator Guion2()
    {

        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("escena2", 0);
    }

}
