using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class RandomNames
{
    private static NameList _list;

    public static string GetName()
    {
        UpdateNameList();
        return  _list.Names[Random.Range(0, _list.Names.Length)];
    }

    public static void UpdateNameList()
    {
        string filepath = Application.streamingAssetsPath + "/" + "Names.json";
        if (!File.Exists(filepath))
        {
            Debug.Log("Name list did not exist, generating a new new one from default values");
            NameList exampleNames = new NameList
            {
                Names = new string[] { "Timmy", "Kevin", "Thomas", "Kai", "Benny", "Roy-Arne", "Harald", "Lars" , "Jason"}
            };

            //Write some text to the file
            StreamWriter writer = new StreamWriter(filepath, true);
            writer.WriteLine(JsonUtility.ToJson(exampleNames, true));
            writer.Close();
        }


        string json = File.ReadAllText(filepath);
        _list = JsonUtility.FromJson<NameList>(json);
    }
    [System.Serializable]
    public class NameList
    {
        public string[] Names;
    }
}
