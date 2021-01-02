using System;
using System.Collections.Generic;
using System.Threading;

public class Game {
    private int _dealerWins = 0, _playerWins = 0;
    private int _gameWager = 250;

    public int NUMBER_OF_DECKS = 4, SHUFFLE_TIMES = 3;

    public Deck gameDeck;
    public Player dealer;
    public Player player;
    public GameStatus gameState { get; set; }


    public Game() {
        dealer = new Player { isDealer = true };
        player = new Player { balance = 5000 };
        gameState = GameStatus.InMenu;
    }

    public void Run() {
        // Application loop
        while (gameState != GameStatus.AppExited) {

            // Main Menu
            if (gameState == GameStatus.InMenu) {
                player.ResetDeck();
                dealer.ResetDeck();
                PrintMenu();
            }

            // Reset everything and set state to InGame
            if (gameState == GameStatus.Restarting) {
                player.ResetDeck();
                dealer.ResetDeck();
                gameDeck.ResetAndShuffle();
                gameState = GameStatus.InGame;
            }

            // Game Loop
            if (gameState == GameStatus.InGame) {
                if (gameDeck == default(Deck)) {
                    Console.WriteLine("\n[Info] Preparing game...\n");
                    gameDeck = new Deck(NUMBER_OF_DECKS, SHUFFLE_TIMES);
                }

                // Print the game stuff
                StartGame();

                // Check who won and log it
                Console.WriteLine("\n########################################\n");
                if (gameState == GameStatus.PlayerWin) {
                    Console.WriteLine("[Game] Player won!");
                } else if (gameState == GameStatus.HouseWin) {
                    Console.WriteLine("[Game] House won!");
                }
                else if (gameState == GameStatus.GameDraw) {
                    Console.WriteLine("[Game] Draw!");
                }
                else {
                    gameState = GameStatus.InMenu;
                }

                PrintStats();
                Thread.Sleep(4000);

                Console.WriteLine("[Game] Do you want to exit or go the main menu?");
                Console.WriteLine("[1] Main Menu");
                Console.WriteLine("[2] Restart");
                Console.WriteLine("[3] Exit");
                Console.Write("\n>>> ");

                // Another check of input so nothing which isn't checked for can be typed in
                bool noOption = true;
                while (noOption) {
                    noOption = false;

                    string strOption = Console.ReadLine();
                    int pOption = ParseOptionToInt(strOption);

                    switch (pOption) {
                        case 1:
                            gameState = GameStatus.InMenu;
                            break;
                        case 2:
                            gameState = GameStatus.Restarting;
                            break;
                        case 3:
                            gameState = GameStatus.AppExited;
                            break;
                        default:
                            noOption = true;
                            break;
                    }
                }
                Console.WriteLine("");

                Thread.Sleep(2000);
            }

        }

        Console.WriteLine("\n[Info] Exiting application...");
        Thread.Sleep(4000);
    }

    private void Reset() {
        gameState = GameStatus.InMenu;
        player.ResetDeck();
    }

    public void PlayerDraw() {
        player.DrawCard(gameDeck);
    }

    public void DealerDraw() {
        dealer.DrawCard(gameDeck);
    }

    private void PlayerWin() {
        _playerWins++;
        player.balance += _gameWager;
        gameState = GameStatus.PlayerWin;
    }

    private void HouseWin() {
        _dealerWins++;
        player.balance -= _gameWager;
        gameState = GameStatus.HouseWin;
    }

    private int ParseOptionToInt(string option) {
        bool validOpt = false;
        int parsedOpt = 0;


        while (!validOpt) {
            try {
                parsedOpt = int.Parse(option);
                validOpt = true;

                return parsedOpt;
            }
            catch (Exception e) {
                Console.WriteLine("\n[Error] Invalid input, try again.\n");
                Console.Write("[Option]: ");
                option = Console.ReadLine();
            }
        }

        return parsedOpt;
    }

    private void PrintStats() {
        Console.WriteLine("\n[Stats]");
        Console.WriteLine("Balance: {0}", player.balance);
        Console.WriteLine("Player Wins: {0}", _playerWins);
        Console.WriteLine("House Wins: {0}\n", _dealerWins);
    }

