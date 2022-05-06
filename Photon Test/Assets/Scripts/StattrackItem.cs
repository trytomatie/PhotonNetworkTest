using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StattrackItem
{
    public string playerName;
    public int kills;
    public int deaths;
    public int assists;
    public int dammageDealt;

    public StattrackItem(string name)
    {
        this.playerName = name;
    }
}
