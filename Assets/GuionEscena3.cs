using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuionEscena3 : MonoBehaviour
{
    public OneAnimationController oac;
    public MoviAnimationController mac;

    public MessageManager mm;

    public NavMeshController navMeshOne;
    public NavMeshController navMeshMovi;

    public GameObject one;
    public GameObject movi;
    public GameObject player;
    public GameObject activatedStairs;

    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("escena3") == 0)
        {

            navMeshOne.parte = 1;
            navMeshMovi.parte = 1;
            StartCoroutine(Guion());
        }
        else
        {
            activatedStairs.SetActive(true);
            navMeshOne.parte = 2;
            navMeshMovi.parte = 2;
            StartCoroutine(Guion2());
        }
    }

    IEnumerator Guion()
    {
        //Rotacion de movi y one hacia el player
        target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        movi.transform.LookAt(target);
        one.transform.LookAt(target);

        yield return new WaitForSeconds(1);
        oac.Talk();
        mm.message(); //0
        yield return new WaitForSeconds(1.55f);
        oac.NoTalk();
        mac.Talk();
        yield return new WaitForSeconds(1);
        mm.message(); //1
        yield return new WaitForSeconds(4.2f);
        mac.NoTalk();
        yield return new WaitForSeconds(1);
        oac.Talk();
        yield return new WaitForSeconds(0.5f);
        mm.message(); //2
        yield return new WaitForSeconds(3.7f);
        oac.NoTalk();
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("escena3", 1);

        SceneManager.LoadSceneAsync(4);
    }

    IEnumerator Guion2()
    {
        mm.id = 3;

        //Rotacion de movi y one hacia el player
        target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        movi.transform.LookAt(target);
        one.transform.LookAt(target);

        yield return new WaitForSeconds(1);
        mac.Talk();
        mm.message(); //3
        yield return new WaitForSeconds(4);
        mac.NoTalk();
        yield return new WaitForSeconds(1);
        oac.Talk();
        mm.message(); //4
        yield return new WaitForSeconds(1.65f);
        oac.NoTalk();
        yield return new WaitForSeconds(1);
        mac.Talk();
        mm.message(); //5
        yield return new WaitForSeconds(3.3f);
        mac.NoTalk();
        yield return new WaitForSeconds(0.5f);
        mac.Walk();
        yield return new WaitForSeconds(1.7f);
        navMeshMovi.enabled = true;
        yield return new WaitForSeconds(0.5f);
        oac.Walk();
        mac.Run();
        yield return new WaitForSeconds(0.5f);
        navMeshOne.enabled = true;
        yield return new WaitForSeconds(0.5f);
        oac.Run();
        yield return new WaitForSeconds(7);
        movi.SetActive(false);
        yield return new WaitForSeconds(1);
        one.SetActive(false);
      
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("escena3", 0);

        SceneManager.LoadSceneAsync(21);
    }
}