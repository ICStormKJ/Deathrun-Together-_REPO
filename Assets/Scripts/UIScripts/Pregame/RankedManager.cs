using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankedManager: MonoBehaviour
{
    [SerializeField] private TMP_Text eloNumber;
    [SerializeField] private Image rankImage;
    [SerializeField] Sprite[] iconSprites;
    //  0 - Fledgeling | 1 - Apprentice | 2 - Advanced | 3 - Expert | 4 - Speedster | 5 - Stunter | 6 - Master | 7 - Godspeed

    private PlayerData data;
    // Start is called before the first frame update
    void Awake()
    {
        data = FindFirstObjectByType<PlayerData>();
        setRankIcon(data.elo);
        eloNumber.text = data.rank.ToString();
    }

    private void setRankIcon(int elo)
    {
        if (elo >= 1500)
            rankImage.sprite = iconSprites[7];
        else if (elo >= 1100)
            rankImage.sprite = iconSprites[6];
        else if (elo >= 800)
            rankImage.sprite = iconSprites[5];
        else if (elo >= 600)
            rankImage.sprite = iconSprites[4];
        else if (elo >= 400)
            rankImage.sprite = iconSprites[3];
        else if (elo >= 250)
            rankImage.sprite = iconSprites[2];
        else if (elo >= 100)
            rankImage.sprite = iconSprites[1];
        else
            rankImage.sprite = iconSprites[0];
    }

    public void StartMatchmaking()
    {

    }

    
}
