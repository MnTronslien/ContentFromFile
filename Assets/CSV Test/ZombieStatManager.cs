using System;
using System.IO;
using UnityEngine;

public class ZombieStatManager : MonoBehaviour
{

    public static string _statFile = "ZombieStats";
    public static TMPro.TextMeshProUGUI _debug;

    public SourceDataFormat _ReadFrom = SourceDataFormat.CSV;

    public enum SourceDataFormat
    {
        CSV, JSon
    }

    [Serializable]
    public class Stats
    {
        public float _WalkSpeed = 0;
        public float _AttackSpeed = 0;
        public float _AttackDamage = 0;
        public float _Hitpoints = 0;
    }

    public static Stats _Stats;
    public static ZombieStatManager _ZombieStatManager;
    // Use this for initialization
    void Start()
    {
        _ZombieStatManager = this;
        _Stats = new Stats();
        if (_debug == null)
        {
            _debug = GameObject.Find("Debug").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GenerateExampleJsonFile();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _ReadFrom = _ReadFrom == SourceDataFormat.CSV ? _ReadFrom = SourceDataFormat.JSon : _ReadFrom = SourceDataFormat.CSV;
            //Above line is a fancy way of saying: When "s" is pressed, is _readFrom file of type CSV? If yes, then set it to JSOn,if not set it to CSV. it is a way to toggle between the two values.
        }
    }

    public void UpdateStats()
    {
        if (_ReadFrom == SourceDataFormat.CSV)
        {
            Debug.Log("Loading Stats");
            float[] stats = CSVParser.LoadFromFileWithHeader(_statFile + ".csv"); //CSV parser is a script i made for this example
            _Stats._WalkSpeed = stats[0];
            _Stats._AttackSpeed = stats[1];
            _Stats._AttackDamage = stats[2];
            _Stats._Hitpoints = stats[3];
        }
        if (_ReadFrom == SourceDataFormat.JSon)
        {
            string filepath = Application.streamingAssetsPath + "/" + _statFile + ".json";
            bool b = File.Exists(filepath);
            if (!b)
            {
                Debug.LogError("JSON Stat file did not exist!");
                GenerateExampleJsonFile();
            }

            string json = File.ReadAllText(filepath);
            _Stats = JsonUtility.FromJson<Stats>(json);
        }

        string debugtext = string.Format(
            "Stats for current Zombie:\nWalkspeed:  {0}\nAttackspeed:  {1}\nAttackDamage:  {2}\nHitPoints:  {3}\nSourcefile:   {4}",
    _Stats._WalkSpeed, _Stats._AttackSpeed, _Stats._AttackDamage, _Stats._Hitpoints, _ReadFrom.ToString());
        Debug.Log("WALk SPEED Right After READING FROM FILE IS: " + _Stats._WalkSpeed);
        _debug.text = debugtext;
    }

    /// <summary>
    /// This method is just for generating the file that you then later play around with. It should not be needed again unless you expand the stats class and want to regenerate an example stat file
    /// </summary>
    public void GenerateExampleJsonFile()
    {
        //Sombie File
        string filepath = Application.streamingAssetsPath + "/" + _statFile + ".json";
        if (!File.Exists(filepath))
        {
            string json = JsonUtility.ToJson(_Stats, true);
            //Write some text to the file
            StreamWriter writer = new StreamWriter(filepath, true);

            writer.WriteLine(json);
            writer.Close();
        }
    }



}
