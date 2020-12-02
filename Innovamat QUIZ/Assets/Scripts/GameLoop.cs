using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject []buttonRespostes;    //Inicialitzem l'array de botons (opcions disponibles)
    public Text titolQuanRespostes;         //Creem la variable que modifica el titol a la pantalla de les opcions. Es fica el nom del número
    public Text titolEnunciat;              //Creem la variable que modifica el text de la paraula que surt abans de la pantalla de les opcions
    public Text numeroAcertades;            //Creem la variable que influeix amb el text del contador de dalt a la dreta dels encerts
    public Text numeroFallades;             //Creem la variable que influeix amb el text del contador de dalt a la dreta de les errades
    public Text numeroNivell;               //Creem la variable que influeix amb el numero que hi ha sota els contadors que mostra amb quin nivell estem jugant

    [Header("Dades Temporals")]
    public bool potContestar = false;       //Aquesta variable indica si el jugador pot interaccionar amb la UI o no
    public int errorsTemporals = 0;         //Aquesta variable mira quants errors s'ha comès durant el torn. Si arriba a 2 es marca com a error
    public int anteriorNumero = 0;          //Aquesta variable guarda el número que ha sortit anteriorment per no repetir numero dos torns seguits
    public int actualNumero = 0;            //Aquesta variable indica sobre quin número estem al torn actual
    [Range(1,3)]
    public int nivell = 0;                  //Aquesta variable indica a quin nivell ens trobem actualment

    [Header("Dades Totals")]
    public int encertsTotals = 0;           //Marcador que indica quants encerts totals tenim a la partida
    public int erradesTotals = 0;           //Marcador que indica quantes errades totals tenim a la partida


    [Header("Animadors")]
    public Animator APantallaPreguntes;     //Animador per la pantalla principal de les preguntes
    public Animator APantallaEnunciat;      //Animador per la pantalla on es veu el número escrit paraula abans de la pantalla de les preguntes
    public Animator APantallaMenu;          //Animador per la pantalla del menu principal
    public Animator APantallaNivells;       //Animador per la pantalla on es selecciona el nivell de joc

    private Diccionaris diccionaris;

    // Start is called before the first frame update
    void Start()
    {
        diccionaris = GetComponent<Diccionaris>();

        //S'inicialitzen els contadors i s'actualitza la UI
        encertsTotals = 0;
        erradesTotals = 0;
        UpdateUIAcceptadesFalladesNivell();

        //S'entra per primer cop al menu
        APantallaMenu.SetBool("Entrant", true);
    }

    //Rutina que s'utilitza per mostrar la pantalla de l'enunciat de la ronda
    IEnumerator SetupEnunciat()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<FuncionamentBoto>().ResetNormalColor();
        }
        //Es creen uns nous valors i s'assignen a la UI
        PosarNousValorsUI();
        //Es posa l'animació d'entrada (que dura uns 2 segons)
        APantallaEnunciat.SetBool("Entrant", true);
        yield return new WaitForSeconds(2);
        //Es manté el text al centre de la pantalla durant 2 segons i mig
        APantallaEnunciat.SetBool("Entrant", false);
        yield return new WaitForSeconds(2.5f);
        //Es posa l'animació de sortida
        APantallaEnunciat.SetBool("Sortint", true);
        yield return new WaitForSeconds(2.5f);
        APantallaEnunciat.SetBool("Sortint", false);
        //S'entra a la rutina de la pantalla on surten les opcions
        StartCoroutine(SetupNovaPregunta());
    }

    //Rutina que s'utilitza per mostrar la pantalla amb les diferentes opcions
    IEnumerator SetupNovaPregunta()
    {
        //Es posa l'animació d'entrada a la pantalla
        APantallaPreguntes.SetBool("Entrant", true);
        yield return new WaitForSeconds(2);
        potContestar = true;
        APantallaPreguntes.SetBool("Entrant", false);
        //Es fa que els botons siguin operatius
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<Button>().interactable = true;
        }
    }

    //Rutina que s'utilitza per posar be les dades i després tornar a fer el bucle de la pregunta
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
            encertsTotals++;
            UpdateUIAcceptadesFalladesNivell();
        }
        else if (fetBe == false)
        {
            erradesTotals++;
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

    //Funció que ensenya immediatament totes les respostes de les tres opcions
    void EnsenyarTotesRespostes()
    {
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<FuncionamentBoto>().ActualitzarColorBoto(true);
        }
    }

    //Funció que actualitza els valors dels panels de encerts, errats i el nivell actual (UI)
    void UpdateUIAcceptadesFalladesNivell()
    {
        numeroAcertades.text = encertsTotals.ToString();
        numeroFallades.text = erradesTotals.ToString();
        numeroNivell.text = nivell.ToString();
    }

    //Funció que crea nous valors i els assigna a la UI
    void PosarNousValorsUI()
    {
        //L'anterior número guarda el número de la ronda passada
        anteriorNumero = actualNumero;
        Dictionary<int, string> diccionariUtilitzat = GetActualDiccionari();

        //Primer calculem quin dels elements del diccionari anem a mostrar, fem un random entre el 0 i el maxim valor
        //Per no repetir numero fem que faci un bucle fins que no sigui el mateix que la ronda passada
        while (actualNumero == anteriorNumero)
        {
            actualNumero = Random.Range(0, diccionariUtilitzat.Count);
        }
        //S'inicialitzen els titols de la UI
        titolQuanRespostes.text = diccionariUtilitzat[actualNumero].ToString();
        titolEnunciat.text = diccionariUtilitzat[actualNumero].ToString();

        //Es randomitza la posició de la resposta vertadera
        int posicioVerdadera = Random.Range(0, 3);

        //Fem el cas per si la resposta correcta es troba a la primera posició
        if (posicioVerdadera == 0)
        {
            //Primer de tot, fiquem la variable be a la UI i l'assignem com a la resposta correcta
            buttonRespostes[0].GetComponentInChildren<Text>().text = actualNumero.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = actualNumero;
            //Es fa un bucle per que el numero que es generi aleatoriament sigui diferent als altres
            while (segonnombre == actualNumero)
            {
                segonnombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            //Es posen els valors a la UI
            buttonRespostes[1].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            //Es fa un bucle per que el numero que es generi aleatoriament sigui diferent als altres
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            //Es posen els valors a la UI
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
            //Es fa un bucle per que el numero que es generi aleatoriament sigui diferent als altres
            while (segonnombre == actualNumero)
            {
                segonnombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            //Es posen els valors a la UI
            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            //Es fa un bucle per que el numero que es generi aleatoriament sigui diferent als altres
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            //Es posen els valors a la UI
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
            //Es fa un bucle per que el numero que es generi aleatoriament sigui diferent als altres
            while (segonnombre == actualNumero)
            {
                segonnombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            //Es posen els valors a la UI
            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = actualNumero;
            //Es fa un bucle per que el numero que es generi aleatoriament sigui diferent als altres
            while (tercernombre == actualNumero || tercernombre == segonnombre)
            {
                tercernombre = Random.Range(0, diccionariUtilitzat.Count);
            }

            //Es posen els valors a la UI
            buttonRespostes[1].GetComponentInChildren<Text>().text = tercernombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;
        }
    }

    //Funció que actualitza el nivell
    public void ActualitzarNivell(int _nounivell)
    {
        if (potContestar == true)
        {
            nivell = _nounivell;
            UpdateUIAcceptadesFalladesNivell();
        }
    }

    //Funció que, depenent el nivell actual, retorna un diccionari per utilitzar a la ronda
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

    //Funció per passar del menu a la pantalla de selecció de nivell
    public void MainMenuToNivell()
    {
        StartCoroutine(C_MainMenuToNivell());
    }

    //Rutina que s'utilitza dintre de la funcio MainMenuToNivell per organitzar les animacions de canvi de pantalla
    IEnumerator C_MainMenuToNivell()
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

    //Funció per passar de la pantalla de les preguntes fins a la pantalla de Main Menu
    public void PreguntesToMainMenu()
    {
        if (potContestar == true)
        {
            StartCoroutine(C_PreguntesToMainMenu());
        }
    }

    //Rutina que s'utilitza dintre de la funcio PreguntesToMainMenu per organitzar les animacions de canvi de pantalla
    IEnumerator C_PreguntesToMainMenu()
    {
        //Tornem a iniciar els contadors a 0
        encertsTotals = 0;
        erradesTotals = 0;
        potContestar = false;
        APantallaPreguntes.SetBool("Sortint", true);
        yield return new WaitForSeconds(2);
        APantallaPreguntes.SetBool("Sortint", false);
        APantallaMenu.SetBool("Entrant", true);

        yield return new WaitForSeconds(2);
        potContestar = true;
    }
}
