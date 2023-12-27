using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stats
{
    public int Value;
    public int Modifier{get{return (int)Mathf.Floor((Value-10)/2);}}
}
