using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    private Fade fade;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fade = FindObjectOfType<Fade>();
            fade.StartFade();
            UIManager.isSecond = true;
            SceneManager.LoadScene("underground");
            Debug.Log("Go Underground");
        }
    }
}
