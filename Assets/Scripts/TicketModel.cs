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
    float coyoteTime;
    bool pendingDestroy;

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
        // this is some baaaad code but its a game jam :)
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
        this.coyoteTime = 0.5f;
        this.timeRemaining = timeToComplete;
        this.pendingDestroy = false;

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
                OnTicketFailed(null);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Die")
        {
            StartCoroutine(CoyoteTime(collision.gameObject));
        }
    }

    void OnCollisionExit(Collision collision)
    {
        this.pendingDestroy = false;
    }

    IEnumerator CoyoteTime(GameObject die)
    {
        this.pendingDestroy = true;
        yield return new WaitForSecondsRealtime(coyoteTime);
        if (this.pendingDestroy)
        {
            this.timerRunning = false;
            // if we need below and the die is below what we need, succeed
            if (this.below && die.GetComponent<DieModel>().value < this.rollNeeded)
            {
                OnTicketSucceeded(die);
            }
            // if we need above and the die is greater or equal to what we need, succeed
            else if (!this.below && die.GetComponent<DieModel>().value >= this.rollNeeded)
            {
                OnTicketSucceeded(die);
            }
            else
            {
                OnTicketFailed(die);
            }
        }
    }

    protected virtual void OnTicketSucceeded(GameObject die)
    {
        // TODO: play success effect
        ticketCompletion.Invoke(true, this.ticketSeverity);
        GameObject.Destroy(gameObject);
        if (die != null)
        {
            GameObject.Destroy(die);
        }
    }
    public virtual void OnTicketFailed(GameObject die)
    {
        // TODO: play fail effect
        ticketCompletion.Invoke(false, this.ticketSeverity);
        GameObject.Destroy(gameObject);
        if (die != null)
        {
            GameObject.Destroy(die);
        }
    }
}
