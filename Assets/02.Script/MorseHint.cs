using System.Collections;
using UnityEngine;

public class MorseHint : MonoBehaviour, IHint
{
    public GameObject[] moss = new GameObject[4];
    [SerializeField]
    private Renderer[] emissionMat;
    public GameObject[] lights;
    public Renderer[] ceilingEmission;

    // Start is called before the first frame update
    void Start()
    {
        emissionMat = this.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < emissionMat.Length; i++)
        {
            emissionMat[i].material.SetColor("_Color", Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator act()
    {
        foreach (GameObject l in lights)
        {
            l.SetActive(false);
        }
        foreach (Renderer k in ceilingEmission)
        {
            k.material.DisableKeyword("_EMISSION");
        }
        yield return null;

        StartCoroutine(TurnOnLightsSequentially());
        
        yield break;
    }

    public void deact()
    {
        emissionMat = this.GetComponentsInChildren<Renderer>();

        foreach (GameObject l in lights)
        {
            l.SetActive(true);
        }
        foreach (Renderer k in ceilingEmission)
        {
            k.material.EnableKeyword("_EMISSION");
        }
        for (int i = 0; i < emissionMat.Length; i++)
        {
            emissionMat[i].material.SetFloat("_Cutoff", 1.0f);
        }
    }

    private IEnumerator TurnOnLightsSequentially()
    {
        Renderer[] previousLight = null;
        while (!UIManager.Instance.isEnd)
        {
            for (int i = 0; i < moss.Length; i++)
            {
                if (previousLight != null)
                {
                    yield return StartCoroutine(TurnOff(previousLight));
                }

                yield return StartCoroutine(TurnOn(moss[i]));
                previousLight = moss[i].GetComponentsInChildren<Renderer>();

                yield return new WaitForSeconds(2);
            }

            if (previousLight != null)
            {
                yield return StartCoroutine(TurnOff(previousLight));
            }
            yield return new WaitForSeconds(5);
        }
        yield break;
    }

    private IEnumerator TurnOn(GameObject moss)
    {
        emissionMat = moss.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < emissionMat.Length; i++)
        {
            emissionMat[i].material.SetFloat("_Cutoff", 0.0f);
        }
        yield return null;
    }

    private IEnumerator TurnOff(Renderer[] lights)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].material.SetFloat("_Cutoff", 1.0f);
        }
        yield return null;
    }
}