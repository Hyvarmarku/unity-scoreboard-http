using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable JSON score object obtained from server
[Serializable]
public class ScoreObjectHTTP {
    public string name;
    public int score;
}
