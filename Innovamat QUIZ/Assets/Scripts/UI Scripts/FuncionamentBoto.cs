using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncionamentBoto : MonoBehaviour
{
    //Creem els tres tipus de colors diferents depenent el seu estat
    public Color ColorNormal;
    public Color ColorFallo;
    public Color ColorEncertat;
    //Agafem la imatge del boto per aplicar els colors
    private Image i_boto;
    //Creem la variable que dirà si l'opció és correcta o no
    public bool esLaCorrecta = false;
    //Referenciem el GameLoop per poder accedir als contadors i funcions
    public GameLoop gameLoop;

    // Start is called before the first frame update
    void Start()
    {
        i_boto = GetComponent<Image>();

        ResetNormalColor();
    }

    //Apliquem a la imatge el color predeterminat
    public void ResetNormalColor()
    {
        if (i_boto != null) i_boto.color = ColorNormal;
    }

    //Actualitzem de nou el color del boto, aquesta funció es cridada principalment quan cliquem
    //al botó. La variable saltar es per si accedim des de codi. Per poder canviar els colors sense
    //tenir que esperar a que el jugador pugui interaccionar.
    public void ActualitzarColorBoto(bool saltar=true)
    {
        if (gameLoop.potContestar == true || saltar == true)
        {
            //Si la resposta es correcta li assignem el ColorEncertat
            if (esLaCorrecta == true)
            {
                if (i_boto != null) i_boto.color = ColorEncertat;
            }
            //Si la resposta es incorrecta li assignem el ColorFallo
            else if (esLaCorrecta == false)
            {
                if (i_boto != null) i_boto.color = ColorFallo;
            }
        }
    }

    //Aquesta funció es cridarà quan es cliqui l'opció.
    public void CheckResposta()
    {
        //Primer es comprova si el jugador pot interactuar i realitzar canvis
        if (gameLoop.potContestar == true)
        {
            //Si no és l'opció correcta es sumarà una unitat al contador temporal i si aquest
            //arriba a dos retorna al GameLoop que ha fallat durant aquest torn.
            if (esLaCorrecta == false)
            {
                gameLoop.errorsTemporals++;
                if (gameLoop.errorsTemporals >= 2)
                {
                    StartCoroutine(gameLoop.CanviarDades(false));
                }
            }
            //Si l'opció es la correcta es retorna al GameLoop que s'ha encertat durant el torn.
            else if (esLaCorrecta == true)
            {
                StartCoroutine(gameLoop.CanviarDades(true));
            }
        }
    }
}
