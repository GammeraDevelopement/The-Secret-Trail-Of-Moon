using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nivel", menuName = "Nivel", order = 0)]
public class Level : ScriptableObject
{
    public string name;

    public int lvlSkill;
    public float currentExp;
    public float maxSkillExp;
    public string skillText;
    public Sprite skillImage;

 

    
    /*
    public void lvlUpMemTrabajo(int skillLvl, float currentExp)
    {
        lvlMemTrabajo += skillLvl;
        expMemTrabajo = currentExp;
        expMaxMemTrabajo = levelJSONManager.getExperienciaMax(lvlMemTrabajo);
    }*/
}
