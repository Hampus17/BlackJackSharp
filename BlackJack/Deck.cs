using System.Collections.Generic;
using System;
using System.Threading;

public class Deck {
    private int _nrOfDecks, _timesToShuffle;
    private List<Card> _cards;

    public Deck(int nrOfDecks, int timesToShuffle) {
        this._nrOfDecks = nrOfDecks;
        this._timesToShuffle = timesToShuffle;
        this._cards = new List<Card>();

        CreateDeck(_nrOfDecks);
        ShuffleCards(_timesToShuffle);
    }

    // Method to create the decks quite dynamically
    private void CreateDeck(int decksToMake) {
        Console.WriteLine("[Info] Playing deck is {0} cards", decksToMake * 52);
        for (int i = 0; i < decksToMake; i++) {
            int suitNumber = 0;

            // Since their is 52 cards in a deck we need to create that amount accordingly
            for (int cardCount = 0; cardCount < 52; cardCount++) {

                SuitType currentSuit = SuitType.Club;
                switch (suitNumber) {
                    case 1:
                        currentSuit = SuitType.Diamond;
                        break;
                    case 2:
                        currentSuit = SuitType.Heart;
                        break;
                    case 3:
                        currentSuit = SuitType.Spade;
                        break;
                    default:
                        break;
                }

                // Since there is 13 cards in a suit you can simply change the suit every 13 loops
                if (cardCount % 13 == 0 || cardCount == 0) {
                    suitNumber++;
                    for (int cardNumber = 0; cardNumber < 13; cardNumber++)
                        _cards.Add(new Card(cardNumber, currentSuit));
                }
            }
        }
    }

    public void ResetAndShuffle() {
        _cards.Clear();
        CreateDeck(_nrOfDecks);
        ShuffleCards(_timesToShuffle);
    }

    private void ShuffleCards(int timesToShuffle) {

        // Based on this solution https://stackoverflow.com/a/1262619/10966097
        // See solution for specific details, also refer to this wikipedia article https://en.wikipedia.org/wiki/Fisher–Yates_shuffle

        Random rand = new Random();
        
        int n = _cards.Count;

        for (int i = 0; i < timesToShuffle; i++) {
            while (n > 1) {
                n--;
                int k = rand.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }

        // This makes the printing look prettier
        Console.Write("[Info] Shuffling decks");
        Thread.Sleep(1000);
        Console.Write(".");
        Thread.Sleep(1500);
        Console.Write(".");
        Thread.Sleep(800);
        Console.Write(".\n");
        Thread.Sleep(1000);
        Console.WriteLine("[Info] Finished shuffling cards\n");
    }

    public Card Draw() {
        Card drawnCard = _cards[0];
        _cards.RemoveAt(0);
        return drawnCard;
    }

}