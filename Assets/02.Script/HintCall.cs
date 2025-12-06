using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintCall : MonoBehaviour, IHint
{
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
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        theAudio.PlaySFX("mission");
        UIManager.Instance.playSubtitle();
        yield return new WaitForSeconds(12.0f);
    }

    public void deact()
    {
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        theAudio.StopSFX("mission");
        UIManager.Instance.stopSubtitle();
    }
}
