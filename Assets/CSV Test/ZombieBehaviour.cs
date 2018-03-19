using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]

public class ZombieBehaviour : Behaviour
{
    //state machine variables

    //component refs
    private Animation _Animation;
    private NavMeshAgent _NavMeshAgent;

    //Sounds
    //[Header("Sounds")]
    //public AudioClip _phaseloop;
    //[Header("Player Stats")] //if you are making a more complex this might me moved into a manager
    ZombieStatManager.Stats _stats;

    //other global variables
    private Vector3 _spawnPosition;

    //Setting up component references, Awake() is called before Start()
    override public void Awake()
    {
        //We do not call base.Awake() in ZombieBehaviour 
        //because we do not want all the behaviour from the 
        //base class because zombie uses legacy Animation instead of an Animator
        _spawnPosition = transform.position;
        _name = gameObject.name;
        _Speaker = GetComponent<AudioSource>();
        _Animation = GetComponentInChildren<Animation>();
        _NavMeshAgent = GetComponent<NavMeshAgent>();
        _FullHealtBarSize = _HealthBar.transform.localScale;
        if (!GetComponentInChildren<Animation>())
        {
            Debug.LogError("NO ANIMATON FOUND FOR ZOMBIE!");
        }
        if (_Target == null)
        {
            _Target = GameObject.FindWithTag("Team 1");
            if (_Target == null) Debug.LogError("No cacti tagged with \"Team 2\" present in scene");
        }
    }
    private void Start()
    {
        _stats = ZombieStatManager._stats;
    }

    // Update is called once per frame
    public override void Update()
    {
        _Animation["Zombie_Walk_01"].speed = _stats._Walkspeed;
        _NavMeshAgent.speed = _stats._Walkspeed/2;
        _Animation["Zombie_Attack_01"].speed = _stats._Attackspeed; //TODO: calculate so that 50 frames becomes 1 second?
        base.Update();

    } //End Update

    public override void StateMachine()
    {
        //Idle state
        if (_state == State.Idle)
        {
            _Animation.Play("Zombie_Idle_01");
            if (_HitPointsCurrent <= 0) ChangeState(State.Dead);
            if (_Target.GetComponent<Behaviour>()._HitPointsCurrent > 0) { ChangeState(State.Moving); }
        }
        if (_state == State.Attacking)
        {
            _Animation.Play("Zombie_Attack_01");
            if (_Target.GetComponent<PlayerBehaviour>().GetState() == PlayerBehaviour.State.Dead)
            {
                ChangeState(State.Idle);
            }
            if (_HitPointsCurrent <= 0) ChangeState(State.Dead);

        }
        if (_state == State.Dead)
        {
            if (_isEnteringState) //using if _isEnteringState because i wnt this to only happen once per state change
            {
                _Animation.Play("Zombie_Death_01");
                Invoke("HardReset", 0.5f);
            }
        }
        if (_state == State.Moving)
        {
            _Animation.Play("Zombie_Walk_01");
            _NavMeshAgent.SetDestination(_Target.transform.position);
            if (Vector3.Distance(transform.position, _Target.transform.position) < 2)
            {
                ChangeState(State.Attacking);
            }
            if (_HitPointsCurrent <= 0) ChangeState(State.Dead);

        }
    }
    public void Die()
    {

    }

    private void Reset()
    {
        _NavMeshAgent.destination = transform.position;        
    }
    override public void HardReset()
    {
        transform.position = _spawnPosition;
        _HitPointsCurrent = ZombieStatManager._stats._Hitpoints;
        _HitpointsMaximum = ZombieStatManager._stats._Hitpoints;
        _Animation.Play("Zombie_Idle_01");
        ChangeState(State.Idle);
    }
    override public void ChangeState(State newState)
    {
        Reset();
        _state = newState;
    }
    public void DoDamage() //helper method because the zombie animation event is legacy and cannot pass arguments to methods.
    {
        DealDamage(_Target.GetComponent<PlayerBehaviour>(), _stats._AttackDamage);
        
    }

}
