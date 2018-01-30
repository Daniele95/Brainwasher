using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
/*
Character:
	Attributi:
		int punteggio
		int energy
		float speed
		float caricaArma
		bool isCharging
		bool isShooting
		Vector3 respawnPoint
		String name
	Metodi:
		Move()
		
	documentazione:
Movimento : il player puo accellerare (rigidbody.addForce()).
Respawn : se il player mette piede nell’altra parte del campo respawna al                              
       punto iniziale
		ChargeImpulse : se il player tieni premuto il trigger aumenta caricaArma e
				   diminuisce energy , quando lascia il trigger caricaArma				  	 , playerPos e name vengono passati al costruttore del 
   trasmettitore.
		Boundary : Il player non puo uscire da un campo predefinito (se non per
			        andare nel campo dell’avversario dove si verifica un respawn)
		Collision: il player ignora le collisioni con : Persona , Trasmettitore.
			      calcola invece le collisioni con : Campo
		Ricarica Energia : l’energia si ricarica costantemente , mentre il player è
        in fase di ChargeImpulse l’energia non si ricarica , se 
        l’energia è minore di n il player non puo entrare in fase di
			                    ChageImpulse

 * 
 * */
public class Character : MonoBehaviour {


	/*
	 * 
	 * 
	 * =============================================================================
	 * ATTRIBUTES
	 * ============================================================================
	 * 
	 * 
	 * */


	public enum PlayerNumber{

		Player1, Player2

	};

	public int id;
	public int punteggio;
	public float energy = 100f;
    public float visualizeEnergy = 100f;
    public float minimumEnergy = 20;
    public float maximumEnergy = 100;
	public float caricaArma;
	public bool isCharging;
	public bool isShooting;
	public Vector3 respawnPoint = Vector3.zero;

    public GameObject transmitter;
    
	protected Animator animator;
	private Rigidbody myBody;
	private AudioSource audioPlayer;
    public AudioClip[] clips;

	public float animatorSpeed = 1.5f;
	public float movementSpeed = 1.5f;
	public float rotationSpeed = 1.5f;

	private Vector3 moveDirection = Vector3.zero;

	public PlayerNumber playerNumber;
	private string joystickNumber;
    
    private float downTime, upTime, pressTime = 0;
    public float countDown = 2.0f;
    private bool ready = false;
    public float multEnergy = 2.0f;

    public float rechargeRatio = 2.0f;

    public float minimumTransmitterSize = 0;
    public float maximumTransmitterSize = 5;

    public float multCost = 2.0f;
    public GameObject visualizer;
    /*
	 * 
	 * 
	 * =============================================================================
	 * METHODS
	 * ============================================================================
	 * 
	 * 
	 * */

    // Use this for initialization
    void Start () {
        visualizer = transform.Find("Visualizer").gameObject;
		myBody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		audioPlayer = GetComponent<AudioSource> ();
		animator.speed = animatorSpeed;

        respawnPoint = transform.position;
        visualizer.SetActive(false);
        maximumEnergy = energy;


     

    }
	
	// Update is called once per frame
	void Update () {


        UpdateHumanInput();
	}
    


