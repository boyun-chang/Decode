using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // ���� ������ ���� private
    public static GameManager Instance
    {
        // �̱��� ������Ƽ(�ܺ� Ŭ�������� �����Ϸ��� ������Ƽ public���ֱ�)
        get
        {
            if (null == instance) return null; // instance ������ �������� �ʱ� ����.
            return instance;
        }
    }
    public bool isDebug = false;
    public static int life = 3;

    public GameObject player;
    public enum room
    {
        first1, first2,
        second1, second2,
        basement, start
    }
    public room currentRoom;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        //
    }
    private void Start()
    {
        if (isDebug)
        {
            UIManager.isStart = true;
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            switch (currentRoom)
            {
                case room.second1:
                    playerCtrl.setPosition(new Vector3(1, 52, -9f));
                    break;
                case room.second2:
                    playerCtrl.setPosition(new Vector3(2.3f, 52, -35f));
                    break;
                case room.basement:
                    playerCtrl.setPosition(new Vector3(-5f, 52, -1.6f));
                    break;
                case room.first2:
                    playerCtrl.setPosition(new Vector3(-4, 45, -12.5f));
                    break;
                case room.first1:
                    playerCtrl.setPosition(new Vector3(-5, 45, -30f));
                    break;
                case room.start:
                    playerCtrl.setPosition(new Vector3(6, 45, -20f));
                    break;
            }
        }
        //Debug.Log($"life{life}");
        if (UIManager.isStart)
        {
            UIManager.Instance.setLifeSlot(life);
        }

    }
    void Update()
    {
        
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(1.0f);
        resetStatic();
        SceneManager.LoadScene("Map_f12");
        yield break;
    }

    public void OnDamage()
    {
        life--;
        UIManager.Instance.setLifeSlot(life);
        if (life <= 0)
        {
            StartCoroutine("Die");
        }
    }

    public void resetStatic()
    {
        UIManager.isStart = false;
        UIManager.isSecond = false;
        UIManager.isOpeningOver = false;
        UIManager.curTime = 1800;
        life = 3;
    }
}
