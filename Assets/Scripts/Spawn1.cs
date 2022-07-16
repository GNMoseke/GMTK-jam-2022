using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn1 : MonoBehaviour
{
    public GameObject onePipe;
    public GameObject die;

    private bool rising = false;
    private int cnt = 0;

    public void OnMouseDown()
    {
        rising = true;
        cnt++;
    }

    public void FixedUpdate()
    {
        if (onePipe.transform.position.y > -70 && rising)
        {
            onePipe.GetComponent<Rigidbody>().useGravity = true;
            for (int i = 0; i < cnt; i++)
                SpawnDie();
            cnt = 0;
            rising = false;
        }

        if (onePipe.transform.position.y < -120)
        {
            onePipe.GetComponent<Rigidbody>().useGravity = false;
            onePipe.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (rising)
            onePipe.GetComponent<Rigidbody>().AddForce(-Physics.gravity * onePipe.GetComponent<Rigidbody>().mass);
    }

    private void SpawnDie()
    {
        GameObject newDice = Instantiate(die, new Vector3(25.0f, 25.0f, 30.0f), die.transform.rotation);
        newDice.GetComponent<Renderer>().material.SetFloat("_Number", 1);
        newDice.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-15000.0f, 0.0f), 0.0f, -15000.0f));
    }
}
