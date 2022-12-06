using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public GameObject go;
    public SoundManager theSound;

    public string call_sound;
    public string cancel_sound;

    public OrderManager theOrder;

    private bool activated;

    private StandardInput theStandard;

    void Start()
    {
        theStandard = FindObjectOfType<StandardInput>();
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Continue()
    {
        activated = false;
        go.SetActive(false);
        theSound.Play(cancel_sound);
        Time.timeScale = 1;
        theStandard.cursorLocked = true;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            if(activated)
            {
                go.SetActive(true);
                theSound.Play(call_sound);
                //Time.timeScale = 0;
                theStandard.cursorLocked = false;
            }
            else
            {
                go.SetActive(false);
                theSound.Play(cancel_sound);
                //Time.timeScale = 1;
                theStandard.cursorLocked = true;
            }
        }
    }
}
