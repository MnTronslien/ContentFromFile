using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


//this is a helper class for parsing csv files
public static class CSVParser : object
{
    public static float[] LoadFromFileWithHeader(string Filename)
    {
        string[] content = File.ReadAllLines(Application.streamingAssetsPath + "/" + Filename);
        return Array.ConvertAll(content[1].Split(','), float.Parse);
    }


}
