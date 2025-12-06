using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;

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
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        instance = this;
        //this.gameObject.SetActive(false);
        if (this.gameObject.activeSelf)
        {
            Debug.Log("Active AudioManager");
        }
        else
        {
            Debug.Log("Deactive AudioManager");
        }
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
        return;
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }
    public void setActiveSound(bool b)
    {
        this.gameObject.SetActive(b);
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }
    public void StopAllSFX()
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i].Stop();
        }
    }
    public void StopSFX(string p_sfxName)
    {
        // sfx 배열에서 해당 이름의 AudioClip을 찾습니다.
        AudioClip targetClip = null;
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                targetClip = sfx[i].clip;
                break;
            }
        }

        // targetClip이 null이 아니라면, sfxPlayer 배열에서 해당 Clip을 재생 중인 AudioSource를 중지합니다.
        if (targetClip != null)
        {
            for (int j = 0; j < sfxPlayer.Length; j++)
            {
                if (sfxPlayer[j].clip == targetClip && sfxPlayer[j].isPlaying)
                {
                    sfxPlayer[j].Stop();
                }
            }
        }
        else
        {
            Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        }
    }

    public void PauseAllSound()
    {
        AudioListener.pause = true;
    }

    public void UnPauseAllSound()
    {
        AudioListener.pause = false;
    }
}