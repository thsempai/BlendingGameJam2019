using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    public Text winText;
    // Start is called before the first frame update
    void Start()
    {
        string category = PlayerPrefs.GetString("win_category");
        switch (category.ToUpper()) {
        case "W": category = "Guerre";break;
        case "D": category = "Maladie";break;
        case "E": category = "Ecosystème";break;
        case "A": category = "Alien";break;
        case "T": category = "Technologie";break;
        }

        string text = "Catégorie gagnante: " + category;
        winText.text = text;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
