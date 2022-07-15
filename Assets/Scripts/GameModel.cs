using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameModel : MonoBehaviour {
    const int TICKETS = 30;
    const float DAY_LENGTH = 180;

    public int day { get; set; };
    public float dayTimer { get; set; };
    
    public List<TicketModel> Tickets { get; set; }
    public float dailyTicketInterval { get; set; };

    public List<DieModel> dice { get; set; }

    public int followerCount { get; set; };

    // Use this for initialization
    void Start () {
        day = 1;
        dayTimer = DAY_LENGTH;
        dailyTicketInterval = DAY_LENGTH/(TICKETS * day);
        followerCount = 3;
        GenerateTickets();
    }
    
    // Update is called once per frame
    void Update () {
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

    public void CheckTicket(TicketModel ticket){
        
    }

    public void ResetTable(){
        
    }

    public void NextDay(){
        day++;
        dayTimer = DAY_LENGTH;
        dailyTickets = TICKETS * day;
    }

    public void GenerateTickets(){
        
    }
}