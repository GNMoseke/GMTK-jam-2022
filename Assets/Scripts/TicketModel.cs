using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketModel : MonoBehaviour
{
    // The age old law: when in doubt, throw a dictionary at it
    private Dictionary<GameObject, float> coyoteTimes;

    // 1 2 or 3. Was an enum but overcomplicated for something so simple
    int ticketSeverity;
    float timeToComplete;
    float timeRemaining;
    int rollNeeded { get; set; }
    bool below { get; set; }
    bool timerRunning = false;
    string plea { get; set; }
    float coyoteTime;
    Image mask;
    public TMPro.TMP_Text pleaTextObj;

    public delegate void NotifyTicketComplete(bool success, int severity, Vector3 loc);
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
        this.coyoteTime = 0.25f;
        this.timeRemaining = timeToComplete;

        this.timerRunning = true;
        this.coyoteTimes = new Dictionary<GameObject, float>();
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

// TODO: bottom of tickets causes problems
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Die")
        {
            // add the die to the coyote times tracker if we can
            this.coyoteTimes.TryAdd(collision.gameObject, coyoteTime);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // while the die is touching, subtract from its coyote time
        if (collision.gameObject.tag == "Die")
        {
            float newTime = this.coyoteTimes[collision.gameObject] - Time.deltaTime;
            if (newTime <= 0)
            {
                ResolveTicket(collision.gameObject);
            }
            this.coyoteTimes[collision.gameObject] = newTime;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Die")
        {
            print($"setting coyote time for ${collision.gameObject} to {this.coyoteTime}");
            // FIXME: slightly dangerous, error handling prob good.
            this.coyoteTimes[collision.gameObject] = this.coyoteTime;
        }
    }

    void ResolveTicket(GameObject die)
    {
        print($"resolving ticket with die {die}...");
        if (die != null)
        {
            this.timerRunning = false;
            var temp = (this.below) ? "MUST BE LOWER THAN" : "MUST BE HIGHER OR EQUAL TO";
            Debug.Log($"{temp} {this.rollNeeded}");
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
        ticketCompletion.Invoke(true, this.ticketSeverity, this.transform.position);
        GameObject.Destroy(gameObject);
        if (die != null)
        {
            GameObject.Destroy(die);
        }
    }
    public virtual void OnTicketFailed(GameObject die)
    {
        ticketCompletion.Invoke(false, this.ticketSeverity, this.transform.position);
        GameObject.Destroy(gameObject);
        if (die != null)
        {
            GameObject.Destroy(die);
        }
    }
}
