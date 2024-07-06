using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Artifact
{
    public string gptPrompt { get; set; }
    public string imageName { get; set; }
    public string imageURL { get; set; }
    public string originalPrompt { get; set; }
}