    // This methods can probably been optimized a lot and remove redundancy
    private void StartGame() {
        Console.Clear();
        Thread.Sleep(1500);
        Console.Clear();
        Thread.Sleep(80);
        Console.Write("W");
        Thread.Sleep(80);
        Console.Write("E");
        Thread.Sleep(80);
        Console.Write("L");
        Thread.Sleep(80);
        Console.Write("C");
        Thread.Sleep(80);
        Console.Write("O");
        Thread.Sleep(80);
        Console.Write("M");
        Thread.Sleep(80);
        Console.Write("E");

        Thread.Sleep(130);
        Console.Write(" T");
        Thread.Sleep(80);
        Console.Write("O ");

        Thread.Sleep(170);
        Console.Write("B");
        Thread.Sleep(80);
        Console.Write("L");
        Thread.Sleep(80);
        Console.Write("A");
        Thread.Sleep(80);
        Console.Write("C");
        Thread.Sleep(80);
        Console.Write("K");
        Thread.Sleep(80);
        Console.Write("J");
        Thread.Sleep(80);
        Console.Write("A");
        Thread.Sleep(80);
        Console.Write("C");
        Thread.Sleep(80);
        Console.Write("K\n\n");

        Thread.Sleep(700);
        dealer.DrawCard(gameDeck);
        Console.WriteLine("[Dealer] Drew hidden card");

        Thread.Sleep(1000);
        player.DrawCard(gameDeck);
        Console.WriteLine("[Player] Drew {0}", player.hand[0].ToString());

        Thread.Sleep(2000);
        dealer.DrawCard(gameDeck);
        Console.WriteLine("[Dealer] Drew {0}", dealer.hand[1].ToString());

        Thread.Sleep(1500);
        player.DrawCard(gameDeck);
        Console.WriteLine("[Player] Drew {0}", player.hand[1].ToString());

        Thread.Sleep(1000);
        Console.WriteLine("\n[Game] The Dealer's hand consist of: " +
            "\n - Hidden" +
            "\n - {0}", dealer.hand[1].ToString());

        player.ToString();

        bool playerStay = false;

        // Loop for player choices (exits when players wants to stay)
        while (gameState == GameStatus.InGame && !playerStay) {

            // Before getting the input whether the player wants to draw or stay, check if loss
            if (player.bestValue == 21) {
                PlayerWin();
                break;
            }
            else if (dealer.bestValue == 21) {
                HouseWin();
                break;
            } else if (player.bestValue > 21) {
                HouseWin();
                break;
            } else if (dealer.bestValue > 21) {
                PlayerWin();
                break;
            }

            Console.WriteLine("\n[Game] Do you want to draw (D) or stay (S)?");
            Console.Write(">>> ");
            string playerChoice = Console.ReadLine();

            Console.WriteLine("");
            // Used StringComparion to check input regardless of case
            while (!string.Equals(playerChoice, "s", StringComparison.OrdinalIgnoreCase) && !string.Equals(playerChoice, "d", StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine("[Warning] Invalid choice, choose either Stay (S) or Draw (D)");
                Console.Write(">>> ");
                playerChoice = Console.ReadLine();

                if (string.Equals(playerChoice, "s", StringComparison.OrdinalIgnoreCase) || string.Equals(playerChoice, "d", StringComparison.OrdinalIgnoreCase))
                    break;
            }
            

            if (playerChoice.Equals("s", StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine("[Player] Stay ");

                Thread.Sleep(1000);
                Console.WriteLine("\n[Game] The Player's hand consist of: ");
                player.ToString();

                Console.WriteLine("\n[Game] Revealing the Dealer's cards ");
                Thread.Sleep(1000);
                dealer.ToString();

                break;
            }

            if (playerChoice.Equals("d", StringComparison.OrdinalIgnoreCase)) {
                player.DrawCard(gameDeck);
                Console.WriteLine("[Player] Drew {0}", player.hand[player.hand.Count - 1].ToString());

                Thread.Sleep(1000);
                player.ToString();

                Console.WriteLine("\n[Game] The Dealer's hand consist of: " +
                    "\n - Hidden" +
                    "\n - {0}", dealer.hand[1].ToString());
            }
        }

        // Simple checks to determine if dealer or player won (or draw)
        if (player.bestValue <= 21 && dealer.bestValue <= 21) {
            if (player.bestValue == dealer.bestValue)
                gameState = GameStatus.GameDraw;
            else if (player.bestValue > dealer.bestValue)
                PlayerWin();
            else if (player.bestValue < dealer.bestValue)
                HouseWin();
        }
        else if (player.bestValue > 21 && dealer.bestValue <= 21)
            HouseWin();
        else if (dealer.bestValue > 21 && player.bestValue <= 21)
            PlayerWin();
        else if (player.bestValue > 21 && dealer.bestValue > 21)
            gameState = GameStatus.GameDraw;

        Thread.Sleep(2000);

    }

    private void PrintMenu() {
        Console.Clear();

        // Print things about game and current stats

        Console.WriteLine(" ____  _            _           _            _ ");
        Thread.Sleep(200);
        Console.WriteLine("|  _ \\| |          | |         | |          | |  ");
        Thread.Sleep(200);
        Console.WriteLine("| |_) | | __ _  ___| | __      | | __ _  ___| | __");
        Thread.Sleep(200);
        Console.WriteLine("|  _ <| |/ _` |/ __| |/ /  _   | |/ _` |/ __| |/ /");
        Thread.Sleep(200);
        Console.WriteLine("| |_) | | (_| | (__|   <  | |__| | (_| | (__|   < ");
        Thread.Sleep(200);
        Console.WriteLine("|____/|_|\\__,_|\\___|_|\\_\\  \\____/ \\__,_|\\___|_|\\_\\\n");
        Thread.Sleep(400);

        Console.WriteLine("by Hampus\n\n");

        Thread.Sleep(800);
        Console.WriteLine("Current balance: ${0}", player.balance);
        Console.WriteLine("Current bet: ${0}\n", _gameWager);
        Console.WriteLine("[1] Start a New Game");
        Console.WriteLine("[2] Change Balance");
        Console.WriteLine("[3] Change Bet Amount");
        Console.WriteLine("[4] Exit");
        Console.Write("\n>>> ");

        // Check the input so it valid, mostly prevent errors but also make the option system much smoother
        bool noOption = true;
        while (noOption) {
            noOption = false;

            string strOption = Console.ReadLine();
            int pOption = ParseOptionToInt(strOption);

            switch (pOption) {
                case 1:
                    gameState = GameStatus.InGame;
                    break;
                case 2:
                    Console.WriteLine("\nEnter new balance (Current: ${0}): ", player.balance);
                    Console.Write(">>> $");
                    strOption = Console.ReadLine();
                    pOption = ParseOptionToInt(strOption);
                    player.balance = pOption;
                    break;
                case 3:
                    Console.WriteLine("\nEnter new bet amount (Current: ${0}): ", _gameWager);
                    Console.Write(">>> $");
                    strOption = Console.ReadLine();
                    pOption = ParseOptionToInt(strOption);
                    _gameWager = pOption;
                    break;
                case 4:
                    gameState = GameStatus.AppExited;
                    break;
                default:
                    noOption = true;
                    break;
            }
        }
    }
}
