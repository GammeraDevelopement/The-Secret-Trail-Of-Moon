using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionEscena1 : MonoBehaviour
{

    public OneAnimationController oac;
    public MessageManager mm;

    public NavMeshController navMesh;

    public GameObject one;

    // Start is called before the first frame update
    void Start()
    {


        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            StartCoroutine(Guion());
            
        }
        else {
            StartCoroutine(Guion2());
        }

        navMesh.enabled = false;
    }


    IEnumerator Guion() {

        oac.Sleep();
        yield return new WaitForSeconds(3);
        oac.NoSleep();
        //oac.Sit();    TODO Activar
        yield return new WaitForSeconds(9);
        oac.Talk();//   TODO cambiar por SitTalk
        mm.message();
        yield return new WaitForSeconds(3);
        oac.NoTalk();//
        yield return new WaitForSeconds(1);
        oac.Talk();//
        mm.message();
        yield return new WaitForSeconds(3);
        oac.NoTalk();//
        yield return new WaitForSeconds(1);
        oac.Talk();//
        mm.message();
        yield return new WaitForSeconds(3);
        oac.NoTalk();//
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("escena1", 1);
        //TODO Llamar a kitsune
    }


    IEnumerator Guion2() {

        mm.id = 3;
        //oac.Sit();    TODO Activar
        yield return new WaitForSeconds(2);
        oac.Talk();
        mm.message();
        yield return new WaitForSeconds(5);
        oac.NoTalk();
        yield return new WaitForSeconds(1);
        oac.Talk();
        mm.message();
        yield return new WaitForSeconds(5);
        oac.NoTalk();
        yield return new WaitForSeconds(1);
        //oac.StandUp();    TODO Activar
        //yield return new WaitForSeconds(2);
        oac.Walk();
        yield return new WaitForSeconds(1);
        navMesh.enabled = true;     //Se activa el navMesh para mover a One
        while (!navMesh.finish)
        {
            yield return null;
        }
        PlayerPrefs.SetInt("escena1", 0);

        one.SetActive(false);
    }

}
