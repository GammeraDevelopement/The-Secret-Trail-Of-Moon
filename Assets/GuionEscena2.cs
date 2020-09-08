using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuionEscena2 : MonoBehaviour
{

    public OneAnimationController oac;
    public MoviAnimationController mac;

    public MessageManager mm;

    public NavMeshController navMeshOne;
    public NavMeshController navMeshMovi;

    public GameObject one;
    public GameObject movi;
    public GameObject player;
    public GameObject auxPosFinalOne;

    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetInt("escena2") == 0) {

            navMeshOne.parte = 1;
            navMeshMovi.parte = 1;
            StartCoroutine(Guion());
        }
        else
        {
            navMeshOne.parte = 2;
            navMeshMovi.parte = 2;
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
        yield return new WaitForSeconds(1.5f);
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
        mac.NoTalk();
        mac.Think();
        yield return new WaitForSeconds(1);
        oac.Talk();
        mm.message();                   //2
        yield return new WaitForSeconds(2.6f);
        oac.NoTalk();
        mac.NoThink();
        yield return new WaitForSeconds(3);
        mac.Talk();
        mm.message();                   //3
        yield return new WaitForSeconds(6);
        mac.NoTalk();
        oac.Walk();
        yield return new WaitForSeconds(1);
        navMeshOne.enabled = true;
        yield return new WaitForSeconds(1.5f);
        oac.NoWalk();
        yield return new WaitForSeconds(0.5f);
        navMeshOne.enabled = false;
        yield return new WaitForSeconds(0.5f);
        oac.Talk();
        yield return new WaitForSeconds(0.5f);
        mm.message();                   //4
        yield return new WaitForSeconds(2.25f);
        oac.NoTalk();
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("escena2", 1);

        SceneManager.LoadSceneAsync(3);
    }

    IEnumerator Guion2()
    {
        
        mm.id = 5;
        one.transform.position = auxPosFinalOne.transform.position; //Poner a one en la misma posicion que termina la parte 1

        //Rotacion de movi y one hacia el player
        target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        movi.transform.LookAt(target);
        one.transform.LookAt(target);

        yield return new WaitForSeconds(1);
        mac.Talk();
        mm.message();           //5
        yield return new WaitForSeconds(1.8f);
        mac.NoTalk();
        yield return new WaitForSeconds(0.5f);
        mac.Talk();
        mm.message();           //6
        yield return new WaitForSeconds(2.8f);
        mac.NoTalk();        
        yield return new WaitForSeconds(1);
        oac.Talk();
        yield return new WaitForSeconds(1.2f);
        mm.message();           //7
        yield return new WaitForSeconds(2.3f);
        oac.NoTalk();
        yield return new WaitForSeconds(1);
        oac.Talk();
        yield return new WaitForSeconds(0.5f);
        mm.message();           //8
        yield return new WaitForSeconds(2.5f);
        oac.NoTalk();
        yield return new WaitForSeconds(1);
        mac.Talk();     
        mm.message();           //9
        yield return new WaitForSeconds(3.6f);
        mac.NoTalk();
        yield return new WaitForSeconds(1);
        mac.Walk();
        yield return new WaitForSeconds(1.2f);
        navMeshMovi.enabled = true;
        mac.Run();
        yield return new WaitForSeconds(2);
        oac.Talk();
        yield return new WaitForSeconds(1);
        mm.message();           //10
        yield return new WaitForSeconds(1.2f);
        oac.NoTalk();
        oac.Walk();
        yield return new WaitForSeconds(1.3f);
        navMeshOne.enabled = true;
        oac.Run();
        yield return new WaitForSeconds(5);
        movi.SetActive(false);
        yield return new WaitForSeconds(5);
        one.SetActive(false);
        Debug.Log("finis");
        PlayerPrefs.SetInt("escena2", 0);
        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(21);
    }

}
