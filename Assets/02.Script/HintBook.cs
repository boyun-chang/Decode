using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBook : MonoBehaviour, IHint
{
    public bool isfirst;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator act()
    {
        if (isfirst)
        {
            UIManager.Instance.OnMorseBtn();
        }
        else
        {
            UIManager.Instance.OnBookRoomHintClick();
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void deact()
    {
        if (!isfirst)
        {
            UIManager.Instance.OnBookRoomHintClose();
        }
        else
        {
            UIManager.Instance.OffMorseBtn();
        }
    }
}
