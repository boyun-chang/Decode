using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("종료 카운트")]
    public int endCount = 2;

    [Header("클리어시 실행 힌트를 가진 오브젝트")]
    public GameObject[] actHint;

    private float fillAmount = 0f;
    private Renderer[] GenRenderer; // LOD Renderers
    [SerializeField]
    int electricCount = 0;

    Coroutine hintCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        float minAmount = 0.4f; // _cutoff 범위 0.4~1.0
        float maxAmount = 0.88f;

        float lineStep = 0.4f;
        GenRenderer = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer l in GenRenderer)
        {
            l.material.SetFloat("_Cutoff", maxAmount); // initial cutoff setting

            lineStep = (maxAmount - minAmount) / endCount; // case1 : 0.48, case 2: 0.24, case 3: 0.16
            l.material.SetFloat("_LineA", minAmount); // case1 : 0.88- 0.44 // case2 : 0.88 - 0.22
            l.material.SetFloat("_LineB", minAmount + lineStep);
            //l.material.SetFloat("_LineC", minAmount + lineStep * endCount);
        }
    }


    public void LineCheck(bool isCorrectLine)
    {
        if (isCorrectLine) { electricCount++; } else { electricCount--; }
        executeHint();
        Debug.Log($"electric count : {electricCount}");
        chargeMaterial(electricCount);
        Invoke("soundPlay", 0.4f); // 다 계산되고 나서 실행해야하는 친구...
    }

    void executeHint()
    {
        if (electricCount == endCount) // sound and hint
        {
            hintCoroutine = StartCoroutine(sequanceActHint()); // if count correct, sequentially activate coroutine
        }
        else
        {
            if (hintCoroutine != null) StopCoroutine(hintCoroutine);
            foreach (GameObject i in actHint)
            {
                i.GetComponent<IHint>().deact();
            }
            if (electricCount > endCount) // 다 계산되기 전에 실행되어야하는 친구.....
            {
                GameManager.Instance.OnDamage();
            }
        }
    }

    void soundPlay()
    {
        // sound in LineCheck
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        if (electricCount == endCount) // sound and hint
        {
            theAudio.PlaySFX("success");
            Debug.Log("success");
        }
        else
        {
            if (electricCount > endCount)
            {
                theAudio.PlaySFX("fail");
                Debug.Log("fail");
            }
        }
    }

    void chargeMaterial(int chargeAmount)
    {
        float minAmount = 0.40f; // _cutoff 범위 0.4~1.0
        float maxAmount = 0.88f; // �ƿ� ��ä���� ����
        Color color = Color.cyan;

        if (chargeAmount == 0)
        {
            fillAmount = 0f;
        }
        else
        {
            fillAmount = (maxAmount - minAmount) * chargeAmount / endCount; // 4가 endcount 인 경우 전기 1당 0.15만큼 컷오프.
            if (chargeAmount > endCount)
            {
                color = Color.red;
            }
        }

        foreach (Renderer l in GenRenderer)
        {
            l.material.SetColor("_Color", color);
            l.material.SetFloat("_Cutoff", maxAmount - fillAmount);
        }
    }
    IEnumerator sequanceActHint()
    {
        foreach (GameObject i in actHint)
        {
            //i.GetComponent<IHint>().act();
            yield return StartCoroutine(i.GetComponent<IHint>().act());
            //yield return new WaitForSeconds(2f);
        }
    }
}
