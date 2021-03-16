using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon
{

    // TODO: Make this Class a Singleton
    class Game
    {
        List<Pokemon> roster = new List<Pokemon>();
        bool isCurrentlyFighting = false;
        Pokemon player = null;
        Pokemon enemy = null;

        public void StartGame()
        {
            Initialize();
            WelcomePlayer();
            RunGameLoop();
        }

        private void RunGameLoop()
        {
            
            while (true)
            {
                PlayerTurn();
            }
        }

        private void PlayerTurn()
        {
            AskForCommands();
            switch (Console.ReadLine())
            {
                case "list":
                    ListPokemon();
                    break;
                case "fight":
                    Fight();
                    break;
                case "heal":
                    HealPokemon();
                    break;
                case "quit":
                    Environment.Exit(0);
                    break;
                default:
                    UnknownCommand();
                    break;
            }
        }

        private void Fight()
        {
            if (isCurrentlyFighting)
            {
                Fighting();
            }
            else
            {
                PrepareFight();
            }
        }

        private void PrepareFight()
        {
            //PRINT INSTRUCTIONS AND POSSIBLE POKEMONS (SEE SLIDES FOR EXAMPLE OF EXECUTION)
            ListPokemon();
            
            Console.WriteLine("Write 2 Pokemon names. The first will be your Pokemon and the second will be your opponent");
            Console.WriteLine("The Pokemon names need to be Case sensitive and separated by a space");
            Console.Write("Choose who should fight?  ");

            //READ INPUT, REMEMBER IT SHOULD BE TWO POKEMON NAMES
            string input = Console.ReadLine();
            //BE SURE TO CHECK THE POKEMON NAMES THE USER WROTE ARE VALID (IN THE ROSTER) AND IF THEY ARE IN FACT 2!

            string[] inputStrings = input.Split(' ');

            Console.WriteLine();

            if (inputStrings.Length == 2)
            {
                for (int i = 0; i < roster.Count; i++) // Goes through the roster
                {
                    if (inputStrings[0] == roster[i].Name)
                    {
                        Console.WriteLine(inputStrings[0] + " found in the roster.");
                        Console.WriteLine();
                        player = roster[i];
                    }

                    if (inputStrings[1] == roster[i].Name)
                    {
                        Console.WriteLine(inputStrings[1] + " found in the roster.");
                        Console.WriteLine();
                        enemy = roster[i];
                    }
                }
                
            }

            if (player == null || enemy == null)
            {
                Console.WriteLine("Your chosen Pokemon Names cannot be found in the roster.");
                Console.WriteLine();
                return;
            }

            if (player != null && player.Hp > 0 && enemy != null && enemy.Hp > 0)
            {
                isCurrentlyFighting = true;
                Fighting();
            }
        }

        private int ValidateMove()
        {
            string input = Console.ReadLine();
            if (Int32.TryParse(input, out int MoveChoosen))
            {
                if (MoveChoosen > 0 && MoveChoosen < player.Moves.Count)
                {
                    return MoveChoosen;
                }
            }
            else
            {
                Console.WriteLine("Move not recognized.");
                Console.WriteLine("Please write the number of move you want to use.");
                ValidateMove();
            }
            return 0;
        }

        private void Fighting()
        {
            //if everything is fine and we have 2 pokemons let's make them fight
            if (player != null && enemy != null && player != enemy)
            {
                Console.WriteLine("A wild " + enemy.Name + " appears!");
                Console.WriteLine(player.Name + " I choose you! ");
                Console.WriteLine();

                //BEGIN FIGHT LOOP
                while (player.Hp > 0 && enemy.Hp > 0)
                {
                    //PRINT POSSIBLE MOVES
                    Console.WriteLine("Your Pokemon knows the following moves");
                    for (int i = 0; i < player.Moves.Count; i++)
                    {
                        Console.WriteLine(i + ". " + player.Moves[i].Name);
                    }
                    Console.Write("What move should we use? Write the number of the move.");
                    Console.WriteLine();

                    //GET USER ANSWER, BE SURE TO CHECK IF IT'S A VALID MOVE, OTHERWISE ASK AGAIN

                    int move = ValidateMove();


                    //CALCULATE AND APPLY DAMAGE
                    int damage = player.Attack(enemy);

                    //print the move and damage
                    Console.WriteLine(player.Name + " uses " + player.Moves[move].Name + ". " + enemy.Name + " loses " + damage + " HP");
                    Console.WriteLine("Your " + player.Name + " has now " + player.Hp + " hp left.");
                    Console.WriteLine("The hostile  " + enemy.Name + " has now " + enemy.Hp + " hp left.");
                    Console.WriteLine();

                    //if the enemy is not dead yet, it attacks
                    if (enemy.Hp > 0)
                    {
                        //CHOOSE A RANDOM MOVE BETWEEN THE ENEMY MOVES AND USE IT TO ATTACK THE PLAYER
                        Random rand = new Random();
                        /*the C# random is a bit different than the Unity random
                         * you can ask for a number between [0,X) (X not included) by writing
                         * rand.Next(X) 
                         * where X is a number 
                         */
                        int enemyMove = rand.Next(enemy.Moves.Count);
                        int enemyDamage = enemy.Attack(player);

                        //print the move and damage
                        Console.WriteLine(enemy.Name + " uses " + enemy.Moves[enemyMove].Name + ". " + player.Name + " loses " + enemyDamage + " HP");
                        Console.WriteLine("Your " + player.Name + " has now " + player.Hp + " hp left.");
                        Console.WriteLine("The hostile  " + enemy.Name + " has now " + enemy.Hp + " hp left.");
                        Console.WriteLine();
                    }
                }
                FinishFight();
            }
        }

        private void FinishFight()
        {
            isCurrentlyFighting = false;
            //The loop is over, so either we won or lost
            if (enemy.Hp <= 0)
            {
                Console.WriteLine(enemy.Name + " faints, you won!");
            }
            else
            {
                Console.WriteLine(player.Name + " faints, you lost...");
            }

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("");
            }
            PlayerTurn();
        }
            private int ChoosePokemon(string input)
        {
            for (var i = 0; i < roster.Count; i++)
            {
                if (roster[i].Name == input && roster[i].Hp > 0)
                {
                    return i;
                }
            }
            Console.WriteLine("The pokemon name is unknown or dead.");
            Console.WriteLine("Please choose a pokemon that is ready to fight!");
            ListPokemon();
            ChoosePokemon(Console.ReadLine());
            return 0;
        }

        private void ListPokemon()
        {
            List<Pokemon> alivePokemon = new List<Pokemon>();
            List<Pokemon> deadPokemon = new List<Pokemon>();

            foreach (var pokemon in roster)
            {
                if (pokemon.Hp > 0)
                {
                    alivePokemon.Add(pokemon);
                }
                else
                {
                    deadPokemon.Add(pokemon);
                }
            }

            if (alivePokemon.Count > 0)
            {
                Console.WriteLine("The following Pokemon are ready to fight: ");
                foreach (Pokemon pokemon in alivePokemon)
                {
                    Console.WriteLine(" - " + pokemon.Name + " with " + pokemon.Hp + " hp." );
                }
                Console.WriteLine("");
            }

            if (deadPokemon.Count > 0)
            {
                Console.WriteLine("The following Pokemon need to be healed first: ");
                foreach (Pokemon pokemon in deadPokemon)
                {
                    Console.WriteLine(" - " + pokemon.Name);
                }
                Console.WriteLine("");
            }
        }

        private void AskForCommands()
        {
            Console.WriteLine("The available commands are list / fight / heal / quit.");
            Console.WriteLine("Please enter a command");
            Console.WriteLine("");
        }

        private void UnknownCommand()
        {
            Console.WriteLine("Unknown command");
            Console.WriteLine("Please write the commands full out and in lowercase.");
            AskForCommands();
        }

        private void WelcomePlayer()
        {
            Console.WriteLine("Welcome to the world of Pokemon!\n");
            Console.WriteLine("Please be aware that the damage calculation is not balanced and might end up in a infinite loop");
            Console.WriteLine("");
        }

        private void HealPokemon(Pokemon pokemon)
        {
            pokemon.Restore();

            Console.WriteLine(pokemon.Name + " has been fully healed.");
            Console.WriteLine();
        }

        private void HealPokemon()
        {
            foreach (var pokemon in roster)
            {
                pokemon.Restore();
            }
            Console.WriteLine("All pokemon have been fully healed.");
            Console.WriteLine();
        }

        public void Initialize()
        {
            PopulateRosterWithPredefinedPokemon();
        }

        private void PopulateRosterWithPredefinedPokemon()
        {
            roster.Add(CreatePredefinedCharmander());
            roster.Add(CreatePredefinedSquirtle());
            roster.Add(CreatePredefinedBulbasaur());
        }

        private Pokemon CreatePredefinedCharmander()
        {
            //Charmander
            string name = "Charmander";
            int level = 3;
            int baseAttack = 52;
            int baseDefence = 42;
            //int hp;
            int maxHp = 39;
            Elements element = Elements.Fire;

            List<Move> moves = new List<Move>();
            moves.Add(new Move("Ember"));
            moves.Add(new Move("Fire Blast"));

            return new Pokemon(name, level, baseAttack, baseDefence, maxHp, element, moves);
        }

        private Pokemon CreatePredefinedSquirtle()
        {
            //Squirtle
            string name = "Squirtle";
            int level = 2;
            int baseAttack = 48;
            int baseDefence = 65;
            //int hp;
            int maxHp = 44;
            Elements element = Elements.Water;

            List<Move> moves = new List<Move>();
            moves.Add(new Move("Bite"));
            moves.Add(new Move("Bubble"));

            return new Pokemon(name, level, baseAttack, baseDefence, maxHp, element, moves);
        }
        
        private Pokemon CreatePredefinedBulbasaur()
        {
            //Bulbasaur
            string name = "Bulbasaur";
            int level = 3;
            int baseAttack = 49;
            int baseDefence = 49;
            //int hp;
            int maxHp = 45;
            Elements element = Elements.Grass;

            List<Move> moves = new List<Move>();
            moves.Add(new Move("Cut"));
            moves.Add(new Move("Mega Drain"));
            moves.Add(new Move("Razor Leaf"));

            return new Pokemon(name, level, baseAttack, baseDefence, maxHp, element, moves);
        }

       

    }
}
