using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zSkillLevel : MonoBehaviour

{
    public int lvlMemTrabajo { get; set; }
    public float expMemTrabajo = 0f;
    public float expMaxMemTrabajo = 100.0f;
    public Slider sliderMemTrabajo;

    public int lvlPlanificacion = 0;
    public float expPlanificacion = 0f;
    public float expMaxPlanificacion = 100.0f;

    public int lvlControlInhibitorio = 0;
    public float expControlInhibitorio = 0f;
    public float expMaxControlInhibitorio = 100.0f;

    public int lvlAtencionSostenida = 0;
    public float expAtencionSostenida = 0f;
    public float expMaxAtencionSostenida = 100.0f;

    public int lvlProcVisoEspacial = 0;
    public float expProcVisoEspacial = 0f;
    public float expMaxProcVisoEspacial = 100.0f;

    public float receivedExp = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        addExp(expMemTrabajo, 5.0f, sliderMemTrabajo);

        if (expMemTrabajo >= expMaxMemTrabajo)
        {
            lvlUp(lvlMemTrabajo, expMaxMemTrabajo, expMemTrabajo);
        }
        if (expPlanificacion >= expMaxPlanificacion)
        {
            lvlUp(lvlPlanificacion, expMaxPlanificacion, expPlanificacion);
        }
        if (expPlanificacion >= expMaxPlanificacion)
        {
            lvlUp(lvlPlanificacion, expMaxPlanificacion, expPlanificacion);
        }
        if (expControlInhibitorio >= expMaxControlInhibitorio)
        {
            lvlUp(lvlControlInhibitorio, expMaxControlInhibitorio, expControlInhibitorio);
        }
        if (expAtencionSostenida >= expMaxAtencionSostenida)
        {
            lvlUp(lvlAtencionSostenida, expMaxAtencionSostenida, expAtencionSostenida);
        }
        if (expProcVisoEspacial >= expMaxProcVisoEspacial)
        {
            lvlUp(lvlProcVisoEspacial, expMaxProcVisoEspacial, expProcVisoEspacial);
        }
    }

    public void addExp( float skillExp, float receivedExp, Slider sli)
    {
        skillExp += 5.0f;
        sliderMemTrabajo.value = skillExp; 

    }

    public void lvlUp(int skillLvl, float maxExpSkill, float expSkill)
    {
        skillLvl += 1;
        maxExpSkill += 25.0f;
        expSkill = 0.0f;
    }

}
