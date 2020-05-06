using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsicoHUDSceneButton : MonoBehaviour
{
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

        //borrar del componente
    }
}
