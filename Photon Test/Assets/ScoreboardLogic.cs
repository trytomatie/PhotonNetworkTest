using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardLogic : MonoBehaviour
{
    public GameObject scoreboard;
    public TextMeshProUGUI playersText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI deathText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            playersText.text = "";
            deathText.text = "";
            killText.text = "";
            foreach (int id in Stattrack.instance.players.Keys)
            {
                playersText.text += Stattrack.instance.players[id].playerName + "\n";
                killText.text += Stattrack.instance.players[id].kills + "\n";
                deathText.text += Stattrack.instance.players[id].deaths + "\n";
            }
        }
        else
        {
            scoreboard.SetActive(false);
        }
    }
}
