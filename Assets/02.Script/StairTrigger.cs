using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    private Fade fade;
    private bool isFloor1; // 1층인지 확인하는 변수

    // Start is called before the first frame update
    void Start()
    {
        fade = FindObjectOfType<Fade>();
        isFloor1 = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // 충돌한 물체가 플레이어인지 확인
        {
            fade.StartFade(); // fade coroutine 실행
            if (isFloor1) // 1층인지 확인
            {
                other.transform.position = spawnPoint2.transform.position; // 충돌 시 2층으로 이동
                isFloor1 = false;
            }
            else
            {
                other.transform.position = spawnPoint1.transform.position; // 충돌 시 1층으로 이동
                isFloor1 = true;
            }
        }
    }
}
