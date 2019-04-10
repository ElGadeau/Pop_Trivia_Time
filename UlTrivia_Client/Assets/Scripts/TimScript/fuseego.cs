using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuseego : MonoBehaviour {

    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void lancerFusee()
    {
        anim.SetTrigger("clicked");
    }
}
