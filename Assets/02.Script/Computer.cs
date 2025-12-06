using System.Collections;
using UnityEngine;

public class Computer : MonoBehaviour, IHint
{
    public GameObject on;
    public GameObject off;
    public GameObject[] actHint;
    public bool iscorrect;
    private bool isAct = false;

    // Start is called before the first frame update
    void Start()
    {
        iscorrect = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator act()
    {
        isAct = true;
        off.SetActive(false);
        on.SetActive(true);

        yield return new WaitForSeconds(0.5f);
    }

    public void deact()
    {
        isAct = false;
        on.SetActive(false);
        off.SetActive(true);
    }

    private void OnMouseOver()
    {
        if (isAct)
        {
            UIManager.Instance.OnComputerPanel();
            if (UIManager.Instance.CorrectNumber())
            {
                foreach (GameObject i in actHint)
                {
                    //i.GetComponent<IHint>().act();
                    StartCoroutine(i.GetComponent<IHint>().act());
                }
                iscorrect = true;
                isAct = false;
            }
        }
        else
        {
            UIManager.Instance.ExitComputerPanel();
        }
    }

    private void OnMouseExit()
    {
        UIManager.Instance.ExitComputerPanel();
    }
}