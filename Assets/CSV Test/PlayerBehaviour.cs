using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]

public class PlayerBehaviour : MonoBehaviour
{
    //state machine variables
    public enum State
    {
        Attacking, Idle, Dead
    }
    private State _state = State.Idle;
    private State _lastState = State.Idle;
    private bool _isEnteringState = false;

    //component refs
    private AudioSource _Speaker;
    private Animator _Animator;


    //Sounds
    //[Header("Sounds")]
    //public AudioClip _phaseloop;
    //[Header("Player Stats")] //if you are making a more complex this might be moved into a manager

    //other global variables
    public GameObject _Zombie;
    public float _HitpointsMaximum = 100;
    public float _HitPointsCurrent = 100;
    public GameObject _HealthBar;
    private Vector3 _FullHealtBarSize;

    public Transform _emitterLeft, _emitterRight, _waveAttack;

    //Setting up component references, Awake() is called before Start()
    private void Awake()
    {
        _Speaker = GetComponent<AudioSource>();
        _Animator = GetComponentInChildren<Animator>();
        //if (!GetComponentInChildren<Animation>())
        //{
        //    Debug.LogError("NO ANIMATON FOUND FOR ZOMBIE!");
        //}
        if (_Zombie == null)
        {
            _Zombie = GameObject.Find("Player");
        }
        _FullHealtBarSize = _HealthBar.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.name = "Player " + _state; //For debugging

        float newX = Mathf.Lerp(0, _FullHealtBarSize.x, _HitPointsCurrent / _HitpointsMaximum);
        _HealthBar.transform.localScale = new Vector3(newX, _FullHealtBarSize.y, _FullHealtBarSize.z);



        if (_state != _lastState) //detect state transition
        {
            _isEnteringState = true;
            _lastState = _state;
        }

        //State Machine -------------------------------------------------------------------------------------------
        //If you do not know what a state machine is, the basic concept is 
        //that you organize the behaviour into certain states that decribe what the player (or enmy or whatever) may do.
        //The clue here is that each state is *inclusive* and not exlusive in the options of behaviour.
        //An example of this is that "jumping" is possible both while idle and moving. 
        //Instead of making specialized bahaviour for jumping and idle, we just allow both states to call the same method and transition into jumping state.

        //Idle state
        if (_state == State.Idle)
        {


        }
        if (_state == State.Attacking)
        {
            _Animator.SetBool("isAttacking", true);
            if (_HitPointsCurrent <= 0)
            {
                ChangeState(State.Dead);
                _Animator.SetBool("isDead", true);
            }
        }
        if (_state == State.Dead)
        {


        }

        _isEnteringState = false; //reset entering state to false

    } //End Update

    private void Reset()
    {
        _Animator.SetBool("isAttacking", false);
        _Animator.SetBool("isDead", false);
    }
    public void ChangeState(State newState)
    {
        Reset();
        _state = newState;
    }

    public State GetState() { return _state; }

    public void HardReset()
    {
        _HitPointsCurrent = 100;
        ChangeState(State.Attacking);
    }

    public void TakeDamage(float damageValue)
    {
        _HitPointsCurrent -= damageValue;
        Debug.Log("Player took " + damageValue + " points of damage!");

    }
    public void SpecialAttack(int isLeft)
    {
            _waveAttack.gameObject.SetActive(true);
        if (isLeft == 1)
        {
            Debug.Log("Fire left Lazooooors!");
            _waveAttack.position = Vector3.Lerp(_emitterLeft.position, _Zombie.transform.position, 0.4f);
            
        }
        else
        {
            Debug.Log("Fire right Lazooooors!");
            _waveAttack.position = Vector3.Lerp(_emitterRight.position, _Zombie.transform.position, 0.4f);
        }
            Invoke("SpecialAttackCleanUp", 0.5f);
    }
    public void SpecialAttackCleanUp()
    {
        _waveAttack.gameObject.SetActive(false);
    }
}
