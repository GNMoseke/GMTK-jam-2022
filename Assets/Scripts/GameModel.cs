using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameModel : MonoBehaviour
{
    // start at 20, increas by 10 each day
    const int INITIAL_TICKETS = 20;
    const float DAY_LENGTH = 90;
    const int VICTORY_COUNT = 200;

    public int day { get; set; }
    public float dayTimer { get; set; }

    public TextAsset ticketsCSV;
    public GameObject ticketPrefab;
    public GameObject dicePrefab;
    public List<TicketModel> tickets { get; set; }
    public float dailyTicketInterval { get; set; }
    private HashSet<GameObject> activeTickets;

    public int followerCount { get; set; }

    public GameObject ticketDispenser;
    public GameObject ticketSpawnPosition;
    public GameObject successParticles;
    public GameObject failParticles;
    public GameObject diceDestroySparks;
    public GameObject diceDestroySmoke;
    public Leaderboard leaderboard;
    public TMPro.TMP_Text followerText;
    public TMPro.TMP_Text ticketsRemainingText;

    public AudioSource printerClip;
    public GameObject victoryBoard;

    public float ticketTimer;

    int ticketIndex = 0;
    public float shootForce;

    private int dailyTicketsRemaining;

    public bool betweenDays;

    // Use this for initialization
    void Start()
    {
        activeTickets = new HashSet<GameObject>();
        tickets = TicketParser.ReadTickets(ticketsCSV);
        // randomize ticket order
        Shuffle(tickets);
        day = 0;
        followerCount = 3;
        UpdateUI();
        NextDay();
        this.victoryBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!betweenDays)
        {
            // there's still tickets in the queue, tick the timer to fire the next one
            if (dailyTicketsRemaining > 0)
            {
                ticketTimer += Time.deltaTime;
                if (ticketTimer >= dailyTicketInterval)
                {
                    print($"using ticket at idx {ticketIndex}");
                    GenerateTicket(tickets[ticketIndex]);
                    ticketIndex++;
                    ticketTimer = 0;
                    dailyTicketsRemaining--;
                    if (dailyTicketsRemaining >= 0)
                    {
                        ticketsRemainingText.text = dailyTicketsRemaining.ToString();
                    }
                }
            }
            else // DAY FINISHED
            {
                print("no more tickets in queue, attempting to finish round");
                // if there are still active tickets, wait for those to be done before we move on
                if (activeTickets.Count == 0) {
                // 1) look up at leaderboard
                // 2) click continue
                betweenDays = true;
                Camera.main.GetComponent<CameraRotate>().StartRotation(false, true);
                // check if the player just won
                if (followerCount > VICTORY_COUNT)
                {
                    leaderboard.gameObject.SetActive(false);
                    victoryBoard.SetActive(true);
                }
                ResetTable();
                }
                else {
                    print($"still {activeTickets.Count} ticketts active, waiting");
                }
            }
        }
    }

    public void ResetTable()
    {
        DeleteRemainingDiceAndVFX();
        // Activate scoreboard, pause game
        Time.timeScale = 0;
    }


    private void DeleteRemainingDiceAndVFX()
    {
        // who likes array concatenation anyways
        GameObject[] remainingDice = GameObject.FindGameObjectsWithTag("Die");
        GameObject[] remainingVFX = GameObject.FindGameObjectsWithTag("VFX");
        foreach (GameObject die in remainingDice)
        {
            GameObject.Destroy(die);
        }
        foreach (GameObject vfx in remainingVFX)
        {
            GameObject.Destroy(vfx);
        }
    }

    public void NextDay()
    {
        activeTickets.Clear();
        day++;
        dayTimer = DAY_LENGTH;
        int specialDiceCounter = 4 + day;
        dailyTicketsRemaining = (INITIAL_TICKETS + (10 * (day - 1)));
        int randomDice = dailyTicketsRemaining - 2*specialDiceCounter;
        dayTimer = DAY_LENGTH;
        dailyTicketInterval = DAY_LENGTH / dailyTicketsRemaining;
        ticketTimer = dailyTicketInterval;

        Time.timeScale = 1f;
        betweenDays = false;
        DiceManager.GenerateDice(randomDice, dicePrefab, specialDiceCounter);
    }
    public void GenerateTicket(TicketModel ticket)
    {
        GameObject ticketGameObj = Instantiate(ticketPrefab, ticketSpawnPosition.transform.position, Quaternion.Euler(0, -35f, 0));
        ticketGameObj.GetComponent<Rigidbody>().AddForce(-ticketDispenser.transform.up * shootForce);
        ticketGameObj.AddComponent<TicketModel>();
        ticketGameObj.GetComponent<TicketModel>().InstantiateTicket(ticket);
        printerClip.Play();
        activeTickets.Add(ticketGameObj);

        ticketGameObj.GetComponent<TicketModel>().ticketCompletion += TicketComplete;
    }

    public void TicketComplete(bool success, int severity, Vector3 loc, GameObject completedTicket)
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
        activeTickets.Remove(completedTicket);
        Instantiate(diceDestroySmoke, loc, Quaternion.identity);
        Instantiate(diceDestroySparks, loc, Quaternion.identity);
        UpdateUI();
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    void UpdateUI()
    {
        leaderboard.UpdateLeaderboard(followerCount);
        followerText.text = followerCount.ToString();
    }

    private static System.Random rng = new System.Random();

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}