using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(LevelJSONManager))]

public class zSkillLevel : MonoBehaviour

{
    public int lvlMemTrabajo = 1;
    public float expMemTrabajo = 0;
    public float expMaxMemTrabajo;
    public TMP_Text textMemTrabajo;
    public Image imagenMemTrabajo;

    public int lvlPlanificacion = 1;
    public float expPlanificacion = 0f;
    public float expMaxPlanificacion = 100.0f;
    public TMP_Text textPlanificacion;
    public Image imagenPlanificacion;

    public int lvlControlInhibitorio = 1;
    public float expControlInhibitorio = 0f;
    public float expMaxControlInhibitorio = 100.0f;
    public TMP_Text textControlInhibitorio;
    public Image imagenControlInhibitorio;

    public int lvlAtencionSostenida = 1;
    public float expAtencionSostenida = 0f;
    public float expMaxAtencionSostenida = 100.0f;
    public TMP_Text textAtencionSostenida;
    public Image imagenAtencionSostenida;

    public int lvlProcVisoEspacial = 1;
    public float expProcVisoEspacial = 0f;
    public float expMaxProcVisoEspacial = 100.0f;
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

        expMaxMemTrabajo = levelJSONManager.getExperienciaMax(1);
        expMaxPlanificacion = levelJSONManager.getExperienciaMax(1);
        expMaxControlInhibitorio = levelJSONManager.getExperienciaMax(1);
        expMaxAtencionSostenida = levelJSONManager.getExperienciaMax(1);
        expMaxProcVisoEspacial = levelJSONManager.getExperienciaMax(1);

    }

    // Update is called once per frame
    void Update()
    {
        imagenMemTrabajo.fillAmount = calculate(expMemTrabajo, expMaxMemTrabajo);
        textMemTrabajo.text = lvlMemTrabajo + "";
        imagenPlanificacion.fillAmount = calculate(expPlanificacion, expMaxPlanificacion);
        textPlanificacion.text = lvlPlanificacion + "";
        imagenControlInhibitorio.fillAmount = calculate(expControlInhibitorio, expMaxControlInhibitorio);
        textControlInhibitorio.text = lvlControlInhibitorio + "";
        imagenAtencionSostenida.fillAmount = calculate(expAtencionSostenida, expMaxAtencionSostenida);
        textAtencionSostenida.text = lvlAtencionSostenida + "";
        imagenProcVisoEspacial.fillAmount = calculate(expProcVisoEspacial, expMaxProcVisoEspacial);
        textProcVisoEspacial.text = lvlProcVisoEspacial + "";


        if (expMemTrabajo >= expMaxMemTrabajo)
        {
            lvlUpMemTrabajo(1, 0);
        }

        if (expPlanificacion >= expMaxPlanificacion)
        {
            lvlUpPlanificacion(1, 0);
        }

        if (expControlInhibitorio >= expMaxControlInhibitorio)
        {
            lvlUpControlInhibitorio(1, 0);
        }

        if (expAtencionSostenida >= expMaxAtencionSostenida)
        {
            lvlUpAtencionSostenida(1, 0);
        }

        if (expProcVisoEspacial >= expMaxProcVisoEspacial)
        {
            lvlUpProcVisoEspacial(1, 0);
        }
    }

    public float calculate(float expAct, float expMax)
    {
        return expAct / expMax;
    }

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

    public void lvlUpMemTrabajo(int skillLvl, float currentExp)
    {
        lvlMemTrabajo += skillLvl;
        expMemTrabajo = currentExp;
        expMaxMemTrabajo = levelJSONManager.getExperienciaMax(lvlMemTrabajo);
    }

    public void lvlUpPlanificacion(int skillLvl, float currentExp)
    {
        lvlPlanificacion += skillLvl;
        expPlanificacion = currentExp;
        expMaxPlanificacion = levelJSONManager.getExperienciaMax(lvlPlanificacion);
    }

    public void lvlUpControlInhibitorio(int skillLvl, float currentExp)
    {
        lvlControlInhibitorio += skillLvl;
        expControlInhibitorio = currentExp;
        expMaxControlInhibitorio += levelJSONManager.getExperienciaMax(lvlControlInhibitorio); ;
    }

    public void lvlUpAtencionSostenida(int skillLvl, float currentExp)
    {
        lvlAtencionSostenida += skillLvl;
        expAtencionSostenida = currentExp;
        expMaxAtencionSostenida = levelJSONManager.getExperienciaMax(lvlAtencionSostenida); ;
    }

    public void lvlUpProcVisoEspacial(int skillLvl, float currentExp)
    {
        lvlProcVisoEspacial += skillLvl;
        expProcVisoEspacial = currentExp;
        expMaxProcVisoEspacial += levelJSONManager.getExperienciaMax(lvlProcVisoEspacial); ;
    }

    
}
