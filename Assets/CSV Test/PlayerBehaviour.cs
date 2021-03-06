﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]

public class PlayerBehaviour : Behaviour
{
    //component refs

    //Sounds
    //[Header("Sounds")]
    //public AudioClip _phaseloop;
    //[Header("Player Stats")] //if you are making a more complex this might be moved into a manager

    //other global variables


    public Transform _emitterLeft, _emitterRight, _waveAttack;

    //Setting up component references, Awake() is called before Start()
    public override void Awake()
    {
        base.Awake(); //we still want the stuff in the superclass's awake method(Behaviour) to happen
        _NameDisplay.text = RandomNames.GetName();
        if (_Target == null)
        {
            _Target = GameObject.FindWithTag("Team 2");
            if (_Target == null) Debug.LogError("No zombie tagged with \"Team 2\" present in scene");
        }
    }

    // Update is called as part of the base class "behaviour" and you will not find it here

    public override void StateMachine()
    {
        //Idle state
        if (_state == State.Idle)
        {
            //TODO: LookforTarget so there is support for multiple zombies
            if (_Target.GetComponent<Behaviour>()._HitPointsCurrent > 0) { ChangeState(State.Attacking); }
        }
        if (_state == State.Attacking)
        {
            _Animator.SetBool("isAttacking", true);
            if (_HitPointsCurrent <= 0)
            {
                ChangeState(State.Dead);
                _Animator.SetBool("isDead", true);
            }
            if (_Target.GetComponent<Behaviour>()._HitPointsCurrent <= 0)
            {
                Debug.Log("enemyi sDEADED");
                ChangeState(State.Idle);
            }
        }
        if (_state == State.Dead)
        { //Do nothing
        }

    }

    private void Reset()
    {
        _Animator.SetBool("isAttacking", false);
        _Animator.SetBool("isDead", false);
        SpecialAttackCleanUp();
    }
    override public void ChangeState(State newState)
    {
        Reset();
        _state = newState;
    }

    public void SpecialAttack(int isLeft) //gets called from animator event
    {
        _waveAttack.gameObject.SetActive(true);
        if (isLeft == 1)
        {
            Debug.Log("Fire left Lazooooors!");
            //_waveAttack.position = Vector3.Lerp(_emitterLeft.position, _Target.transform.position, 0.4f);
            _waveAttack.position = _emitterLeft.position;
        }
        else
        {
            Debug.Log("Fire right Lazooooors!");
            //_waveAttack.position = Vector3.Lerp(_emitterRight.position, _Target.transform.position, 0.4f);
            _waveAttack.position = _emitterRight.position;
        }
        DealDamage(_Target.GetComponent<Behaviour>(), 3f);
        Invoke("SpecialAttackCleanUp", 0.5f);
    }
    public void SpecialAttackCleanUp()
    {
        _waveAttack.gameObject.SetActive(false);
    }
}
