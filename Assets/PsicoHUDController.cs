using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsicoHUDController : MonoBehaviour
{

    public List<GameObject> scenePool;

    public Transform content;

    // Start is called before the first frame update
    void Start()
    {
        scenePool = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScenePool(GameObject mechanic) {
        
       GameObject clone = Instantiate(mechanic, content);
        scenePool.Add(clone);
    }

    
}
