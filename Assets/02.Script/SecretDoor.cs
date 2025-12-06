using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : MonoBehaviour, IHint
{
    public bool isNext;
    private Animator Secretdoor_anim;

    // Start is called before the first frame update
    void Start()
    {
        isNext = false;
        Secretdoor_anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator act()
    {
        if (Secretdoor_anim != null)
        {
            if (Secretdoor_anim.GetBool("SecretDoorOpen") == false)
            {
                Secretdoor_anim.SetBool("SecretDoorOpen", true);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }
    public void deact()
    {
        if (Secretdoor_anim != null)
        {
            if (Secretdoor_anim.GetBool("SecretDoorOpen") == true)
            {
                Secretdoor_anim.SetBool("SecretDoorOpen", false);
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
