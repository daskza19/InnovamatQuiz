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
    public Text numeroAcertades;
    public Text numeroFallades;
    public Text numeroNivell;

    [Header("Dades Temporals")]
    public bool potContestar = false;
    public int errorsTemporals = 0;
    public int anteriorNumero = 0;
    public int actualNumero = 0;
    [Range(1,3)]
    public int nivell = 0;

    [Header("Dades Totals")]
    public int acertsTotals = 0;
    public int errorsTotals = 0;


    [Header("Animadors")]
    public Animator APantallaPreguntes;
    public Animator APantallaEnunciat;
    public Animator APantallaMenu;
    public Animator APantallaNivells;

    private Diccionaris diccionaris;

    // Start is called before the first frame update
    void Start()
    {
        diccionaris = GetComponent<Diccionaris>();

        acertsTotals = 0;
        errorsTotals = 0;

        UpdateUIAcceptadesFalladesNivell();
        APantallaMenu.SetBool("Entrant", true);
    }

    //Funció per passar del menu a la pantalla de selecció de nivell
    public void PlayNivell()
    {
        StartCoroutine(MainMenuToNivell());
    }

    //Rutina que s'utilitza dintre de la funcio PlayNivell per organitzar les animacions de canvi de pantalla
    IEnumerator MainMenuToNivell()
    {
        APantallaMenu.SetBool("Entrant", false);
        APantallaMenu.SetBool("Sortint", true);
        yield return new WaitForSeconds(2);
        APantallaMenu.SetBool("Sortint", false);
        APantallaNivells.SetBool("Entrant", true);
        yield return new WaitForSeconds(3);
        potContestar = true;
    }

    //Funció per passar de la pantalla de nivell a la del enunciat de la pregunta
    public void NivellToEnunciat()
    {
        if (potContestar == true)
        {
            StartCoroutine(C_NivellToEnunciat());
        }
    }

    //Rutina que s'utilitza dintre de la funcio NivellToEnunciat per organitzar les animacions de canvi de pantalla
    IEnumerator C_NivellToEnunciat()
    {
        potContestar = false;
        APantallaNivells.SetBool("Entrant", false);
        APantallaNivells.SetBool("Sortint", true);
        yield return new WaitForSeconds(2);
        APantallaNivells.SetBool("Sortint", false);

        StartCoroutine(SetupEnunciat());
    }

    public void TornarAMenuPrincipal()
    {
        if (potContestar == true)
        {
            potContestar = false;
            StartCoroutine(PreguntesToMainMenu());
        }
    }

    IEnumerator PreguntesToMainMenu()
    {
        APantallaPreguntes.SetBool("Sortint", true);
        yield return new WaitForSeconds(2);
        APantallaPreguntes.SetBool("Sortint", false);
        APantallaMenu.SetBool("Entrant", true);

        yield return new WaitForSeconds(2);
        potContestar = true;
    }

    IEnumerator SetupEnunciat()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<FuncionamentBoto>().ResetNormalColor();
        }
        PosarNousValorsUI();
        APantallaEnunciat.SetBool("Entrant", true);
        yield return new WaitForSeconds(2);
        APantallaEnunciat.SetBool("Entrant", false);
        yield return new WaitForSeconds(2.5f);
        APantallaEnunciat.SetBool("Sortint", true);
        yield return new WaitForSeconds(2.5f);
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
            UpdateUIAcceptadesFalladesNivell();
        }
        else if (fetBe == false)
        {
            errorsTotals++;
            UpdateUIAcceptadesFalladesNivell();
            EnsenyarTotesRespostes();
        }

        APantallaPreguntes.SetBool("Sortint", true);
        //Actualitzem els contadors de la UI
        yield return new WaitForSeconds(4);
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

    void UpdateUIAcceptadesFalladesNivell()
    {
        numeroAcertades.text = acertsTotals.ToString();
        numeroFallades.text = errorsTotals.ToString();
        numeroNivell.text = nivell.ToString();
    }

    void PosarNousValorsUI()
    {
        anteriorNumero = actualNumero;
        Dictionary<int, string> diccionariUtilitzat = GetActualDiccionari();

        //Primer calculem quin dels elements del diccionari anem a mostrar, fem un random entre el 0 i el maxim valor
        while (actualNumero == anteriorNumero)
        {
            actualNumero = Random.Range(0, diccionariUtilitzat.Count);
        }
        titolQuanRespostes.text = diccionariUtilitzat[actualNumero].ToString();
        titolEnunciat.text = diccionariUtilitzat[actualNumero].ToString();

        int posicioVerdadera = Random.Range(0, 3);

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
                segonnombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            buttonRespostes[1].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionariUtilitzat.Count);
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
                segonnombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionariUtilitzat.Count);
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
                segonnombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            buttonRespostes[1].GetComponentInChildren<Text>().text = tercernombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;
        }
    }

    public void ActualitzarNivell(int _nounivell)
    {
        if (potContestar == true)
        {
            nivell = _nounivell;
            UpdateUIAcceptadesFalladesNivell();
        }
    }

    Dictionary<int, string> GetActualDiccionari()
    {
        Dictionary<int, string> _diccionari = new Dictionary<int, string>();

        switch (nivell)
        {
            case (1):
                _diccionari = diccionaris.d_primernivell;
                break;
            case (2):
                _diccionari = diccionaris.d_segonnivell;
                break;
            case (3):
                _diccionari = diccionaris.d_tercernivell;
                break;
            default:
                _diccionari = diccionaris.d_primernivell;
                break;
        }
        return _diccionari;
    }
}
