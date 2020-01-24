using UnityEngine;
using UnityEngine.UI;

public class CSAT_ShowTarget : MonoBehaviour
{
    public Image target1;
    public Image target2;
    public Image target3;
    public Text pena;
    public Sprite spriteComodin;

    private CSAT csat;

    // Start is called before the first frame update
    void Start()
    {
        csat = GetComponent<CSAT>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!csat.tresTarget && csat.instruccion) {
            target1.sprite = csat.imagenesAMostrar[1];
            target2.sprite = csat.imagenesAMostrar[2];
        } else if (!csat.tresTarget && !csat.instruccion) {
            target1.sprite = csat.imagenesAMostrar[2];
            target2.sprite = csat.imagenesAMostrar[1];
        } else {
            target1.sprite = csat.imagenesAMostrar[1];
            target2.sprite = spriteComodin;
            target3.sprite = csat.imagenesAMostrar[2];
        }
        pena.text = "-"+ csat.penalizacionDelError;
    }
}
