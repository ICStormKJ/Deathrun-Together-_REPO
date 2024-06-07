using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultInformation : MonoBehaviour
{
    [SerializeField] Image resultBg;
    private TMP_Text resultText;
    [SerializeField] TMP_Text eloText;
    private PlayerData data;
    private PlayerManager man;
    // Start is called before the first frame update
    void Start()
    {
        data = FindFirstObjectByType<PlayerData>();
        man = FindFirstObjectByType<PlayerManager>();
        SetUIElements();
    }

    void SetUIElements()
    {
        if (man.PlayerWinCondition())
        {
            resultText.text = "VICTORY!!";
            resultBg.color = Color.green;
        }
        else
        {
            resultText.text = "DEFEAT...";
            resultBg.color = Color.red;
        }
        int eloGained = EloCalculate.CalculateElo(data.rank, false, man.PlayerWinCondition());
        data.UpdateElo(data.elo +  eloGained);
        eloText.text = "Elo: " + data.elo;
    }

    
}
