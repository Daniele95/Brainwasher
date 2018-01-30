using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieManager : MonoBehaviour {

    private GameObject displayer;
    private MovieTexture texture;

	// Use this for initialization
	void Start () {

        texture = 
        ((MovieTexture)displayer.GetComponent<Renderer>().material.mainTexture);

        texture.Play();

	}
	
	// Update is called once per frame
	void Update () {
		
        

	}
    
}
