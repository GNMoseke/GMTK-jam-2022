using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameModel : MonoBehaviour
{
    // start at 20, increas by 10 each day
    const int TICKETS = 20;
    const float DAY_LENGTH = 120;
    const int VICTORY_COUNT = 200;

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
    public GameObject successParticles;
    public GameObject failParticles;
    public GameObject diceDestroySparks;
    public GameObject diceDestroySmoke;
    public Leaderboard leaderboard;
    public TMPro.TMP_Text followerText;
    public AudioSource printerClip;

    public float ticketTimer;

    int ticketIndex = 0;
    public float shootForce;

    public int nat20Counter;
    public int nat1Counter;


    public bool betweenDays;

    // Use this for initialization
    void Start()
    {
        tickets = TicketParser.ReadTickets(ticketsCSV);
        // randomize ticket order
        Shuffle(tickets);
        day = 0;
        followerCount = 3;
        UpdateUI();
        NextDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!betweenDays)
        {
            if (dayTimer > 0)
            {
                this.dayTimer -= Time.deltaTime;
                ticketTimer += Time.deltaTime;
                if (ticketTimer >= dailyTicketInterval)
                {
                    print($"using ticket at idx {ticketIndex}");
                    GenerateTicket(tickets[ticketIndex]);
                    ticketIndex++;
                    ticketTimer = 0;
                }
            }
            else // DAY FINISHED
            {
                // 1) look up at leaderboard
                // 2) click continue
                betweenDays = true;
                Camera.main.GetComponent<CameraRotate>().StartRotation(false, true);
                // check if the player just won
                if (followerCount > VICTORY_COUNT) 
                {

                }
                ResetTable();
            }
        }
    }

    public void ResetTable()
    {
        DeleteRemainingDice();
        // Activate scoreboard, pause game
        Time.timeScale = 0;
    }


    private void DeleteRemainingDice()
    {
        GameObject[] remainingDice = GameObject.FindGameObjectsWithTag("Die");
        foreach (GameObject die in remainingDice)
        {
            GameObject.Destroy(die);
        }
    }

    public void NextDay()
    {
        day++;
        dayTimer = DAY_LENGTH;
        nat20Counter = 4 + day;
        nat1Counter = 4 + day;
        int dailyTickets = (TICKETS + (10 * (day - 1)));
        int randomDice = dailyTickets - nat1Counter - nat20Counter;
        dayTimer = DAY_LENGTH;
        dailyTicketInterval = DAY_LENGTH / dailyTickets;
        ticketTimer = dailyTicketInterval - 1f;

        Time.timeScale = 1f;
        betweenDays = false;
        DiceManager.GenerateDice(randomDice, dicePrefab);
    }
    public void GenerateTicket(TicketModel ticket)
    {
        GameObject ticketGameObj = Instantiate(ticketPrefab, ticketSpawnPosition.transform.position, Quaternion.Euler(0, -35f, 0));
        ticketGameObj.GetComponent<Rigidbody>().AddForce(-ticketDispenser.transform.up * shootForce);
        ticketGameObj.AddComponent<TicketModel>();
        ticketGameObj.GetComponent<TicketModel>().InstantiateTicket(ticket);
        printerClip.Play();

        ticketGameObj.GetComponent<TicketModel>().ticketCompletion += TicketComplete;
    }

    public void TicketComplete(bool success, int severity, Vector3 loc)
    {
        if (success)
        {
            GameObject.Instantiate(successParticles, loc, Quaternion.identity);
            followerCount += severity;
        }
        else
        {
            GameObject.Instantiate(failParticles, loc, Quaternion.identity);
            followerCount -= severity;
            if (followerCount < 0)
            {
                followerCount = 0;
            }
        }
        Instantiate(diceDestroySmoke, loc, Quaternion.identity);
        Instantiate(diceDestroySparks, loc, Quaternion.identity);
        UpdateUI();
    }

    void UpdateUI()
    {
        leaderboard.UpdateLeaderboard(followerCount);
        followerText.text = followerCount.ToString();
        // TODO: scale better
        //followerSlider.value = (float)followerCount / 100f;
    }

    private static System.Random rng = new System.Random();  

    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}