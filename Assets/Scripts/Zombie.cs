using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    //Valori pubblici
    public float minTime = 1.0f;
    public float maxTime = 7.0f;
    public float speed = 2.0f;

    //Valori logici
    private bool isAttracted = false;
    private bool changeDirection = true;
    private bool isInConflict = false;
    public float attractValue;
    private float randomTime;
    private Rigidbody rb;
    private Transform attractor;
    private Ray personaRay;
    private RaycastHit hit;
    float distance;
    private bool stopCoroutine = false;
    public int tester = 0;
    public IEnumerator mycor;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttracted)
        {
            distance = 0;
            if (changeDirection)
            {
                StartCoroutine(CambiaDirezione());
            }
        }
        else
        {
            if (stopCoroutine == false)
            {
                stopCoroutine = true;
                StopCoroutine(CambiaDirezione());
            }
            personaRay = new Ray(transform.position, attractor.position - transform.position);
            //Debug.DrawRay(transform.position, attractor.position - transform.position, Color.green);
            if (Physics.Raycast(personaRay, out hit, 2000, 12))
            {   //RAYCAST LAYER DA MODIFICARE IL 13
                distance = hit.distance;
                attractValue = 1 / distance;
            }

            transform.LookAt(attractor);

        }
        //rb.velocity = transform.forward * speed;
        rb.position += transform.forward * speed * Time.deltaTime;

        if (rb.position.x<157||rb.position.z<-25||rb.position.z>25||rb.position.x>188) //numeri da cambiare
            transform.Rotate(Vector3.up, 180);

    }

    void OnCollisionEnter(Collision collision)
    {
        /*foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			transform.Rotate(Vector3.up, 180);
			transform.Translate(Vector3.forward);
		}*/
    }

    IEnumerator CambiaDirezione()
    {
        changeDirection = false;
        randomTime = Random.Range(minTime, maxTime);
        Quaternion randomRot = Quaternion.Euler(new Vector3(0, Random.Range(0, 359), 0));
        transform.rotation = randomRot;
        yield return new WaitForSeconds(randomTime);
        changeDirection = true;
        
    }

    public void setIsAttracted(bool valore)
    {
        isAttracted = valore;
    }

    public void setAttractorPos(Transform pos)
    {
        //mettere qua l'if per sapere se mettere un nuovo attractor
        attractor = pos;
    }

    public void setChangeDirection(bool valore)
    {
        changeDirection = valore;
    }


}


