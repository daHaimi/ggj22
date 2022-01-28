using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeededRandom : MonoBehaviour
{
    // Unity GlobalInstance Pattern
    public static SeededRandom instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    
    private int _seed;
    public int seed
    {
        get => _seed;

        set
        {
            _seed = value;
            Random.InitState(_seed);
        }
    }
}
