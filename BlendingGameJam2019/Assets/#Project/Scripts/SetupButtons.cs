using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupButtons : MonoBehaviour
{
    public AudioSource sound;
    private bool ok = false;
    public void Update() {
        if (ok && !sound.isPlaying) {

            SceneManager.LoadScene(1);
        }
    }

    public void SetupPlayer(int number)
    {
    sound.Play();
    PlayerPrefs.SetInt("players_number", number);
        ok = true;
}
}
