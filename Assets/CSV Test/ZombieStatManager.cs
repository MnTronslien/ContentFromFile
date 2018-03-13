using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatManager : MonoBehaviour
{

    public static string _statFile = "ZombieStats.csv";
    public static TMPro.TextMeshProUGUI _debug;

    public class Stats
    {
        public float _Walkspeed = 0;
        public float _Attackspeed = 0;
        public float _AttackDamage = 0;
        public float _Hitpoints = 0;
    }

    public static Stats _stats;
    public static ZombieStatManager _ZombieStatManager;
    // Use this for initialization
    void Start()
    {
        _ZombieStatManager = this;
        _stats = new Stats();
        if (_debug == null)
        {
            _debug = GameObject.Find("Debug").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void UpdateStats()
    {
            Debug.Log("Loading Stats");
            float[] stats = CSVParser.LoadFromFileWithHeader(_statFile);
            _stats._Walkspeed = stats[0];
            _stats._Attackspeed = stats[1];
            _stats._AttackDamage = stats[2];
            _stats._Hitpoints = stats[3];

            string debugtext = "Stats for current Zombie\n" +
                "Walkspeed: " + stats[0] + "\n" +
                "Attackspeed: " + stats[1] + "\n" +
                "AttackDamage: " + stats[2] + "\n" +
                "HitPoints: " + stats[3];
            _debug.text = debugtext;
    }
}
