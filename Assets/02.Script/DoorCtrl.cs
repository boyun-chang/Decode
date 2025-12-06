using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour, IHint
{
    public bool isNext;
    private Animator door_anim;

    // Start is called before the first frame update
    void Start()
    {
        isNext = false;
        door_anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator act()
    {
        if (door_anim != null)
        {
            if (door_anim.GetBool("DoorOpen") == false)
            {
                AudioManager theAudio;
                theAudio = AudioManager.instance;
                door_anim.SetBool("DoorOpen", true);
                theAudio.PlaySFX("doorOpen");
            }
        }
        /*
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        theAudio.PlaySFX("doorOpen");*/
        yield return new WaitForSeconds(0.5f);
    }
    public void deact()
    {
        if (door_anim != null)
        {
            if (door_anim.GetBool("DoorOpen") == true)
            {
                door_anim.SetBool("DoorOpen", false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isNext)
        {
            isNext = false;
        }
        else
        {
            isNext = true;
        }
    }
}
