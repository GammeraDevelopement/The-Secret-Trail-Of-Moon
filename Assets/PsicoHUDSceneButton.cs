using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PsicoHUDSceneButton : MonoBehaviour
{
    public string name;
    public string level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteSceneOnClick() {
        PsicoHUDController psico = GameObject.FindGameObjectWithTag("GameController").GetComponent<PsicoHUDController>();
        
        psico.scenePool.Remove(gameObject);

        GameObject.Find("Dropdown").GetComponent<Dropdown>().Select();

        Destroy(gameObject);
        


    }
}
