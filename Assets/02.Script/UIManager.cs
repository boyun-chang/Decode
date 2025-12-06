using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    //public GameManager gameManager;

    // Panel

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }

    private PlayerCtrl player;
    public GameObject homePanel;
    public GameObject PlayPanel;
    public GameObject escPanel;

    // StartImage
    [Header("Start Image")]
    public GameObject StartImage;
    public static bool isStart = false;
    public static bool isSecond = false;

    [Header("Opening Video")]
    public GameObject OpeningVideo;
    private VideoPlayer opening;
    public static bool isOpeningOver = false;

    [Header("skip button")]
    public GameObject skipBtn;

    [Header("Ending Video")]
    public GameObject EndingVideo;
    private VideoPlayer ending;

    public GameObject lifePanel;
    public Image[] lifeSlots = new Image[3];

    //  𽺺 ȣ  ؼ   
    [Header("Moss Panel")]
    public GameObject morseBtn;
    public GameObject mossPanel;

    //  𽺺 ȣ  Է â
    [Header("Moss Input Panel")]
    public RectTransform mossInput;

    [Header("bookRoomMorse")]
    public GameObject bookRoomMorse;

    // Morse Click UI
    [Header("Morse Click Panel")]
    public GameObject MorseClick;
    [Header("Morse UI Text")]
    public TMP_Text morseText;
    [SerializeField] private string copyMorseText;

    // Computer UI
    [Header("Computer Panel")]
    public GameObject computerPanel;
    [Header("Computer UI Text")]
    [SerializeField] private TMP_InputField computerText;

    // timer
    [Header("Timer UI Text")]
    [SerializeField] private TMP_Text timerTxt;
    [Header("Current Time")]
    public static float curTime = 1800;

    Coroutine timerCoroutine; // Ÿ ̸   ڷ ƾ
    private bool firstCorrect = false;

    public GameObject missionPanel;
    public TextMeshProUGUI uiText; // TextMeshProUGUI 컴포넌트 배열
    public string[] messages; // 변경할 메시지 배열
    private int currentIndex = 0;

    public bool isEnd = false;
    public RectTransform black;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        escPanel.SetActive(false);
        missionPanel.SetActive(false);
        black.gameObject.SetActive(true);
        lifeSlots = lifePanel.GetComponentsInChildren<Image>();
        lifeSlots = lifeSlots.Skip(1).ToArray();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start()
    {
        if (!isStart && !isSecond)
        {
            player = this.transform.GetComponentInParent<PlayerCtrl>();
            player.enabled = false;
            Cursor.visible = true;
            StartImage.SetActive(true);
            isStart = true;
            SceneManager.LoadScene("Map_f12");
        }
        else if (isStart && !isSecond)
        {
            homePanel.SetActive(true);
            Cursor.visible = true;

            black.gameObject.SetActive(true);
        }
        else if (isStart && isSecond)
        {
            StartImage.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayPanel.SetActive(true);
            morseBtn.SetActive(true);
            timerCoroutine = StartCoroutine(StartTimer());
            AudioManager.instance.setActiveSound(true);
            AudioManager.instance.PlayBGM("BGM1");
            Debug.Log("Playing Underground Audio");

            black.gameObject.SetActive(false);
        }
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !homePanel.activeSelf && !StartImage.activeSelf)
        {
            // Menu Panel Ȱ  ȭ   Ȱ  ȭ
            if (!escPanel.activeSelf)
            {
                escPanel.SetActive(true);
                //  ⺻       esc        Ŀ   Ȱ  ȭ  ۵ .
                Cursor.lockState = CursorLockMode.None; //    콺 Ȱ  ȭ.
                Cursor.visible = true;

                if (isOpeningOver && !EndingVideo.activeSelf)
                {
                    StopCoroutine(timerCoroutine);
                    AudioManager.instance.PauseAllSound();
                    if (missionPanel.activeSelf) Time.timeScale = 0;
                }
                else if (!isOpeningOver && OpeningVideo.activeSelf)
                {
                    opening.Pause();
                }
                else if (EndingVideo.activeSelf)
                {
                    ending.Pause();
                }
            }
            else
            {
                escPanel.SetActive(false);
                if (isOpeningOver && !EndingVideo.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    timerCoroutine = StartCoroutine(StartTimer());
                    AudioManager.instance.UnPauseAllSound();
                    if (missionPanel.activeSelf) Time.timeScale = 1;
                }
                else if (!isOpeningOver && OpeningVideo.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    opening.Play();
                }
                else if (EndingVideo.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    ending.Play();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Tab) && !homePanel.activeSelf && morseBtn.activeSelf)
        {
            // Moss Ȱ  ȭ
            // if Ȱ  ȭ  Ǿ          .
            if (!mossPanel.activeSelf)
            {
                mossPanel.SetActive(true);
            }
            else
            {
                mossPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LateUpdate() //  update      ȣ  
    {
        //       
    }


    IEnumerator StartTimer()
    {
        int minute;
        int second;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            minute = (int)curTime / 60;
            second = (int)curTime % 60;
            timerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if (curTime <= 0)
            {
                curTime = 0;
                setLifeSlot(0);
                StartCoroutine(GameManager.Instance.Die());
                yield break;
            }
        }
    }

    void CheckOver2(VideoPlayer vp)
    {
        isOpeningOver = true;
        OpeningVideo.SetActive(false);
        PlayPanel.SetActive(true);
        timerCoroutine = StartCoroutine(StartTimer());

        AudioManager.instance.setActiveSound(true);
        AudioManager.instance.PlayBGM("BGM0");
        Debug.Log("play audio");
        black.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void CheckOver3(VideoPlayer vp)
    {//ending
        GameManager.Instance.resetStatic();
        SceneManager.LoadScene("Map_f12");
        AudioManager.instance.setActiveSound(true);
        AudioManager.instance.PlayBGM("BGM0");
    }

    // button UI

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                        Application.Quit(); //    ø    ̼      
        #endif
    }

    public void OnStartClick()
    {
        homePanel.SetActive(false);
        StartImage.SetActive(false);

        if (!isOpeningOver)
        {
            OpeningVideo.SetActive(true);
            opening = OpeningVideo.GetComponentInChildren<VideoPlayer>();
            opening.Play();
            opening.loopPointReached += CheckOver2;
        }
        else if (EndingVideo.activeSelf)
        {
            ending.Play();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayPanel.SetActive(true);
            timerCoroutine = StartCoroutine(StartTimer());
            AudioManager.instance.UnPauseAllSound();
            if (missionPanel.activeSelf) Time.timeScale = 1;
        }
    }


    public void OnRestartClick()
    {
        //  ٽ          
        escPanel.SetActive(false);
        // Ÿ ̸   ۵ .
        if (isOpeningOver && !EndingVideo.activeSelf)
        {
            timerCoroutine = StartCoroutine(StartTimer());
            AudioManager.instance.UnPauseAllSound();
            if (missionPanel.activeSelf) Time.timeScale = 1;
        }
        else if (!isOpeningOver && OpeningVideo.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // 7/1 UIManager 고친 부분 
            opening.Play();
            return;
        }
        else if (EndingVideo.activeSelf)
        {
            ending.Play();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnHomeClick()
    {
        //    ӿ           Ȩ    .
        escPanel.SetActive(false);
        homePanel.SetActive(true);
    }

    public void OnMorseBtn()
    {
        morseBtn.SetActive(true);
    }

    public void OffMorseBtn()
    {
        morseBtn.SetActive(false);
    }

    public void OnMossBtnClick()
    {
        // mossImg enable
        if (mossPanel.activeSelf) mossPanel.SetActive(false);
        else
        {
            mossPanel.SetActive(true);
        }
    }
    public void OnMossBtnClose()
    {
        // mossImg enable
        mossPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMorseClick()
    {
        if (!Cursor.visible)
        {
            MorseClick.SetActive(true);
        }
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void MorseShortText()
    {
        morseText.text += ".";
        copyMorseText = morseText.text;
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        theAudio.PlaySFX("short");
    }

    public void MorseLongText()
    {
        morseText.text += "-";
        copyMorseText = morseText.text;
        AudioManager theAudio;
        theAudio = AudioManager.instance;
        theAudio.PlaySFX("long");
    }

    public void DeleteMorse()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && copyMorseText.Length != 0)
        {
            copyMorseText = copyMorseText.Substring(0, copyMorseText.Length - 1);
            morseText.text = copyMorseText;
        }
    }

    public void ExitMorseClick()
    {
        MorseClick.SetActive(false);
        //morseText.text = "";
        copyMorseText = morseText.text;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public bool CorrectMorse()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager theAudio;
            theAudio = AudioManager.instance;
            if (morseText.text == ".........-" && !firstCorrect)
            {
                // add correct effect
                morseText.text = "";
                copyMorseText = morseText.text;
                firstCorrect = true;
                theAudio.PlaySFX("success");
                return false;
            }
            else if (morseText.text == "-....-.----" && firstCorrect)
            {
                // add correct effect
                StopCoroutine(timerCoroutine);
                PlayPanel.SetActive(false);
                EndingVideo.SetActive(true);
                theAudio.PlaySFX("success");

                ending = EndingVideo.GetComponentInChildren<VideoPlayer>();
                ending.Play();
                ending.loopPointReached += CheckOver3;
                isEnd = true;
                return true;
            }
            else
            {
                GameManager.Instance.OnDamage();
                //morseText.text = "";
                copyMorseText = morseText.text;
                // add incorrect effect
                theAudio.PlaySFX("fail");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void OnBookRoomHintClick()
    {
        // mossImg enable
        if (bookRoomMorse.activeSelf) bookRoomMorse.SetActive(false);
        else
        {
            bookRoomMorse.SetActive(true);
        }
    }
    public void OnBookRoomHintClose()
    {
        // mossImg enable
        bookRoomMorse.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnComputerPanel()
    {
        computerPanel.SetActive(true);
        computerText.ActivateInputField();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ExitComputerPanel()
    {
        computerPanel.SetActive(false);
        //computerText.text = "";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool CorrectNumber()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (computerText.text == "0618")
            {
                // add correct effect
                OnBookRoomHintClose();
                return true;
            }
            else
            {
                // add incorrect effect
                computerText.text = "";
                GameManager.Instance.OnDamage();
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void setLifeSlot(int life)
    {
        for (int i = 0; i < 3 - life; i++)
        {
            lifeSlots[2 - i].color = Color.black;
        }
    }

    public void OnSkip()
    {
        // mossImg enable
        black.gameObject.SetActive(false);
        skipBtn.SetActive(false);

        isOpeningOver = true;
        OpeningVideo.SetActive(false);
        PlayPanel.SetActive(true);
        timerCoroutine = StartCoroutine(StartTimer());

        AudioManager.instance.setActiveSound(true);
        AudioManager.instance.PlayBGM("BGM0");
        Debug.Log("play audio");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    Coroutine subtitleCoroutine;
    public void playSubtitle()
    {
        missionPanel.SetActive(true);
        subtitleCoroutine = StartCoroutine(ChangeText());
        Debug.Log("play subtitle");
    }
    public void stopSubtitle()
    {
        missionPanel.SetActive(false);
        StopCoroutine(subtitleCoroutine);
    }
    private IEnumerator ChangeText()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            // TextMeshProUGUI 오브젝트의 텍스트 변경
            uiText.text = messages[i];
            yield return new WaitForSeconds(3.0f); // 2초 대기
        }
        missionPanel.SetActive(false);
        yield break;
    }
}
