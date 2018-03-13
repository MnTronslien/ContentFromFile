using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour {

    private ZombieBehaviour _ZB;

    private void Start()
    {
        _ZB = GetComponentInParent<ZombieBehaviour>();
    }

    public void AttackHit()
    {
        Debug.Log("animation event reporting!");
        _ZB.DoDamage();
    }

}
