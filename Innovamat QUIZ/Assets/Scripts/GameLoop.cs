using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject []buttonRespostes;
    public Text titolQuanRespostes;
    public Text titolEnunciat;
    public Text titolPreviaEnunciat;
    public Text numeroAcertades;
    public Text numeroFallades;

    [Header("Dades Temporals")]
    public bool potContestar = false;
    public int errorsTemporals = 0;
    public int anteriorNumero = 0;
    public int actualNumero = 0;

    [Header("Dades Totals")]
    public int acertsTotals = 0;
    public int errorsTotals = 0;


    [Header("Animadors")]
    public Animator APantallaPreguntes;
    public Animator APantallaEnunciat;
    public Animator APantallaMenu;

    private Diccionaris diccionaris;

    // Start is called before the first frame update
    void Start()
    {
        diccionaris = GetComponent<Diccionaris>();

        UpdateUIAcertadesFallades();
        StartCoroutine(SetupEnunciat());
    }

    IEnumerator SetupEnunciat()
    {
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<FuncionamentBoto>().ResetNormalColor();
        }
        PosarNousValorsUI();
        yield return new WaitForSeconds(2);
        APantallaEnunciat.SetBool("Entrant", true);
        yield return new WaitForSeconds(2);
        APantallaEnunciat.SetBool("Entrant", false);
        yield return new WaitForSeconds(2);
        APantallaEnunciat.SetBool("Sortint", true);
        yield return new WaitForSeconds(2);
        APantallaEnunciat.SetBool("Sortint", false);
        StartCoroutine(SetupNovaPregunta());
    }

    IEnumerator SetupNovaPregunta()
    {
        APantallaPreguntes.SetBool("Entrant", true);
        yield return new WaitForSeconds(2);
        potContestar = true;
        APantallaPreguntes.SetBool("Entrant", false);
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<Button>().interactable = true;
        }
    }

    public IEnumerator CanviarDades(bool fetBe)
    {
        potContestar = false;
        //Primer fem que els botons no siguin interactuables per a que el jugador no pugui ficar una nova resposta
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<Button>().interactable = false;
        }

        //Després posem els errors temporals a 0, ja que aquest es reinicien a cada ronda
        errorsTemporals = 0;

        //Incrementem els dos contadors totals, per la part dels errors comencem una rutina per ensenyar totes les respostes
        yield return new WaitForSeconds(2);
        if (fetBe == true)
        {
            acertsTotals++;
            UpdateUIAcertadesFallades();
        }
        else if (fetBe == false)
        {
            errorsTotals++;
            UpdateUIAcertadesFallades();
            EnsenyarTotesRespostes();
        }

        APantallaPreguntes.SetBool("Sortint", true);
        //Actualitzem els contadors de la UI
        yield return new WaitForSeconds(3);
        APantallaPreguntes.SetBool("Sortint", false);

        //Tornem a començar la rutina
        StartCoroutine(SetupEnunciat());
    }

    void EnsenyarTotesRespostes()
    {
        
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<FuncionamentBoto>().ActualitzarColorBoto(true);
        }
    }

    void UpdateUIAcertadesFallades()
    {
        numeroAcertades.text = acertsTotals.ToString();
        numeroFallades.text = errorsTotals.ToString();
    }

    void PosarNousValorsUI()
    {
        anteriorNumero = actualNumero;
        //Primer calculem quin dels elements del diccionari anem a mostrar, fem un random entre el 0 i el maxim valor
        while (actualNumero == anteriorNumero)
        {
            actualNumero = Random.Range(0, diccionaris.d_primernivell.Count);
        }
        titolQuanRespostes.text = diccionaris.d_primernivell[actualNumero].ToString();
        titolEnunciat.text = diccionaris.d_primernivell[actualNumero].ToString();

        int posicioVerdadera = Random.Range(0, 2);

        //Fem el cas per si la resposta correcta es troba a la primera posició
        if (posicioVerdadera == 0)
        {
            //Primer de tot, fiquem la variable be a la UI i l'assignem com a la resposta correcta
            buttonRespostes[0].GetComponentInChildren<Text>().text = actualNumero.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = actualNumero;
            while (segonnombre == actualNumero)
            {
                segonnombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[1].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[2].GetComponentInChildren<Text>().text = tercernombre.ToString();
            buttonRespostes[2].GetComponent<FuncionamentBoto>().esLaCorrecta = false;
        }

        //Fem el cas per si la resposta correcta es troba a la primera posició
        if (posicioVerdadera == 1)
        {
            //Primer de tot, fiquem la variable be a la UI i l'assignem com a la resposta correcta
            buttonRespostes[1].GetComponentInChildren<Text>().text = actualNumero.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = actualNumero;
            while (segonnombre == actualNumero)
            {
                segonnombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[2].GetComponentInChildren<Text>().text = tercernombre.ToString();
            buttonRespostes[2].GetComponent<FuncionamentBoto>().esLaCorrecta = false;
        }

        //Fem el cas per si la resposta correcta es troba a la primera posició
        if (posicioVerdadera == 2)
        {
            //Primer de tot, fiquem la variable be a la UI i l'assignem com a la resposta correcta
            buttonRespostes[2].GetComponentInChildren<Text>().text = actualNumero.ToString();
            buttonRespostes[2].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = actualNumero;
            while (segonnombre == actualNumero)
            {
                segonnombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[1].GetComponentInChildren<Text>().text = tercernombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;
        }
    }
}
