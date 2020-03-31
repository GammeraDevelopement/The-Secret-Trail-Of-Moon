using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class zSkillLevel : MonoBehaviour

{
    public int lvlMemTrabajo = 1;
    public float expMemTrabajo = 0;
    public float expMaxMemTrabajo = 100.0f;
    public Slider sliderMemTrabajo;
    public TMP_Text textMemTrabajo;

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
        expMaxMemTrabajo = 100.0f;
        expMemTrabajo = 0;

        sliderMemTrabajo.value = calculate(expMemTrabajo, expMaxMemTrabajo);
        textMemTrabajo.text = lvlMemTrabajo + "";
    }

    // Update is called once per frame
    void Update()
    {// imagen.fill = calculate;
        sliderMemTrabajo.value = calculate(expMemTrabajo, expMaxMemTrabajo);
        textMemTrabajo.text = lvlMemTrabajo + "";

        if (expMemTrabajo >= expMaxMemTrabajo)
        {
            lvlUpMemTrabajo(1, 0, 25);
        }
    }

    public float calculate(float expAct, float expMax)
    {
        return expAct / expMax;
    }

    public void addExp()
    {


    }

    public void lvlUpMemTrabajo(int skillLvl, float currentExp, float maxExp)
    {
        lvlMemTrabajo += skillLvl;
        expMemTrabajo = currentExp;
        expMaxMemTrabajo += maxExp;
    }

}
