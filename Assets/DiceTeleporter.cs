using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTeleporter : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Die")
        {
            collision.gameObject.transform.position = DiceManager.standardSpawnPosition;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (collision.gameObject.tag == "Ticket")
        {
            collision.gameObject.GetComponent<TicketModel>().OnTicketFailed(null);
        }
    }
}
