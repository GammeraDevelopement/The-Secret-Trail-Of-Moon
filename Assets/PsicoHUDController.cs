using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsicoHUDController : MonoBehaviour
{
    public GameObject[] scenePrefabs;

    public List<GameObject> scenePool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScenePool(GameObject mechanic) {
        scenePool.Add(mechanic);
    }
}
