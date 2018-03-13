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

	// Use this for initialization
	void Awake () {
        _GameStateManager = this;

    }
    private void Start()
    {
        Reset();
        _GameState = GameState.Running;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            ZombieStatManager.UpdateStats();
            Reset();
            _GameState = GameState.Running;
        }
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
