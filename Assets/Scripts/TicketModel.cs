using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void NotifyTicketComplete(bool success, Severity severity);

public class TicketModel : MonoBehaviour
{
    Severity ticketSeverity;
    public float timeRemaining = 10f;
    int rollNeeded;
    bool below;
    bool timerRunning = false;
    Text plea;

    public event NotifyTicketComplete ticketCompletion;

    TicketModel(Severity severity, int rollNeeded, bool below, Text plea)
    {
        this.ticketSeverity = severity;
        this.rollNeeded = rollNeeded;
        this.below = below;
        this.plea = plea;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.timerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                this.timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("ticket timed out");
                OnTicketFailed();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // TODO: magic string
        if (collision.gameObject.tag == "die")
        {
            this.timerRunning = false;
            // if we need below and the die is below what we need, succeed
            if (this.below && collision.gameObject.GetComponent<DieModel>().value < this.rollNeeded)
            {
                OnTicketSucceeded();
            }
            // if we need above and the die is greater or equal to what we need, succeed
            else if (!this.below && collision.gameObject.GetComponent<DieModel>().value >= this.rollNeeded)
            {
                OnTicketSucceeded();
            }
            else
            {
                OnTicketFailed();
            }
        }
    }

    protected virtual void OnTicketSucceeded()
    {
        // TODO: play success anim & destroy self/die
        ticketCompletion.Invoke(true, this.ticketSeverity);
    }
    protected virtual void OnTicketFailed()
    {
        // TODO: play fail anim & destroy self/die
        ticketCompletion.Invoke(false, this.ticketSeverity);
    }
}

public enum Severity : int
{
    Low = 1,
    Medium = 2,
    High = 3
}
