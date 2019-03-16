using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupButtons : MonoBehaviour
{
    public void SetupPlayer(int number)
    {
        PlayerPrefs.SetInt("players_number", number);
    }
}