	public void UpdateHumanInput(){

        if (!isCharging)
        {
            visualizeEnergy = energy;
        }
        //to prettify


        if (playerNumber == PlayerNumber.Player1)
        {

            joystickNumber = "L";

        }
        else if (playerNumber == PlayerNumber.Player2)
        {

            joystickNumber = "R";



        }

        if (energy != 100) {

           // Debug.Log("P" + joystickNumber + ": " + energy);


        }


        //Keep restoring power bar until it reaches maximum

        chargeBar();

		Vector3 directionRaw = new Vector3(Input.GetAxis (joystickNumber+"Vertical"), 0 ,Input.GetAxis (joystickNumber+"Horizontal"));

        Vector3 direction = new Vector3(directionRaw.x, directionRaw.y, directionRaw.z);

        MoveCharacter (direction);

		Vector3 localDirection = transform.InverseTransformDirection (direction);


		if (localDirection.z > 0.1) {

            //audioPlayer.clip
			animator.SetBool ("isWalking", true);
            //playAudio(clips[0], true)
            audioPlayer.clip = clips[0];
            audioPlayer.loop = true;
            audioPlayer.Play();

		} else {

            audioPlayer.Stop();
            audioPlayer.loop = false;

            animator.SetBool ("isWalking", false);

		}

        if (Input.GetAxisRaw(joystickNumber + "Trigger") != 0 && ready == false && energy >= minimumEnergy)
        {
            visualizer.SetActive(true);
            //Debug.Log("Pressing on " + joystickNumber);
            isCharging = true;
            downTime = Time.time;
            pressTime = (downTime + countDown);
            ready = true;

        }else if(Input.GetAxisRaw(joystickNumber + "Trigger") != 0 && ready == true && energy >= minimumEnergy)
        {
            visualizeEnergy = energy - (((Time.time - downTime)*multEnergy)*multCost);
            if(visualizer.transform.localScale.x < 33 && visualizeEnergy >1) {
                visualizer.transform.localScale *= 1.01f;
            }
        }

        if (Input.GetAxisRaw(joystickNumber + "Trigger") == 0 && ready == true)
        {
            ready = false;
            pressTime = Time.time - downTime;
            //Debug.Log("Pressed for: " + pressTime + " on " + joystickNumber);

            //Debug.Log("Charging: "+pressTime);
            deploy(pressTime);
            visualizer.transform.localScale = new Vector3(10, 0.1f, 10);
            visualizer.SetActive(false);
        }



    }


    public void MoveCharacter(Vector3 moveDirection){

        //transform.Translate (moveDirection * movementSpeed * Time.deltaTime, Space.World);

        myBody.AddForce(moveDirection*movementSpeed);

        Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirection, rotationSpeed * Time.deltaTime, 0.0f);
        //Debug.DrawRay(transform.position, moveDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.forward = newDir;

	}

    public void deploy(float carica)
    {

        if(energy >= minimumEnergy)
        {

            //calculate how long the button has been pressed

            if (carica < minimumTransmitterSize)
            {

                carica = minimumTransmitterSize;

            }
            if (carica >= energy)
            {

                Debug.Log("Capped to max");
                carica = energy;

            }
            if(carica >= maximumTransmitterSize)
            {

                carica = maximumTransmitterSize;

            }
            carica *= multEnergy;

            energy -= (carica*multCost);

           
            //Generate Transmitter
            GameObject spanwedTransmitter = Instantiate(transmitter, this.transform.position, Quaternion.identity) as GameObject;
            spanwedTransmitter.GetComponent<Transmitter>().setCaricaArma(carica);
            spanwedTransmitter.GetComponent<Transmitter>().setTempo(carica);
            spanwedTransmitter.GetComponent<Transmitter>().setSphereColliderRadius(carica);
            spanwedTransmitter.GetComponent<Transmitter>().setPlayerNumber(playerNumber.ToString());

            //Debug.Log("Deployed on " + joystickNumber);


        } else
        {

           // Debug.Log("Could not deploy on " + joystickNumber);

        }

        

        isCharging = false;
        

    }

    public void chargeBar()
    {
        if (!isCharging)
        {
            if(energy < maximumEnergy)
            {

                energy += rechargeRatio;

            }
            
        }

        if(energy >= maximumEnergy)
        {

            energy = maximumEnergy;

        }

        if(energy < 0)
        {

            energy = 0;

        }
    }

    public void respawn()
    {
        //gameObject.SetActive(false);

        transform.position = respawnPoint;

        //gameObject.SetActive(true);

        //Block Commands
    }

    public void playAudio(AudioClip clip, bool loop)
    {

        if (loop)
        {

            audioPlayer.loop = true;

        }

        audioPlayer.clip = clip;
        audioPlayer.Play();
    }

}
