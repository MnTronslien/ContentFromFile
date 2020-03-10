using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]

public class Behaviour : MonoBehaviour
{
    //state machine variables
    public enum State
    {
        Attacking, Idle, Dead, Moving
    }
    public State _defaultState = State.Idle;
    [NonSerialized]
    public State _state;
    [NonSerialized]
    public State _lastState = State.Idle;
    [NonSerialized]
    public bool _isEnteringState = false;

    //component refs
    [NonSerialized]
    public AudioSource _Speaker;
    [NonSerialized]
    public Animator _Animator;

    //Sounds
    //[Header("Sounds")]
    //public AudioClip _phaseloop;
    //[Header("Player Stats")] //if you are making a more complex this might be moved into a manager

    //other global variables
    public float _HitpointsMaximum = 100;
    public float _HitPointsCurrent = 100;
    public GameObject _HealthBar;
    [NonSerialized] public Vector3 _FullHealtBarSize;
    [NonSerialized] public GameObject _Target;
    [NonSerialized] public String _name;
    public TextMeshProUGUI _NameDisplay;
  


    //Setting up component references, Awake() is called before Start()
    virtual public void Awake()
    {
        _name = RandomNames.GetName();
        _state = _defaultState;
        _Speaker = GetComponent<AudioSource>();
        _Animator = GetComponentInChildren<Animator>();
        _FullHealtBarSize = _HealthBar.transform.localScale;
        
    }

    // Update is called once per frame
    virtual public void Update()
    {
        gameObject.name = _name + _state; //For debugging
        UpdateHealthBarGraphic();        
        if (GameStateManager._GameState == GameStateManager.GameState.Running)
        {
            DetectStateTransition();
            StateMachine();
            ResetStateTransition();
        }
    } //End Update

    private void UpdateHealthBarGraphic()
    {
        float newX = Mathf.Lerp(0, _FullHealtBarSize.x, _HitPointsCurrent / _HitpointsMaximum);
        _HealthBar.transform.localScale = new Vector3(newX, _FullHealtBarSize.y, _FullHealtBarSize.z);
    }
    private void DetectStateTransition()
    {
        if (_state != _lastState) //detect state transition
        {
            _isEnteringState = true;
            _lastState = _state;
        }
    }
    virtual public void StateMachine()
    {
        //Finite State Machine -------------------------------------------------------------------------------------------
        //If you do not know what a state machine is, the basic concept is 
        //that you organize the behaviour into certain states that decribe what the player (or enmy or whatever) may do.
        //The clue here is that each state is *inclusive* and not exlusive in the options of behaviour.
        //An example of this is that "jumping" is possible both while idle and moving. 
        //Instead of making specialized bahaviour for jumping and idle, we just allow both states to call the same method and transition into jumping state.


        //Idle state
        if (_state == State.Idle)
        {
            if (_HitPointsCurrent <= 0) ChangeState(State.Dead);
        }
        if (_state == State.Attacking)
        {
            if (_HitPointsCurrent <= 0) ChangeState(State.Dead);
        }
        if (_state == State.Dead)
        {

        }
        if (_state == State.Moving)
        {
            if (_HitPointsCurrent <= 0) ChangeState(State.Dead);
        }
        
    }
    private void ResetStateTransition()
    {
        _isEnteringState = false; //reset entering state to false
    }


    public virtual void ChangeState(State newState)
    {
        _state = newState;
    }

    public State GetState() { return _state; }

    public virtual void HardReset()
    {
        _NameDisplay.text = RandomNames.GetName();
        _HitPointsCurrent = _HitpointsMaximum;
        ChangeState(_defaultState);
    }

    public void TakeDamage(float damageValue)
    {
        _HitPointsCurrent -= damageValue;
        Debug.Log(gameObject.name + " took " + damageValue + " points of damage!");

    }
    public void DealDamage(Behaviour target, float amount)
    {
        target.TakeDamage(amount);
        Debug.Log(gameObject.name + " attacked " + target.name + " for " + amount + " damage!");
    }

}
