using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zSkillLevel : MonoBehaviour

{
    public int lvlMemTrabajo { get; set; }
    public float expMemTrabajo = 0f;
    public float expMaxMemTrabajo = 100.0f;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //addExp();
        if (expMemTrabajo >= expMaxMemTrabajo)
        {
            lvlUp(lvlMemTrabajo);
        }
    }

    public void addExp( float skill)
    {


    }

    public void lvlUp(int skillLvl)
    {
        skillLvl += 1;
    }

    
}
