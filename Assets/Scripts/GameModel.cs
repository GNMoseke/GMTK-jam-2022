using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameModel : MonoBehaviour
{
    const int TICKETS = 30;
    const float DAY_LENGTH = 180;

    public int day { get; set; }
    public float dayTimer { get; set; }

    public TextAsset ticketsCSV;
    public GameObject ticketPrefab;
    public GameObject dicePrefab;
    public List<TicketModel> tickets { get; set; }
    public float dailyTicketInterval { get; set; }

    public int followerCount { get; set; }

    public GameObject ticketDispenser;
    public GameObject ticketSpawnPosition;

    public float ticketTimer;

    public int numActiveTickets;
    public float shootForce;

    public int nat20Counter;
    public int nat1Counter;

    // Use this for initialization
    void Start()
    {
        int dailyTickets = (TICKETS * day);
        int randomDice = dailyTickets-nat1Counter-nat20Counter
        day = 1;
        dayTimer = DAY_LENGTH;
        dailyTicketInterval = DAY_LENGTH / dailyTicket;
        followerCount = 3;
        shootForce = 1000f
        nat20Counter = 5
        nat1Counter = 5
        tickets = TicketParser.ReadTickets(ticketsCSV);
        Debug.Log(tickets.Count);
        GenerateTicket(tickets[0]);

        ticketTimer = 0;
        numActiveTickets = 1;
        DiceManager.GenerateDice(randomDice, dicePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (dayTimer > 0)
        {
            this.dayTimer -= Time.deltaTime;
            ticketTimer += Time.deltaTime;
            if (ticketTimer >= dailyTicketInterval)
            {
                GenerateTicket(tickets[numActiveTickets]);
                numActiveTickets++;
                ticketTimer = 0;
            }
        }
        else // DAY FINISHED
        {
            // 1) look up at leaderboard
            // 2) click continue

            ResetTable();
            NextDay();
        }
        /*
        dec dayTimer
        if(dayTimer > 0)
             if(need to print a ticket)
                 print a ticket
             loop all tickets on table
                 if ticket expired or ticket failed
                     lose followers
                 else if ticket passed
                     gain followers
                     lose dice

         else
             reset table
             -----maybe do some other shit----
             start nextDay
        */


    }

    public void CheckTicket(TicketModel ticket)
    {

    }

    public void ResetTable()
    {
        // TODO: re-initialize dice and add extra nat 20's and nat 1's (based on performance of previous day)
    }

    public void NextDay()
    {
        day++;
        dayTimer = DAY_LENGTH;
    }

    public void GenerateTicket(TicketModel ticket)
    {
        GameObject ticketGameObj = Instantiate(ticketPrefab, ticketSpawnPosition.transform.position,  Quaternion.Euler(0, -35f, 0));
        ticketGameObj.GetComponent<Rigidbody>().AddForce(-ticketDispenser.transform.up * shootForce);
        ticketGameObj.AddComponent<TicketModel>();
        ticketGameObj.GetComponent<TicketModel>().InstantiateTicket(ticket);

        ticketGameObj.GetComponent<TicketModel>().ticketCompletion += TicketComplete;
    }

    public void TicketComplete(bool success, int severity)
    {
        if (success)
        {
            followerCount += severity;
        } 
        else
        {
            followerCount -= severity;
        }
    }
}