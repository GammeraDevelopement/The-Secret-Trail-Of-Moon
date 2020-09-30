using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GuionEscena1 : MonoBehaviour
{

    public OneAnimationController oac;
    public MessageManager mm;

    public NavMeshController navMesh;

    public GameObject one;
    public GameObject rock;
/*
    [Header("Timelines")]
    private PlayableDirector director;
    public GameObject controlPanel;
    */
    // Por Navmeshes y animators

    void Start()
    {


        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            navMesh.parte = 1;
            StartCoroutine(Guion());      
        }
        else {
            navMesh.parte = 2;
            StartCoroutine(Guion2());
        }

        navMesh.enabled = false;
    }


    IEnumerator Guion() {
        rock.SetActive(true);
        oac.Sleep();
        yield return new WaitForSeconds(3);
        oac.NoSleep();
        oac.Sit();    
        yield return new WaitForSeconds(9);
        oac.SitTalk();
        mm.message();
        yield return new WaitForSeconds(5);
        oac.NoSitTalk();
        yield return new WaitForSeconds(1);
        oac.SitTalk();
        mm.message();
        yield return new WaitForSeconds(3);
        oac.NoSitTalk();
        yield return new WaitForSeconds(1);
        oac.SitTalk();
        mm.message();
        yield return new WaitForSeconds(3);
        oac.NoSitTalk();
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("escena1", 1);

        SceneManager.LoadSceneAsync(1);
    }


    IEnumerator Guion2() {

        rock.SetActive(false);
        mm.id = 3;
        oac.Sit();
        yield return new WaitForSeconds(3);
        oac.SitTalk();
        mm.message();
        yield return new WaitForSeconds(3.75f);
        oac.NoSitTalk();
        yield return new WaitForSeconds(1);
        oac.StandUp();
        yield return new WaitForSeconds(1);
        oac.Talk();
        yield return new WaitForSeconds(0.5f);
        mm.message();
        yield return new WaitForSeconds(4);
        oac.NoTalk();
        yield return new WaitForSeconds(1);
        //oac.StandUp();
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

        SceneManager.LoadSceneAsync(21);
    }

    //Por Timelines
/*
    public void NextScene()     //Carga la siguiente escena
    { 

        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            PlayerPrefs.SetInt("escena1", 1);
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            PlayerPrefs.SetInt("escena1", 0);
            SceneManager.LoadSceneAsync(21);
        }
        
    }

    public void StartAt()   //Para que empiece la segunda parte de la escena
    {
        PlayerPrefs.SetInt("escena1", 1);
        if (PlayerPrefs.GetInt("escena1") == 1){
            director = GetComponent<PlayableDirector>();
            director.time = 25;
        }
    }*/

}
