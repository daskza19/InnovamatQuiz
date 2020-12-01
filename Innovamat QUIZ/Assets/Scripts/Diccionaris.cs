using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diccionaris : MonoBehaviour
{
    public Dictionary<int, string> d_primernivell = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Awake()
    {
        d_primernivell.Add(0, "zero");
        d_primernivell.Add(1, "un");
        d_primernivell.Add(2, "dos");
        d_primernivell.Add(3, "tres");
        d_primernivell.Add(4, "quatre");
        d_primernivell.Add(5, "cinc");
        d_primernivell.Add(6, "sis");
        d_primernivell.Add(7, "set");
        d_primernivell.Add(8, "vuit");
        d_primernivell.Add(9, "nou");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
