using UnityEngine;

public class LongClick : MonoBehaviour
{
    private Animator Morse;
    private float elapsedTime = 0.0f;
    private float longClickTime = 0.3f;
    private bool isClicked = false;
    private bool isLongClick = false;
    private bool isClear = false;

    // Start is called before the first frame update
    void Start()
    {
        Morse = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked)
        {
            elapsedTime += Time.deltaTime;
            isLongClick = (longClickTime <= elapsedTime ? true : false);
        }
    }
    
    private void OnMouseOver()
    {
        if (!isClear)
        {
            UIManager.Instance.OnMorseClick();
            UIManager.Instance.DeleteMorse();
            if (UIManager.Instance.CorrectMorse())
            {
                isClear = true;
            }
        }
        else
        {
            UIManager.Instance.ExitMorseClick();
        }
    }

    private void OnMouseExit()
    {
        UIManager.Instance.ExitMorseClick();
    }

    private void OnMouseDown()
    {
        if (!Cursor.visible)
        {
            if (Morse != null && !isClear)
            {
                if (Morse.GetBool("MorseDown") == false)
                {
                    Morse.SetBool("MorseDown", true);
                }
            }
            isClicked = true;
        }
    }

    private void OnMouseUp()
    {
        if (!Cursor.visible)
        {
            if (isLongClick)
            {
                UIManager.Instance.MorseLongText();
            }
            else if (isClicked)
            {
                UIManager.Instance.MorseShortText();
            }

            if (Morse.GetBool("MorseDown") == true)
            {
                Morse.SetBool("MorseDown", false);
            }
            isClicked = false;
            isLongClick = false;
            elapsedTime = 0.0f;
        }
    }
}