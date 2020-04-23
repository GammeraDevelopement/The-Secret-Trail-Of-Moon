using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NivelManager", menuName = "LevelManager")]
public class LevelManager : ScriptableObject
{
    public Dictionary<string, Level> levels;

    public LevelJSONManager levelJSONManager;

    public void addExp(string name, int mechanicLevel)
    {
        Level nivel;
       
            switch (name)
            {
                case "Smasher":
                levels.TryGetValue("AtencionSostenida", out nivel);
                nivel.currentExp += levelJSONManager.getExperienciaCSAT(mechanicLevel) * 0.8f;
                levels.TryGetValue("ControlInhibitorio", out nivel);
                nivel.currentExp += levelJSONManager.getExperienciaCSAT(mechanicLevel) * 0.2f;
                    break;
                case "Kuburi":
                    break;
                case "TekaTeki":
                    break;
                case "Enigma":
                    break;
                case "Kitsune":
                    break;

                default:
                    break;
            }
        

        

    }
}
