using System.Collections;
using UnityEngine;

public class Guide : MonoBehaviour, IHint
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
        this.GetComponent<Renderer>().material.SetFloat("_Cutoff", 0.0f);
        yield return new WaitForSeconds(0.5f);
    }

    public void deact()
    {
        this.GetComponent<Renderer>().material.SetFloat("_Cutoff", 1.0f);
    }
}
