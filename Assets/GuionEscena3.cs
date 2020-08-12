using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //PlayerPrefs.SetInt("escena3", 1);
    }

    IEnumerator Guion2()
    {

        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("escena3", 0);
    }
}