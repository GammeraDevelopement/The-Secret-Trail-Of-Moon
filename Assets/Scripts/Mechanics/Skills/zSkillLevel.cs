using System.Collections;
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

    public Button butCsat; 
    public Button butEnigma;
    public Button butCubos;
    public Button butTekaTeki;
    public Button butGoNoGo;

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

        imagenPlanificacion.fillAmount = calculate(levelManager.getSkill("planificacion").currentExp, levelManager.getSkill("Planificacion").maxSkillExp);
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

        if (levelManager.getSkill("planificacion").currentExp >= levelManager.getSkill("Planificacion").maxSkillExp)
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
    /*
    public void addExpCSAT()
    {
        expAtencionSostenida += levelJSONManager.getExperienciaCSAT(1) * 0.8f;
        expControlInhibitorio += levelJSONManager.getExperienciaCSAT(1) * 0.2f;
    }

    public void addExpEnigma()
    {
        expMemTrabajo += levelJSONManager.getExperienciaEnigma(1) * 0.8f;
        expPlanificacion += levelJSONManager.getExperienciaEnigma(1) * 0.2f;
    }

    public void addExpCubos()
    {
        expProcVisoEspacial += levelJSONManager.getExperienciaCubos(1) * 0.8f;
        expAtencionSostenida += levelJSONManager.getExperienciaCubos(1) * 0.2f;
    }

    public void addExpTekaTeki(int level)
    {
        expPlanificacion += levelJSONManager.getExperienciaTekaTeki(level) * 0.9f;
        expControlInhibitorio += levelJSONManager.getExperienciaTekaTeki(level) * 0.1f;
    }

    public void addExpGoNoGo()
    {
        expAtencionSostenida += levelJSONManager.getExperienciaGoNoGo(1) * 0.2f;
        expControlInhibitorio += levelJSONManager.getExperienciaGoNoGo(1) * 0.8f;
    }
    */
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
