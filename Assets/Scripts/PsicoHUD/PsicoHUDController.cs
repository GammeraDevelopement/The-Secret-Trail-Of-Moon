using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PsicoHUDController : MonoBehaviour
{

    public List<GameObject> scenePool;

    public Transform content;

    public Slider[] slider;
    public Dropdown dropdown;
    public GameObject loading;

    // Start is called before the first frame update
    void Start()
    {
        scenePool = new List<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScenes() {
        string sceneString = "";
        foreach (GameObject item in scenePool) {
            PsicoHUDSceneButton phsb = item.GetComponent<PsicoHUDSceneButton>();
            sceneString += phsb.name + ":" + phsb.level +";"/* ";XpScene;"*/;
        }
        Debug.Log(sceneString);
        loading.SetActive(true);
        PlayerPrefs.SetString("SceneString", sceneString);
        gameObject.GetComponent<SceneLoader>().LoadSceneInOrder();
    }

    public void AddToScenePool(GameObject mechanic) {

        string name = mechanic.GetComponent<PsicoHUDSceneButton>().name;

       GameObject clone = Instantiate(mechanic, content);
        string value = "";
        switch (name)
        {
            case "smasher":
                value = slider[0].value + "";
                break;
            case "tekateki":
                value = slider[1].value + "";
                break;
            case "kuburi":
                value = slider[2].value + "";
                break;
            case "enigma":
                value = slider[3].value + "";
                break;
            case "kitsune":
                value = slider[4].value + "";
                break;
            case "chess":
                value = dropdown.captionText.text + "";
                break;
            default:
                break;
        }

        //Se ha sacado el valor del componente al exterior para poder dar valor al atributo level de cada prefab
        clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = value;
        clone.GetComponent<PsicoHUDSceneButton>().level = value;

        scenePool.Add(clone);
    }
}
