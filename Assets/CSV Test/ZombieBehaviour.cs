using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]

public class ZombieBehaviour : MonoBehaviour
{
    //state machine variables
    public enum State
    {
        Attacking, Idle, Dead, Moving
    }
    private State _state = State.Moving;
    private State _lastState = State.Moving;
    private bool _isEnteringState = false;

    //component refs
    private AudioSource _Speaker;
    private Animation _Animation;
    private NavMeshAgent _NavMeshAgent;

    //Sounds
    //[Header("Sounds")]
    //public AudioClip _phaseloop;
    //[Header("Player Stats")] //if you are making a more complex this might me moved into a manager
    ZombieStatManager.Stats _stats;

    //other global variables
    public GameObject _Player;
    public float _HitpointsMaximum = 100;
    public float _HitPointsCurrent = 100;
    public GameObject _HealthBar;
    private Vector3 _FullHealtBarSize;

    //Setting up component references, Awake() is called before Start()
    private void Awake()
    {
        _Speaker = GetComponent<AudioSource>();
        _Animation = GetComponentInChildren<Animation>();
        _NavMeshAgent = GetComponent<NavMeshAgent>();
        if (!GetComponentInChildren<Animation>())
        {
            Debug.LogError("NO ANIMATON FOUND FOR ZOMBIE!");
        }
        if (_Player == null)
        {
            _Player = GameObject.Find("Player");
        }
        _FullHealtBarSize = _HealthBar.transform.localScale;
    }
    private void Start()
    {
        _stats = ZombieStatManager._stats;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.name = "Zombie " + _state; //For debugging

        float newX = Mathf.Lerp(0, _FullHealtBarSize.x, _HitPointsCurrent / _HitpointsMaximum);
        _HealthBar.transform.localScale = new Vector3(newX, _FullHealtBarSize.y, _FullHealtBarSize.z);

        _Animation["Zombie_Walk_01"].speed = _stats._Walkspeed;
        _NavMeshAgent.speed = _stats._Walkspeed/2;
        _Animation["Zombie_Attack_01"].speed = _stats._Attackspeed; //TODO: calculate so that 50 frames becomes 1 second?


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
            _Animation.Play("Zombie_Idle_01");
        }
        if (_state == State.Attacking)
        {
            _Animation.Play("Zombie_Attack_01");
            if (_Player.GetComponent<PlayerBehaviour>().GetState() == PlayerBehaviour.State.Dead)
            {
                ChangeState(State.Idle);
            }
            
        }
        if (_state == State.Dead)
        {
            _Animation.Play("Zombie_Death_01");

        }
        if (_state == State.Moving)
        {
            _Animation.Play("Zombie_Walk_01");
            _NavMeshAgent.SetDestination(_Player.transform.position);
            if (Vector3.Distance(transform.position, _Player.transform.position) < 2)
            {
                ChangeState(State.Attacking);
            }

        }
        _isEnteringState = false; //reset entering state to false

    } //End Update

    private void Reset()
    {
        _NavMeshAgent.destination = transform.position;        
    }
    public void HardReset()
    {
        transform.position = new Vector3(-4, 0, 0);
        _HitPointsCurrent = ZombieStatManager._stats._Hitpoints;
        _HitpointsMaximum = ZombieStatManager._stats._Hitpoints;
        ChangeState(ZombieBehaviour.State.Moving);
    }
    public void ChangeState(State newState)
    {
        Reset();
        _state = newState;
    }
    public void DoDamage ()
    {
        _Player.GetComponent<PlayerBehaviour>().TakeDamage(_stats._AttackDamage);
        Debug.Log("Zombie behaviour transmits damage!");
    }
    public State GetState() { return _state; }
}
