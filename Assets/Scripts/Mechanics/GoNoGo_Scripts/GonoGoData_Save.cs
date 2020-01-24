using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GonoGoData_Save: MonoBehaviour
{
    
    public int aciertosRonda1 = 0; //Ha dado correctamente
    public int aciertosRonda2 = 0; //No dar al estímulo
    
    public int errorPorContinuación = 0;  //Errores en la segunda ronda.
    public int errorPorAzar;   //--> esto lo verán los psicólogos en persona
    public int errorPorOmision = 0;    //Error por omitir el estímulo al que tiene que dar.
    public int errorPorPerseveracionDeRespuesta;   //Ha dado a un input distinto sobre un mismo estímulo cada vez que aparece. --> esto lo verán los psicólogos en persona

    /*public string[] dataToSave()
    {
        string[] contents = new string[]
        {
            "Aciertos Ronda 1: " + aciertosRonda1,
            "Aciertos Ronda 2" + aciertosRonda2,
            "Error por continuación" + errorPorContinuación,
            "Error por omisión" + errorPorOmision
        };
        return contents;
    }*/

    public void set_aciertosRonda1()
    {
        aciertosRonda1++;
    }
    public void set_aciertosRonda2()
    {
        aciertosRonda2++;
    }

    public void set_errorXContinuacion()
    {
        errorPorContinuación++;
    }
    public void set_errorXOmision()
    {
        errorPorOmision++;
    }
}
