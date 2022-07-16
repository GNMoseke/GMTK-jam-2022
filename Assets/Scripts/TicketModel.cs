using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketModel : MonoBehaviour
{
    // 1 2 or 3. Was an enum but overcomplicated for something so simple
    int ticketSeverity;
    float timeToComplete;
    float timeRemaining;
    int rollNeeded { get; set; }
    bool below { get; set; }
    bool timerRunning = false;
    string plea { get; set; }

    Image mask;
    public TMPro.TMP_Text pleaTextObj;

    public delegate void NotifyTicketComplete(bool success, int severity);
    public event NotifyTicketComplete ticketCompletion;

    public TicketModel(string plea, int rollNeeded, bool below, int severity)
    {
        this.ticketSeverity = severity;
        this.rollNeeded = rollNeeded;
        this.below = below;
        this.plea = plea;
    }

    public void InstantiateTicket(TicketModel model)
    {
        this.ticketSeverity = model.ticketSeverity;
        this.rollNeeded = model.rollNeeded;
        this.below = model.below;
        this.plea = model.plea;

        this.pleaTextObj = this.GetComponentInChildren<TMPro.TMP_Text>();
        Debug.Log(pleaTextObj);
        // FIXME multiple images might screw this
        List<Image> images = new List<Image>(this.GetComponentsInChildren<Image>());
        foreach (Image img in images)
        {
            if (img.gameObject.name == "Mask")
            {
                this.mask = img;
                break;
            }
        }

        this.pleaTextObj.text = model.plea;
        this.timeToComplete = 10f;
        this.timeRemaining = timeToComplete;

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
        GameObject.Destroy(gameObject);
    }
}
