using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PsicoHUDController : MonoBehaviour
{

    public List<GameObject> scenePool;

    public Transform content;

    public Slider[] slider;
    public Dropdown dropdown;

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

        string name = mechanic.GetComponent<PsicoHUDSceneButton>().name;

       GameObject clone = Instantiate(mechanic, content);

        switch (name)
        {
            case "smasher":
                clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = slider[0].value +"";
                break;
            case "tekateki":
                clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = slider[1].value + "";
                break;
            case "kuburi":
                clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = slider[2].value + "";
                break;
            case "enigma":
                clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = slider[3].value + "";
                break;
            case "kitsune":
                clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = slider[4].value + "";
                break;
            case "chess":
                clone.transform.GetChild(clone.transform.childCount - 1).GetComponent<TMP_Text>().text = dropdown.captionText.text + "";
                break;
            default:
                break;
        }

        
        scenePool.Add(clone);
    }
}
