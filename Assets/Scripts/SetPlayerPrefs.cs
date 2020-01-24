using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetPlayerPrefs : MonoBehaviour
{
    public TMP_InputField input;

    public void setCSATLevel() { PlayerPrefs.SetInt("nivelCSAT", int.Parse(input.text)); }
    public void setSokobanLevel() { PlayerPrefs.SetInt("nivelSokoban", int.Parse(input.text)); }
    public void setEnigmaLevel() { PlayerPrefs.SetInt("nivelEnigma", int.Parse(input.text)); }
    public void setCubosLevel() { PlayerPrefs.SetInt("nivelCubos", int.Parse(input.text)); }
    public void setGonogoLevel() { PlayerPrefs.SetInt("nivelGonogo", int.Parse(input.text)); }
    public void setTorreLevel() { PlayerPrefs.SetInt("nivelTorre", int.Parse(input.text)); }
    public void setAlfilLevel() { PlayerPrefs.SetInt("nivelAlfil", int.Parse(input.text)); }
    public void setCSATLevel(int level) { PlayerPrefs.SetInt("nivelCSAT", level); }
    public void setSokobanLevel(int level) { PlayerPrefs.SetInt("nivelSokoban", level); }
    public void setEnigmaLevel(int level) { PlayerPrefs.SetInt("nivelEnigma", level); }
    public void setCubosLevel(int level) { PlayerPrefs.SetInt("nivelCubos", level); }
    public void setGonogoLevel(int level) { PlayerPrefs.SetInt("nivelGonogo", level); }
    public void setTorreLevel(int level) { PlayerPrefs.SetInt("nivelTorre", level); }
    public void setAlfilLevel(int level) { PlayerPrefs.SetInt("nivelAlfil", level); }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
