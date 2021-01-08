using System;

public class Card {
    public int cardValue { get; }
    public int blackJackValue { get; set; }
    public SuitType suit { get; }

    public Card(int value, SuitType Suit) {

        if (value == 0) blackJackValue = 1;
        else if (value >= 11 && cardValue <= 13) blackJackValue = 10;
        else blackJackValue = value;

        this.suit = Suit;
        this.cardValue = value;
    }

    override public string ToString() {
        string currentSuit = "";

        switch (suit) {
            case SuitType.Diamond:
                currentSuit = "Diamond";
                break;
            case SuitType.Club:
                currentSuit = "Club";
                break;
            case SuitType.Heart:
                currentSuit = "Heart";
                break;
            case SuitType.Spade:
                currentSuit = "Spade";
                break;
            default:
                break;
        }

        // Since the cards are created using integers for the values (1-13) we need to determine which are the "joker" cards
        if (cardValue == 0 || cardValue >= 11) {
            string jokerCard = "";
            if (cardValue == 0) jokerCard = "Ace";
            if (cardValue == 11) jokerCard = "Jack";
            if (cardValue == 12) jokerCard = "Queen";
            if (cardValue == 13) jokerCard = "King";

            return String.Format("{0} of {1}s (game value {2})", jokerCard, suit, blackJackValue); 
        }
        else { return String.Format("{0} of {1}s (game value {2})", cardValue, suit, blackJackValue); }
    }
}

