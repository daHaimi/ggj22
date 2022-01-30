using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class StartupUI : MonoBehaviour
{
    public TextMeshProUGUI seedInput;
    public Button startButton;
    
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(ButtonClick);
    }

    // Update is called once per frame
    void ButtonClick()
    {
        int seed;
        var seedStr = seedInput.GetParsedText();
        if (seedStr.Contains("[Random]"))
        {
            seed = new Random(Guid.NewGuid().GetHashCode()).Next();
        }
        else
        {
            int.TryParse(seedStr, out seed);
        }
        
    }
}
