using System.Collections;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Generator onFull; // event 대신 오브젝트 연결..

    public Conductor[] conductor;
    public bool isLineActivate = false;

    [SerializeField]
    private bool isFirstLine;
    [SerializeField]
    [Header("채워지는 시간")]
    private float fillTime = 1.0f;

    public bool isPreShaderFilled = false;


    void Start()
    {
        StartCoroutine(CheckIsLineActivate());
    }

    IEnumerator CheckIsLineActivate()
    {
        yield return new WaitUntil(() => isLineActivate);
        StartCoroutine(fillShader());
        //Debug.Log("line activate");
        yield break;
    }

    IEnumerator fillShader()
    {
        float fillTimer = 0.0f; // 쉐이더 채우는 시간
        if (!isFirstLine) { yield return StartCoroutine(Timer()); } //if isnot first line -> delay

        // if isfirstLine -> fill shader instantly or is Shader filled
        while (isLineActivate && (isFirstLine || isPreShaderFilled) && fillTimer <= 1.0) // fillTimer = 0 immediately when line is deactive
        {
            fillTimer += 1.0f / fillTime * Time.deltaTime;
            //Debug.Log($"shader : {fillTimer}");
            this.gameObject.GetComponent<Renderer>().material.SetFloat("_Cutoff", 1.0f - fillTimer);
            yield return null;
        }
        fillTimer = 0; // filltimer 초기화
        //Debug.Log("Count++");
        yield break;
    }
    public void activateLines()
    {
        // line isActivate = true, conductor activate
        isLineActivate = true;
        for (int i = 0; i < conductor.Length; i++)
        {
            conductor[i].activate();
        }
        onFull?.LineCheck(true); // ?. check onFull event whether is or isn't null. : Generator shader fill

    }

    public void deactivateLines()
    {
        isLineActivate = false;
        this.gameObject.GetComponent<Renderer>().material.SetFloat("_Cutoff", 1.0f);
        for (int i = 0; i < conductor.Length; i++)
        {
            conductor[i].deactivate(this);
        }
        onFull?.LineCheck(false);
        StartCoroutine(CheckIsLineActivate()); // 다시 체크 코루틴 활성화.
    }

    IEnumerator Timer()
    {
        float Timer = 0.0f;
        while (Timer <= 1.0f)
        {
            Timer += 1.0f / fillTime * Time.deltaTime; // fill time 만큼 대기.
            //Debug.Log($"Timer : {Timer}");
            yield return null; // essential.
        }
        isPreShaderFilled = true;
        yield break;
    }
}
