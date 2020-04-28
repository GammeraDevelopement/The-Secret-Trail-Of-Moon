﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(LevelJSONManager))]

public class zSkillLevel : MonoBehaviour

{
    public LevelManager levelManager;

    public TMP_Text textMemTrabajo;
    public Image imagenMemTrabajo;

    public TMP_Text textPlanificacion;
    public Image imagenPlanificacion;


    public TMP_Text textControlInhibitorio;
    public Image imagenControlInhibitorio;


    public TMP_Text textAtencionSostenida;
    public Image imagenAtencionSostenida;


    public TMP_Text textProcVisoEspacial;
    public Image imagenProcVisoEspacial;

    public float receivedExp = 1.0f;

    public Button butSmasher; 
    public Button butEnigma;
    public Button butKuburi;
    public Button butTekaTeki;
    public Button butKitsune;

    private LevelJSONManager levelJSONManager;

    // Start is called before the first frame update
    void Start()
    {


        levelJSONManager = gameObject.GetComponent<LevelJSONManager>();
        levelManager.Instantiate(levelJSONManager);

        //Inicializar exp
        /*levelManager.getSkill("MemTrabajo").maxSkillExp = levelJSONManager.getExperienciaMax(1);
        levelManager.getSkill("Planificacion").maxSkillExp = levelJSONManager.getExperienciaMax(1);
        levelManager.getSkill("ControlInhibitorio").maxSkillExp = levelJSONManager.getExperienciaMax(1);
        levelManager.getSkill("AtencionSostenida").maxSkillExp = levelJSONManager.getExperienciaMax(1);
        levelManager.getSkill("ProcVisoEspacial").maxSkillExp = levelJSONManager.getExperienciaMax(1);
        */

        foreach (Level item in levelManager.levels)
        {
            item.maxSkillExp = levelJSONManager.getExperienciaMax(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Relleno de la exp de la skill y mostrar numero del nivel
        imagenMemTrabajo.fillAmount = calculate(levelManager.getSkill("MemTrabajo").currentExp, levelManager.getSkill("MemTrabajo").maxSkillExp);
        textMemTrabajo.text = levelManager.getSkill("MemTrabajo").lvlSkill + "";

        imagenPlanificacion.fillAmount = calculate(levelManager.getSkill("Planificacion").currentExp, levelManager.getSkill("Planificacion").maxSkillExp);
        textPlanificacion.text = levelManager.getSkill("Planificacion").lvlSkill + "";

        imagenControlInhibitorio.fillAmount = calculate(levelManager.getSkill("ControlInhibitorio").currentExp, levelManager.getSkill("ControlInhibitorio").maxSkillExp);
        textControlInhibitorio.text = levelManager.getSkill("ControlInhibitorio").lvlSkill + "";

        imagenAtencionSostenida.fillAmount = calculate(levelManager.getSkill("AtencionSostenida").currentExp, levelManager.getSkill("AtencionSostenida").maxSkillExp);
        textAtencionSostenida.text = levelManager.getSkill("AtencionSostenida").lvlSkill + "";

        imagenProcVisoEspacial.fillAmount = calculate(levelManager.getSkill("ProcVisoEspacial").currentExp, levelManager.getSkill("ProcVisoEspacial").maxSkillExp);
        textProcVisoEspacial.text = levelManager.getSkill("ProcVisoEspacial").lvlSkill + "";
        
        //Condicion para subir de nivel
        if (levelManager.getSkill("MemTrabajo").currentExp >= levelManager.getSkill("MemTrabajo").maxSkillExp)
        {
            lvlUpMemTrabajo();
        }

        if (levelManager.getSkill("Planificacion").currentExp >= levelManager.getSkill("Planificacion").maxSkillExp)
        {
            lvlUpPlanificacion();
        }

        if (levelManager.getSkill("ControlInhibitorio").currentExp >= levelManager.getSkill("ControlInhibitorio").maxSkillExp)
        {
            lvlUpControlInhibitorio();
        }

        if (levelManager.getSkill("AtencionSostenida").currentExp >= levelManager.getSkill("AtencionSostenida").maxSkillExp)
        {
            lvlUpAtencionSostenida();
        }

        if (levelManager.getSkill("ProcVisoEspacial").currentExp >= levelManager.getSkill("ProcVisoEspacial").maxSkillExp)
        {
            lvlUpProcVisoEspacial();
        }
    }

    public float calculate(float expAct, float expMax)
    {
        return expAct / expMax;
    }
    
    public void addExpSmasher(int nivel)
    {
        levelManager.AddExp("Smasher", nivel);
    }

    public void addExpEnigma(int nivel)
    {
        levelManager.AddExp("Enigma", nivel);
    }

    public void addExpKuburi(int nivel)
    {
        levelManager.AddExp("Kuburi", nivel);
    }

    public void addExpTekaTeki(int nivel)
    {
        levelManager.AddExp("TekaTeki", nivel);
    }

    public void addExpKitsune(int nivel)
    {
        levelManager.AddExp("Kitsune", nivel);
    }
    
    public void lvlUpMemTrabajo()
    {
       /* lvlMemTrabajo += 1;
        expMemTrabajo = 0;
        expMaxMemTrabajo = levelJSONManager.getExperienciaMax(lvlMemTrabajo);*/
        foreach (Level item in levelManager.levels)
        {
            if (item.name == "MemTrabajo")
            {
                item.lvlSkill += 1;
                item.currentExp = 0;
                item.maxSkillExp = levelJSONManager.getExperienciaMax(item.lvlSkill);
            }
        }
    }

    public void lvlUpPlanificacion()
    {
        foreach (Level item in levelManager.levels)
        {
            if (item.name == "Planificacion")
            {
                item.lvlSkill += 1;
                item.currentExp = 0;
                item.maxSkillExp = levelJSONManager.getExperienciaMax(item.lvlSkill);
            }
        }
    }

    public void lvlUpControlInhibitorio()
    {
        foreach (Level item in levelManager.levels)
        {
            if (item.name == "ControlInhibitorio")
            {
                item.lvlSkill += 1;
                item.currentExp = 0;
                item.maxSkillExp = levelJSONManager.getExperienciaMax(item.lvlSkill);
            }
        }
    }

    public void lvlUpAtencionSostenida()
    {
        foreach (Level item in levelManager.levels)
        {
            if (item.name == "AtencionSostenida")
            {
                item.lvlSkill += 1;
                item.currentExp = 0;
                item.maxSkillExp = levelJSONManager.getExperienciaMax(item.lvlSkill);
            }
        }
    }

    public void lvlUpProcVisoEspacial()
    {
        foreach (Level item in levelManager.levels)
        {
            if (item.name == "ProcVisoEspacial")
            {
                item.lvlSkill += 1;
                item.currentExp = 0;
                item.maxSkillExp = levelJSONManager.getExperienciaMax(item.lvlSkill);
            }
        }
    }

    
}
