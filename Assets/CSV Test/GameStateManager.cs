using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public static GameStateManager _GameStateManager;
    public enum GameState
    {
        Idle, Running
    }
    public static GameState _GameState = GameState.Idle;
    //Refs
    public ZombieBehaviour _Zombie;
    public PlayerBehaviour _Player;

    public static TMPro.TextMeshProUGUI _clock;
    public float _TimeOfLatestGameStart;

    // Use this for initialization
    void Awake () {
        _GameStateManager = this;
        _clock = GameObject.Find("Clock").GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Start()
    {
        Reset();
        _GameState = GameState.Idle;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            ZombieStatManager.UpdateStats();
            Reset();
            _GameState = GameState.Idle;
            Invoke("RunGame",1);
        }
        if (_GameState == GameState.Running) UpdateClock();
        if (_Player.GetState() == Behaviour.State.Dead)
        {
            _Zombie.HardReset();
            _GameState = GameState.Idle;
        }
	}

    private void RunGame()
    {
        _GameState = GameState.Running;
        _TimeOfLatestGameStart = Time.time;
    }
    void UpdateClock()
    {
        float timer = Time.time - _TimeOfLatestGameStart;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        int milliseconds = (int)(timer * 1000f) % 1000;
        //string niceTime = string.Format("{0:00}:{1:00}:{}", minutes, seconds, milliseconds);
        _clock.text = minutes.ToString("D2") + "::" + seconds.ToString("D2") + "::" + milliseconds.ToString("D3");
        //_clock.text = niceTime;
    }
    private void Reset()
    {
        _Zombie.HardReset();
        _Player.HardReset();

    }
    private void StartGame()
    {

    }

}
