using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    public int numeroCampo;
    public List<GameObject> zombieList;

    public bool otherPlayerIn = false;
    public Color color;

    // Use this for initialization
    void Start()
    {

        zombieList = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie")
        {
            zombieList.Add(other.gameObject);

        }
        else if (other.tag == "Character")
        {
            if (numeroCampo == 1 && other.gameObject.GetComponent<Character>().id == 2 || numeroCampo == 2 && other.gameObject.GetComponent<Character>().id == 1)
            {
                otherPlayerIn = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Zombie")
        {
            zombieList.Remove(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        foreach (GameObject obj in zombieList)
        {
            obj.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_EmissionColor", color*2);
        }
    }
}


