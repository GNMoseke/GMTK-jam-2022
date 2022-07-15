using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void NotifyTicketComplete(bool success, int severity);

public class TicketModel : MonoBehaviour
{
    // 1 2 or 3. Was an enum but overcomplicated for something so simple
    int ticketSeverity;
    public float timeToComplete = 10f;
    public float timeRemaining;
    int rollNeeded;
    bool below;
    bool timerRunning = false;
    Text pleaText;
    string plea;

    public Image mask;

    public event NotifyTicketComplete ticketCompletion;

    public TicketModel(string plea, int rollNeeded, bool below, int severity)
    {
        this.ticketSeverity = severity;
        this.rollNeeded = rollNeeded;
        this.below = below;
        this.plea = plea;
        this.timeRemaining = timeToComplete;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.pleaText.text = this.plea;
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
                float fillAmt = timeRemaining / timeToComplete;
                mask.fillAmount = fillAmt;
            }
            else
            {
                timerRunning = false;
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
