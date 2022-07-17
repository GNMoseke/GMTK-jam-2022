using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTeleporter : MonoBehaviour
{
    public GameObject dice;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Die")
        {
            GameObject.Destroy(collision.gameObject);
            GameObject newDice = Instantiate(dice, new Vector3(38.0f, 25.0f, 15.0f), dice.transform.rotation);
            int num = Random.Range(2, 20);
            newDice.GetComponent<DieModel>().value = num;
            newDice.name = num.ToString();
            newDice.GetComponent<Renderer>().material.SetFloat("_Number", num);
            newDice.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-25.0f, -15.0f), 0.0f, Random.Range(-20.0f, -10.0f));
        }
        if (collision.gameObject.tag == "Ticket")
        {
            collision.gameObject.GetComponent<TicketModel>().OnTicketFailed(null);
        }
    }
}
