using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NivelManager", menuName = "LevelManager")]
public class LevelManager : ScriptableObject
{

    public List<Level> levels;
    private LevelJSONManager levelJSONManager;

    public void addExp(string name, int mechanicLevel)
    {
        switch (name)
        {
            case "Smasher":
                foreach (Level item in levels)
                {
                    if (item.name == "AtencionSostenida")
                    {
                        item.currentExp += levelJSONManager.getExperienciaCSAT(mechanicLevel) * 0.8f;
                    }
                    if (item.name == "ControlInhibitorio")
                    {
                        item.currentExp += levelJSONManager.getExperienciaCSAT(mechanicLevel) * 0.2f;
                    }
                }
                break;
            case "Kuburi":
                foreach (Level item in levels)
                {
                    if (item.name == "ProcVisoEspacial")
                    {
                        item.currentExp += levelJSONManager.getExperienciaCubos(mechanicLevel) * 0.8f;
                    }
                    if (item.name == "AtencionSostenida") 
                    {
                        item.currentExp += levelJSONManager.getExperienciaCubos(mechanicLevel) * 0.2f;
                    }
                }
                break;
            case "TekaTeki":
                foreach (Level item in levels)
                {
                    if (item.name == "Planificacion")
                    {
                        item.currentExp += levelJSONManager.getExperienciaTekaTeki(mechanicLevel) * 0.9f;
                    }
                    if (item.name == "ControlInhibitorio")
                    {
                        item.currentExp += levelJSONManager.getExperienciaTekaTeki(mechanicLevel) * 0.1f;
                    }
                }
                break;
            case "Enigma":
                foreach (Level item in levels)
                {
                    if (item.name == "MemTrabajo")
                    {
                        item.currentExp += levelJSONManager.getExperienciaEnigma(mechanicLevel) * 0.8f;
                    }
                    if (item.name == "Planificacion")
                    {
                        item.currentExp += levelJSONManager.getExperienciaEnigma(mechanicLevel) * 0.2f;
                    }
                }
                break;
            case "Kitsune":
                foreach (Level item in levels)
                {
                    if (item.name == "AtencionSostenida")
                    {
                        item.currentExp += levelJSONManager.getExperienciaGoNoGo(mechanicLevel) * 0.8f;
                    }
                    if (item.name == "ControlInhibitorio")
                    {
                        item.currentExp += levelJSONManager.getExperienciaGoNoGo(mechanicLevel) * 0.2f;
                    }
                }
                break;

            default:
                break;
        }


    }
    public Level getSkill(string nombre) {

        foreach (Level item in levels)
        {
            if (item.name == nombre)
            {
                return item;
            }
        }
        return null;
    }

    public void Instantiate(LevelJSONManager levelJSONManager) {
        this.levelJSONManager = levelJSONManager;
    }
}
