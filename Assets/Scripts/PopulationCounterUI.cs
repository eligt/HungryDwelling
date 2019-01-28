using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationCounterUI : MonoBehaviour {

    public static int populationCount = 0;
    Text population;
    
	// Use this for initialization
	void Start () {
        population = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        population.text = "Population: " + populationCount;
	}
}
