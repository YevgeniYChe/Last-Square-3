using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public GameObject gameManager;
    private GeneralNumbers genNumbers;

    void Awake()
    {
        genNumbers = gameManager.GetComponent<GeneralNumbers>();
    }

    public void ChangeText()
    {
        Text TextButton = GetComponentInChildren<Text>();

        if(genNumbers.gameMode == GeneralNumbers.PossibleGameMode.normal)
        {
            TextButton.text = "Let`s play";
        }
        else if(genNumbers.gameMode == GeneralNumbers.PossibleGameMode.toTarget)
        {
            TextButton.text = "Return to the free walking";
        }

    }
}
