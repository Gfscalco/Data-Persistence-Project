using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    private TextMeshProUGUI names;
    private TextMeshProUGUI scores;
    // Start is called before the first frame update
    void Start()
    {
        names = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        scores = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        names.text = "";
        scores.text = "";
        for (int i = 0; i < DataManager.Instance.highscore.Length; i++)
        {
           names.text += (i + 1) + ". " + DataManager.Instance.names[i] + ":<br>";
           scores.text += DataManager.Instance.highscore[i] + "<br>";
        }
    }

}
