using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionEscena1 : MonoBehaviour
{

    public OneAnimationController oac;
    public MessageManager mm;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            PlayerPrefs.SetInt("escena1", 1);
        }
        else {
            PlayerPrefs.SetInt("escena1", 0);
        }

        StartCoroutine(Guion());
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("escena1") == 1)
        {
            StartCoroutine(Guion2());
            PlayerPrefs.SetInt("escena1", 0);
        }    
    }

    IEnumerator Guion() {
        oac.Sleep();
        yield return new WaitForSeconds(5);
        oac.NoSleep();
        oac.Sit();
        yield return new WaitForSeconds(2);
        oac.Talk();  // Sentado
        yield return new WaitForSeconds(5);
        mm.message();
        yield return new WaitForSeconds(5);
        mm.message();
        yield return new WaitForSeconds(5);
        mm.message();
        yield return new WaitForSeconds(5);
        oac.NoTalk();  // Sentado
        PlayerPrefs.SetInt("escena1", 1);
    }

    IEnumerator Guion2() {
        yield return new WaitForSeconds(1);
        oac.StandUp();
        yield return new WaitForSeconds(1);
        oac.Talk();
        yield return new WaitForSeconds(5);
        mm.message();
        yield return new WaitForSeconds(5);
        mm.message();
        yield return new WaitForSeconds(5);
        oac.NoTalk();
        yield return new WaitForSeconds(5);

    }

}
