using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


/// <summary>
/// This is a helper class for parsing csv files.
/// It is a staic class, meaning that i don't need a reference to an object of this type to acces its methods from other parts of my code
/// </summary>
public static class CSVParser : object
{
    public static float[] LoadFromFileWithHeader(string Filename)
    {
        string[] content = File.ReadAllLines(Application.streamingAssetsPath + "/" + Filename);
        return Array.ConvertAll(content[1].Split(','), float.Parse);
    }


}
