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

    [Header("Dades Temporals")]
    public int errorsTemporals = 0;

    [Header("Dades Totals")]
    public int acertsTotals = 0;
    public int errorsTotals = 0;

    private Diccionaris diccionaris;
    

    // Start is called before the first frame update
    void Start()
    {
        diccionaris = GetComponent<Diccionaris>();

        StartCoroutine(SetupNovaPregunta());
    }

    IEnumerator SetupNovaPregunta()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < buttonRespostes.Length; i++)
        {
            buttonRespostes[i].GetComponent<FuncionamentBoto>().ResetNormalColor();
        }
        PosarNousValorsUI();
        yield return new WaitForSeconds(2);
    }

    public void CanviarDades(bool fetBe)
    {
        errorsTemporals = 0;
        if (fetBe == true) acertsTotals++;
        else if (fetBe == false) errorsTotals++;

        StartCoroutine(SetupNovaPregunta());
    }

    void PosarNousValorsUI()
    {
        //Primer calculem quin dels elements del diccionari anem a mostrar, fem un random entre el 0 i el maxim valor
        int i = Random.Range(0, diccionaris.d_primernivell.Count);
        titolQuanRespostes.text = diccionaris.d_primernivell[i].ToString();

        int posicioVerdadera = Random.Range(0, 2);

        //Fem el cas per si la resposta correcta es troba a la primera posició
        if (posicioVerdadera == 0)
        {
            //Primer de tot, fiquem la variable be a la UI i l'assignem com a la resposta correcta
            buttonRespostes[0].GetComponentInChildren<Text>().text = i.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = i;
            while (segonnombre == i)
            {
                segonnombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[1].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = i;
            while (tercernombre == i)
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
            buttonRespostes[1].GetComponentInChildren<Text>().text = i.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = i;
            while (segonnombre == i)
            {
                segonnombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = i;
            while (tercernombre == i)
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
            buttonRespostes[2].GetComponentInChildren<Text>().text = i.ToString();
            buttonRespostes[2].GetComponent<FuncionamentBoto>().esLaCorrecta = true;

            //En segon lloc, mirem quin numero posar a la segona posicio, aquest ha de ser diferent, ja que no es poden repetir.
            int segonnombre = i;
            while (segonnombre == i)
            {
                segonnombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[0].GetComponentInChildren<Text>().text = segonnombre.ToString();
            buttonRespostes[0].GetComponent<FuncionamentBoto>().esLaCorrecta = false;

            //Per últim seguim el mateix procediment que a la part anterior
            int tercernombre = i;
            while (tercernombre == i)
            {
                tercernombre = Random.Range(0, diccionaris.d_primernivell.Count);
            }

            buttonRespostes[1].GetComponentInChildren<Text>().text = tercernombre.ToString();
            buttonRespostes[1].GetComponent<FuncionamentBoto>().esLaCorrecta = false;
        }
    }
}
