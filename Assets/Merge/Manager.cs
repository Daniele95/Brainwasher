using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Manager : MonoBehaviour {



    public enum PlayerNumber
    {

        Player1, Player2

    };

    public Field campo1;
    public Field campo2;

    public Character character1;
    public Character character2;

    private bool isRunning = false;
    public float stopTime = 1.0f;
    public int goal = 50;
    public bool firstHalf = false;
    public bool firstHalcor = false;
    public bool began = false;
     public Text alert;
    public Text LScore;
    public Text RScore;
    //public Text LCharge;
    //public Text RCharge;
    public Slider LSlider;
    public Slider RSlider;
    public GameObject RPlayer;
    public GameObject LPlayer;
    public GameObject PlayerR;
    public GameObject PlayerL;
    public float display = 2;
    public GameObject[] pubblicita;
    Character winner;
    public GameObject PlayAgain;
    bool gameWinned = false;

    public GameObject[] endings;
    public GameObject Intro;

    IEnumerator AssignPoint()
    {
        isRunning = true;
        foreach(GameObject zombie in campo1.zombieList)
        {
            character1.punteggio++;
        }
        foreach (GameObject zombie in campo2.zombieList)
        {
            character2.punteggio++;
        }
        yield return new WaitForSeconds(2f);
        isRunning = false;
    }

    // Use this for initialization
    void Start () {

        StartCoroutine(beginGame());
		
	}
	
	// Update is called once per frame
	void Update () {

        //Score Display

        LScore.text = character1.punteggio.ToString();
        RScore.text = character2.punteggio.ToString();

        /*
        LCharge.text = character1.energy.ToString();
        RCharge.text = character2.energy.ToString();
        */
        LSlider.value = character1.energy/100;
        RSlider.value = character2.energy/100;

        if (began)
        {

            if (!isRunning)
                StartCoroutine(AssignPoint());

            if (campo1.otherPlayerIn)
            {

                character2.respawn();
                StartCoroutine(blockCommands(campo1, character2));


            }

            if (campo2.otherPlayerIn)
            {

                character1.respawn();
                StartCoroutine(blockCommands(campo2, character1));
            }

            //mid game animation

            if ((character1.punteggio >= goal / 2 || character2.punteggio >= goal / 2) && firstHalf == false)
            {

                //fai partire animazione
                print("ho settato firsthal to true");
                firstHalf = true;
                //StartCoroutine(Alert("Half Way"));

            }

            //check if game has finished

            if (character1.punteggio >= goal && !gameWinned)
            {

                //StartCoroutine(Alert("Player 1 wins"));
                //disabilita UI e abilita video
                character1.gameObject.SetActive(false);
                character2.gameObject.SetActive(false);
                PlayAgain.gameObject.SetActive(true);
                endings[0].SetActive(true);
                gameWinned = true;
                
            }

            if (character2.punteggio >= goal && !gameWinned)
            {

                character1.gameObject.SetActive(false);
                character2.gameObject.SetActive(false);
                PlayAgain.gameObject.SetActive(true);
                PlayerR.SetActive(false);
                PlayerL.SetActive(false);
                endings[1].SetActive(true);
                gameWinned = true;
            }

            if (firstHalf)
            {
                if (!firstHalcor)   //se firsthalf coroutine non è ancora partito
                    print("ho fatto partire la coroutine halfWay");
                    StartCoroutine(halfWay(0));

                //firstHalf = false;

            }
            //play animation with winner player







        }

    }

    public void respawn(Character character)
    {

        character.respawn();

    }

    IEnumerator halfWay(int randomAD)
    {

        firstHalcor = true;
        print("ho settato firsthalf core to true");
        pubblicita[0].SetActive(true);
        yield return new WaitForSeconds(3);
        print("ho finito l'halfway courtine e setto la publblicita to vero ");
        pubblicita[0].SetActive(false);

    }

    IEnumerator Alert(string message)
    {
        alert.enabled = true;

        alert.text=message;

        yield return new WaitForSeconds(display);

        alert.text = "";
        alert.enabled = false;

    }

    IEnumerator blockCommands(Field campo, Character character)
    {
        character.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(stopTime);
        campo.otherPlayerIn = false;
        character.GetComponent<Animator>().enabled = true;

    }

    IEnumerator beginGame()
    {
        character1.gameObject.SetActive(false);
        character2.gameObject.SetActive(false);

        //display ready
        alert.text = "Get Ready";
        yield return new WaitForSeconds(2);

        alert.text = "Set";
        yield return new WaitForSeconds(2);

        alert.text = "Go";
        yield return new WaitForSeconds(2);
        //Intro.SetActive(true);
        /*Debug.Log("Intro");
        while (Intro.GetComponent<VideoPlayer>().isPlaying) // while the movie is playing
        {
            yield return new WaitForSeconds(0.01f);
        }

        Intro.SetActive(false);
        */alert.enabled = false;

        //disable text

        /* character1.GetComponent<Character>().enabled = true;
         character2.GetComponent<Character>().enabled = true;
         */

        character1.gameObject.SetActive(true);
        character2.gameObject.SetActive(true);
        began = true;
    }

   
    public void resetScene()
    {
        SceneManager.LoadScene("scena1", LoadSceneMode.Single);
    }
}
