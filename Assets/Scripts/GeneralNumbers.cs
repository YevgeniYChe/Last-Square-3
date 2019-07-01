using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNumbers : MonoBehaviour
{
    public int minGridSize = 5;
    public int maxGridSize = 10;
    public int minNumberOfUnits = 1;
    public int maxNumberOfUnits = 5;
    public int gridSize;
    public int numberOfUnits;

    public enum PossibleGameMode {normal, toTarget};
    public PossibleGameMode gameMode = PossibleGameMode.normal;

    void Awake()
    {
        gridSize = Random.Range(minGridSize, maxGridSize);
        numberOfUnits = Random.Range(minNumberOfUnits, maxNumberOfUnits);
    }

  
}
