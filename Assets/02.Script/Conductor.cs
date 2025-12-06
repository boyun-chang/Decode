using UnityEngine;

public class Conductor : MonoBehaviour
{
    public Line[] inLine; // 들어오는 선
    public Line[] outLine; // 나가는 선
    [SerializeField]
    [Header("활성화된 선")]
    int activatedLineIdx = -1;

    [SerializeField]
    [Header("맞는 전도체 유무")] private bool isConductor = false; // 맞는 전도체가 들어왔는지.
    [SerializeField] [Header("전기 활성화 유무")] private bool isActivated = false; // 지금 활성되어있는지
    [Header("인접 컨덕터")]
    public Item[] conductorItems = new Item[1];
    private Color rim; // 외곽선 색

    [SerializeField]
    int cnt = 0;

    void Start()
    {
        rim = this.gameObject.GetComponent<Renderer>().material.GetColor("_RimColor"); // 설정한 외곽선 색 가져오기
        activatedLineIdx = -1;
    }
    void Update()
    {

    }

    public void activate()
    {
        if (!isConductor) return; // 전도체 연결되어있지 않으면 return
        if (isActivated) return; // 현재 conductor가 활성화 되어있으면 return

        int i = 0;
        foreach (Line l in inLine) // 켜진 선이 배열에서 몇번째인지
        {
            if (l.isLineActivate == true)
            {
                cnt++; //cnt = 1인 경우만 가능.
                if (activatedLineIdx == i) return; // 이미 활성화된 선인 경우 뒤의 코드들 실행 되지 않도록
                activatedLineIdx = i;

                break;// 활성화된 라인이 2개 이상 안되게.
            }
            i++;
        }
        if (cnt == 0) { isActivated = false; return; } // 아무 선도 활성화되지 않은 경우. 
        //라인이 활성화 된 경우
        if (!outLine[activatedLineIdx].isLineActivate) outLine[activatedLineIdx].activateLines();
        // outLine 쉐이더 채우기
        isActivated = true;
    }

    public void deactivate(Line activatedLine)
    {
        //if (!isConductor) return; // 전도체 연결되어있지 않으면 켜져있지 않을테니까 ,,,
        if (!isActivated) return; // 현재 conductor가 활성화 되어있지 않으면 끌게 없으니까 리턴

        int i = 0; int idx = 0;
        foreach (Line l in inLine)
        {
            if (activatedLine == l) { idx = i; cnt--; break; }
            // 끄려는 라인하고 연결되어있는 라인(inout배열에서 같은 idx) 삭제.
            i++;
        }
        if (activatedLineIdx == idx) outLine[activatedLineIdx].deactivateLines();
        isActivated = false;
        activatedLineIdx = -1; // 초기화 일부러 IndexOutOfArray 발생시켜서 문제 있으면 파악하려고.
        activate(); // 끄면 다른 선 켜져야하니까.
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (isOutItem)
        {
            return;
        }*/
        foreach (Item i in conductorItems)
        {
            if (isConductor) return;
            if (other.gameObject.GetComponent<Item>() == i)
            {
                isConductor = true;
                i.isItem = true;
                //isOutItem = false;
                //Debug.Log("올바른 전도체를 입력받음.");
                freezeItem(other);
                other.transform.position = this.transform.position;
                this.gameObject.GetComponent<Renderer>().material.SetColor("_RimColor", Color.cyan); // 맞는 물체면 외곽선 색 -> 원래 색
                activate();
                //if (AudioManager.instance.isActiveAndEnabled) AudioManager.instance.PlaySFX("batteryTrue");
            }
            else
            {
                //Debug.Log("틀린 전도체를 입력받음.");
                if (other.CompareTag("Player")) return;
                this.gameObject.GetComponent<Renderer>().material.SetColor("_RimColor", Color.red); // 틀린 물체면 외곽선 -> 색 빨간색
                //if (AudioManager.instance.isActiveAndEnabled) AudioManager.instance.PlaySFX("batteryFalse");
            }
        }

        if (isConductor)
        {
            if (AudioManager.instance.isActiveAndEnabled) AudioManager.instance.PlaySFX("batteryTrue");
        }
        else
        {
            if (AudioManager.instance.isActiveAndEnabled) AudioManager.instance.PlaySFX("batteryFalse");
        }
    }

    private void freezeItem(Collider other)
    {
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // 리지드 바디 요소 고정
        other.transform.position = this.transform.position; // 위치 고정
        other.transform.eulerAngles = this.transform.eulerAngles;
    }

    private void OnTriggerExit(Collider other)  // 외곽선 색 -> 원래 색
    {
        if (isConductor)
        {
            this.gameObject.GetComponent<Renderer>().material.SetColor("_RimColor", Color.cyan);
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.SetColor("_RimColor", rim);
        }
    }

    public void outItem()
    {
        if (isConductor)
        {
            isConductor = false;
        }
        if (activatedLineIdx != -1) deactivate(inLine[activatedLineIdx]);

        this.gameObject.GetComponent<Renderer>().material.SetColor("_RimColor", rim);
    }
}