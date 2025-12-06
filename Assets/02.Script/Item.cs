using UnityEngine;


public class Item : MonoBehaviour
{
    public bool isItem; // 아이템이 있는지 확인하는 변수 -> Conductor 스크립트에서 사용
    public GameObject floor;
    public GameObject conductor;
    public Material preMat;
    public Material Mat;


    private Collider conductorCol;
    private Vector3 pos;
    private Vector3 rotation;
    private Vector3 ConductorColpos;
    private bool wasSoundPlayed;
    private bool isDrag; // 드래그 하고 있는지 확인

    // Start is called before the first frame update
    void Start()
    {
        isDrag = false;
        isItem = false;
        preMat = this.GetComponent<MeshRenderer>().material;
        pos = this.transform.position;
        rotation = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseEnter()
    {
        if(wasSoundPlayed == true) return ; // play됐었으면 리턴 사운드 재생 안함.
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        theAudio.PlaySFX("itemAct");
        wasSoundPlayed = true;
    }
    private void OnMouseExit()
    {
        if(isDrag==false) wasSoundPlayed = false; // 나갈때 다시 False로 초기화
        //Debug.Log("Item Exit");
    }

    private void OnMouseUp()
    {
        isDrag = false; // 아이템에 마우스 떼면 드래그 안하고 있는 판정
        if (!isItem && conductorCol != null && ConductorColpos != this.GetComponent<Collider>().bounds.center)
        {
            //conductor.GetComponent<Conductor>().isConductor = false;
            conductorCol.enabled = true;
        }
    }
    private void OnMouseDown()
    {
        isDrag = true; // 아이템 클릭하고 있으면 드래그 하고 있는 판정
        wasSoundPlayed = true; // 옮기는 동안 소리 안나게
        if (isItem)
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.GetComponent<Rigidbody>().isKinematic = false;
            ConductorColpos = conductorCol.bounds.center;
            conductorCol.enabled = false;
            conductor.GetComponent<Conductor>().outItem();
        }
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.GetComponent<Rigidbody>().isKinematic = false;
        isItem = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == floor && !isItem)
        {
            moveOrigin();
        }
        else if (isItem)
        {
            conductorCol = other.GetComponent<Collider>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDrag && !collision.gameObject.CompareTag("Player") && !isItem) // 드래그 하고 있고, 부딫힌 물체가 플레이어가 아니고, 컨덕터에 안들어 갔을 때
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            Vector3 lastVelocity = rb.velocity; // 드래그 했을 때 마지막 이동 속도
            float speed = Mathf.Clamp(lastVelocity.magnitude, 0f, 0.3f); // 마지막 이동 속도로 거리 구하고 0 ~ 0.3으로 Clamp
            ContactPoint contact = collision.contacts[0]; // 다른 물체 콜라이더에 닿은 첫 번째 포인트 위치
            Vector3 normal = contact.normal; // 다른 물체 콜라이더에 닿은 첫 번째 포인트 위치에서 법선 벡터

            this.transform.position += normal * speed; // 아이템 위치 재정의
        }
    }

    public void moveOrigin()
    {
        this.transform.eulerAngles = rotation;
        this.transform.position = pos;
    }
}
