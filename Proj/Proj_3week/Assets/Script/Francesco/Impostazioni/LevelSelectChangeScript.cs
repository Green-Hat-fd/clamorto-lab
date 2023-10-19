using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectChangeScript : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;

    [SerializeField] Button vuoto_notteStellataButton,
                            vuoto_grandeOndaButton,
                            vuoto_cittaCheSaleButton;
    [SerializeField] Button pieno_notteStellataButton,
                            pieno_grandeOndaButton,
                            pieno_cittaCheSaleButton;

    


    void Update()
    {
        //Cambia l'interagilit� dei pulsanti quando 
        vuoto_grandeOndaButton.interactable = stats_SO.GetIsLevelOneCompleted();
        vuoto_cittaCheSaleButton.interactable = stats_SO.GetIsLevelTwoCompleted();


        //Rende visibili o no se NON si � completato il livello
        vuoto_notteStellataButton.gameObject.SetActive(!stats_SO.GetIsLevelOneCompleted());
        vuoto_grandeOndaButton.gameObject.SetActive(!stats_SO.GetIsLevelTwoCompleted());
        vuoto_cittaCheSaleButton.gameObject.SetActive(!stats_SO.GetIsBossLevelCompleted());

        //Rende visibili o no se si � completato il livello
        pieno_notteStellataButton.gameObject.SetActive(stats_SO.GetIsLevelOneCompleted());
        pieno_grandeOndaButton.gameObject.SetActive(stats_SO.GetIsLevelTwoCompleted());
        pieno_cittaCheSaleButton.gameObject.SetActive(stats_SO.GetIsBossLevelCompleted());
    }
}
