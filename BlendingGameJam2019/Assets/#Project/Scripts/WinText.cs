using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    public Text winText;
    public GameObject war;
    public GameObject disease;
    public GameObject techno;
    public GameObject alien;
    public GameObject ecosystem;
    // Start is called before the first frame update
    void Start()
    {
        string category = PlayerPrefs.GetString("win_category");
        switch (category.ToUpper()) {
        case "W": war.SetActive(true);break;
        case "D": disease.SetActive(true); break;
        case "E": ecosystem.SetActive(true); break;
        case "A": alien.SetActive(true); break;
        case "T": techno.SetActive(true); break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
