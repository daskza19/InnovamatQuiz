using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncionamentBoto : MonoBehaviour
{
    public Color ColorNormal;
    public Color ColorFallo;
    public Color ColorEncertat;
    public bool esLaCorrecta = false;
    public GameLoop gameLoop;
    private Image i_boto;

    // Start is called before the first frame update
    void Start()
    {
        i_boto = GetComponent<Image>();

        ResetNormalColor();
    }

    public void ResetNormalColor()
    {
        if (i_boto != null) i_boto.color = ColorNormal;
    }

    public void ActualitzarColorBoto(bool saltar=true)
    {
        if (gameLoop.potContestar
            == true || saltar == true)
        {
            if (esLaCorrecta == true)
            {
                if (i_boto != null) i_boto.color = ColorEncertat;
            }
            else if (esLaCorrecta == false)
            {
                if (i_boto != null) i_boto.color = ColorFallo;
            }
        }
    }

    public void CheckResposta()
    {
        if (gameLoop.potContestar == true)
        {
            if (esLaCorrecta == false)
            {
                gameLoop.errorsTemporals++;
                if (gameLoop.errorsTemporals >= 2)
                {
                    StartCoroutine(gameLoop.CanviarDades(false));
                }
            }
            else if (esLaCorrecta == true)
            {
                StartCoroutine(gameLoop.CanviarDades(true));
            }
        }
    }
}
