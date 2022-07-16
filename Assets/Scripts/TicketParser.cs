using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TicketParser : MonoBehaviour
{
    public static List<TicketModel> ReadTickets(TextAsset ticketCSV) {
        List<TicketModel> tickets = new List<TicketModel>();
        List<string> ticketRows = new List<string>(ticketCSV.text.Split('\n'));
        // drop header row
        ticketRows.RemoveAt(0);
        foreach(string ticketRaw in ticketRows) {
            string[] components = ticketRaw.Split(',');
            int rollNeeded = int.Parse(components[1]);
            bool below = bool.Parse(components[2]);
            int severity = int.Parse(components[3]);

            tickets.Add(new TicketModel(components[0], rollNeeded, below, severity));
        }

        return tickets;
    }
}
