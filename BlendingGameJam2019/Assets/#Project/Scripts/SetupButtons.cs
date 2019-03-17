using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupButtons : MonoBehaviour
{
    public void SetupPlayer(int number)
    {
        PlayerPrefs.SetInt("players_number", number);
        SceneManager.LoadScene(1);
    }
}
