using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

public class Player {
    public int lowValue { get; set; }
    public int highValue { get; set; }
    public int bestValue { get; set; }
    public List<Card> hand = new List<Card>();
    public Card lastDrawnCard { get; set; }
    public int balance { set; get; }

    public bool isDealer { get; set; }
    private bool aceIsPresent;

    public void ResetDeck() { hand.Clear(); }

    private void Update() {
        int sumOfCards = 0;
        List<int> indexOfAces = new List<int>();
        List<Card> tempHand = hand;
        highValue = 0;

        if (isDealer) {
            for (int i = 0; i < tempHand.Count; i++)
                for (int j = 0; j < indexOfAces.Count; j++)
                    if (i.Equals(indexOfAces[j]))
                        tempHand[i].blackJackValue = 11;
        } else if (!isDealer) {
            foreach (Card card in tempHand)
                if (card.cardValue.Equals(0)) {
                    aceIsPresent = true;
                    indexOfAces.Add(hand.IndexOf(card));
                }

            if (aceIsPresent) {
                lowValue = tempHand.Sum(card => card.blackJackValue);

                for (int i = 0; i < hand.Count; i++) 
                    for (int j = 0; j < indexOfAces.Count; j++)
                        if (i.Equals(indexOfAces[j]))
                            tempHand[i].blackJackValue = 11;

                highValue = tempHand.Sum(card => card.blackJackValue);
                Console.WriteLine("[Log] {0}", highValue);
            } else {
                lowValue = tempHand.Sum(card => card.blackJackValue);
            }
        }

        bestValue = tempHand.Sum(card => card.blackJackValue);
        int bestValueAlt = tempHand.Sum(card => card.blackJackValue);

        bestValue = (bestValue - 21) < (bestValueAlt - 21) ? bestValueAlt : bestValue;

    }

    public void DrawCard(Deck gameDeck) {
        Card drawnCard = gameDeck.Draw();
        hand.Add(drawnCard);
        lastDrawnCard = drawnCard;
        Update();
    }

    override public string ToString() {

        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("[Game] The {0}'s hand consist of: ", isDealer ? "Dealer" : "Player");

        Console.WriteLine();
        foreach (Card card in this.hand)
            sb.AppendFormat("\n- {0}", card.ToString());

        sb.AppendFormat("\n\n- Best Score = {0}", bestValue);
        sb.AppendFormat("\n- Lowest Score = {0}", lowValue);

        if (highValue > 0)
            sb.AppendFormat("\n- Highest Score = {0}", highValue);

        return sb.ToString();
    }

}

