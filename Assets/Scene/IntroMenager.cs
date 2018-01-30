using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenager : MonoBehaviour {

    

    // Use this for initialization
    void Start () {
        StartCoroutine(nextScene());
    }
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator nextScene()
    {
        yield return new WaitForSeconds(20.0f);
        SceneManager.LoadScene("scena1", LoadSceneMode.Single);
    }
}
