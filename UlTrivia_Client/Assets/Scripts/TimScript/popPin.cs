using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popPin : MonoBehaviour {

    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void setPin()
    {
        anim.SetTrigger("pin");
    }
}
