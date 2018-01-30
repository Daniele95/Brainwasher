using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour {



    public enum PlayerNumber
    {

        Player1, Player2

    };

    public Material onda;
    public float caricaArma;
    public float tempo;
    public float tempoMult = 1.0f;
    public float multScalaCollider = 1.0f;
    public float multScalaOnda = 1.0f;
    public GameObject effetto;
    public string playerNumber;
    private Renderer rend;
    public List<Zombie> zombie;


    public void setCaricaArma(float caricaArma)
    {

        this.caricaArma = caricaArma;

    }

    public float sizeRatio = 0.8f;

    public float getCaricaArma()
    {

        return caricaArma;

    }

    public void setTempo(float tempo)
    {
        tempo *= tempoMult;
        this.tempo = tempo;

    }

    public float getTempo()
    {

        return tempo;

    }

    public void setPlayerNumber(string playerNumber)
    {
        this.playerNumber = playerNumber;
    }



    // Use this for initialization
    void Start () {
        zombie = new List<Zombie>();

        StartCoroutine(LerpUp());

    }

    IEnumerator LerpUp()
    {
        float progress = 0;

        Vector3 FinalScale = new Vector3(tempo * sizeRatio, tempo * sizeRatio, tempo * sizeRatio);
        GameObject antenna = GameObject.Find("Antenna");

        while (progress <= 1)
        {
            antenna.transform.localScale = Vector3.Lerp(Vector3.zero, FinalScale, progress*4);
            progress += Time.deltaTime * sizeRatio * 1.5f;
            yield return null;
        }
        antenna.transform.localScale = FinalScale;

    }

    public void setSphereColliderRadius(float carica)
    {
        gameObject.GetComponent<SphereCollider>().radius = carica * multScalaCollider;
       // gameObject.GetComponentInChildren<Renderer>().material.SetFloat("dimensione",0.0f);
        Material mat  = new Material(Shader.Find("onda"));
        //mat.SetFloat("dimensione", carica);
        //Debug.Log("Mat get float: " + mat.GetFloat("dimensione"));
        mat.SetFloat("dimensione", (carica*multScalaCollider/17f));

        //Debug.Log("Mat get float: " + mat.GetFloat("dimensione"));
        gameObject.GetComponentInChildren<Renderer>().material = mat;
        //Debug.Log("material.getfloat: "+gameObject.GetComponentInChildren<Renderer>().material.GetFloat("dimensione"));

        // Debug.Log(myfloat);
        //Debug.Log("Dimensione: "+ gameObject.GetComponentInChildren<Renderer>().material.GetFloat("dimensione"));
    }

	// Update is called once per frame
	void Update () {

		if(tempo > 0)
        {

            tempo -= Time.deltaTime;

        } else
        {

            GameObject.Destroy(this.gameObject);

        }

	}



   

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Zombie")
        {
            print("uno zombie è entrato");
            collision.gameObject.GetComponent<Zombie>().setIsAttracted(true);
            collision.gameObject.GetComponent<Zombie>().setAttractorPos(transform);
            zombie.Add(collision.gameObject.GetComponent<Zombie>());
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Zombie")
        {
            print("uno zombie è uscito");
            collision.gameObject.GetComponent<Zombie>().setIsAttracted(false);
            collision.gameObject.GetComponent<Zombie>().setChangeDirection(true);
            zombie.Remove(collision.gameObject.GetComponent<Zombie>());
        }
    }

    void OnDestroy()
    {
        foreach (Zombie obj in zombie)
        {
            obj.setIsAttracted(false);
            obj.setChangeDirection(true);
        }

    }




}
