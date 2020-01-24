using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueToText : MonoBehaviour
{
    public TMP_InputField inputField;
    public Text texto;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    public void ChangeValue(bool entero)
    {
        if (inputField != null) {
            if (entero) {
                inputField.text = ("" + (int)slider.value);
            } else {
                inputField.text = "" + (Mathf.Round(slider.value * 100f) / 100f);
            }
        } else if (texto != null) {
            if (entero) {
                texto.text = ("" + (int)slider.value);
            } else {
                texto.text = "" + (Mathf.Round(slider.value * 100f) / 100f);
            }
        }
        
    }
}
