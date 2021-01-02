using System.Collections.Generic;
using System.Linq;
using System;

public class Player {
    public bool isDealer { get; set; }      
    public int lowValue { get; set; }
    public int highValue { get; set; }
    public int bestValue { get; set; }
    public List<Card> hand = new List<Card>();
    public Card lastDrawnCard { get; set; }
    public int balance { set; get; }

    public void ResetDeck() { hand.Clear(); }

    private void Update() {
        int sumOfCards = 0;
        foreach (Card card in hand) {
            sumOfCards += card.blackJackValue;
        }

        bestValue = sumOfCards;
        lowValue = hand.Min(card => card.blackJackValue); // Instead of creating a sorting algorithm from scratch I used the one built in with C# (not sure about its speed though)
        highValue = hand.Max(card => card.blackJackValue);
    }

    public void DrawCard(Deck gameDeck) {
        Card drawnCard = gameDeck.Draw();
        hand.Add(drawnCard);
        lastDrawnCard = drawnCard;
        Update();
    }

    override public string ToString() {

        Console.WriteLine("\n[Game] The {0}'s hand consist of: ", isDealer ? "Dealer" : "Player");
        foreach (Card card in this.hand)
            Console.WriteLine("- {0}", card.ToString());

        return "test";
    }

}

