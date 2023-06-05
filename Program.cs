using System;
using System.Collections.Generic;
using System.Threading;

namespace Jogo
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            List<Player> players = new List<Player>();
            List<string> roles = new List<string> { "m", "d", "x", "c", "c", "c", "c", "c" };
            string humanInput;
            List<int> judgement = new List<int>();

            Console.WriteLine("Seja bem-vindo ao jogo Cidade Dorme!");
            Console.WriteLine("\nPor favor, digite seu nome para jogar: ");
            //string name = Console.ReadLine();
            string name = Console.ReadLine();
            Console.WriteLine($"\n{name}, você estará jogando com outros 7 jogadores.");
            Console.WriteLine("\nHá um mafioso solto pela cidade!!!\nFelizmente, ainda temos um doutor e um xerife por aí...");

            int round = 1;
            int players_alive = 8;

            Player human = new Player("0", name);

            players.Add(human);

            string y;

            for (int x = 1; x <= 7; x++)
            {
                y = Convert.ToString(x);
                players.Add(new Player(y));
            }

            Random rnd = new Random();

            for (int x = 0; x <= 7; x++)
            {
                int number = rnd.Next(0, roles.Count);
                players[x].SetRole(roles[number]);
                roles.RemoveAt(number);
            }

            Console.WriteLine($"\nAtenção!!! {players[0].Name}, você é um {players[0].Role}!");

            while (players_alive > 3)
            {
                Thread.Sleep(500);
                Console.WriteLine($"\n*** RODADA {round} ***");
                int dead = Kill(players);
                bool doctor_rescued = Rescue(players, dead);
                if (doctor_rescued)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nNa última noite, um cidadão sofreu uma tentativa de assassinato. Felizmente, o doutor não mediu esforços e conseguiu salvá-lo!");
                }
                else if (dead != 0)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"\nNa última noite, um terrível assassinato ocorreu na cidade, e o player {dead} foi morto.");
                    players_alive--;
                }
                else
                {
                    Thread.Sleep(1000);
                    players_alive--;
                    Console.WriteLine($"\nNa última noite, um terrível assassinato ocorreu na cidade, e {players[0].Name} foi morto.");
                    Console.WriteLine($"\nOh não! {players[0].Name} o mafioso te matou!!!");
                    Console.WriteLine("\nA partir daqui você só poderá participar como espectador. Deseja sair do jogo?");
                    humanInput = Console.ReadLine();

                    if (humanInput == "Sim" || humanInput == "S" || humanInput == "sim" || humanInput == "s")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nAtenção!!! Agora, você está jogando como espectador!");
                    }
                }
                   
                if(players[0].Role == "xerife" && players[0].Life == 1)
                {
                    XerifeWorks(players);
                }

                if(players_alive <= 4)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nCuidado e atenção nas decisões! Se o mafioso não for eliminado agora, será o fim do jogo com derrota da cidade.");
                }

                judgement = Trial(players, players_alive);
                players_alive = judgement[0];
                round++;
                if (players_alive == 2 || players_alive == 3)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\n*** THE END ***");
                    Thread.Sleep(500);
                    Console.WriteLine("\nO MAFIOSO VENCEU!!!");
                    Thread.Sleep(500);
                    foreach (Player person in players)
                    {
                        if (person.Role == "mafioso" && person.Id == "0")
                        {
                            Console.WriteLine($"\n{person.Name} era o terrível mafioso.");
                        }

                        else if(person.Role == "mafioso"){
                            Console.WriteLine($"\nO player {person.Id} era o terrível mafioso.");
                        }
                    }

                    }
                    else if(judgement[1] == 0 && players_alive != 0)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("\nA partir daqui você só poderá participar como espectador. Deseja sair do jogo?");
                        humanInput = Console.ReadLine();

                        if (humanInput == "Sim" || humanInput == "S" || humanInput == "sim" || humanInput == "s")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nAtenção!!! Agora, você está jogando como espectador!");
                        }
                    }
               
            }

        }

        public static int Kill(List<Player> lista)
        {
            bool done = false;
            if(lista[0].Role == "mafioso")
            {
                while (!done)
                {
                    Console.WriteLine("\nDigite o número do player que você deseja matar: ");
                    string a = Console.ReadLine();

                    if(Int32.TryParse(a, out int choice) && choice > 0 && choice < 8 && lista[choice].Life == 1)
                    {
                        lista[choice].Die();
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine("\nEscolha inválida.");
                    }

                }
            }
            else
            {
                while (!done)
                {
                    Random rnd = new Random();
                    int choice = rnd.Next(0, lista.Count);
                    if(lista[choice].Life == 1 && lista[choice].Role != "mafioso")
                    {
                        lista[choice].Die();
                        return choice;
                    }
                }
            }
            return 10;
        }

        public static bool Rescue(List<Player> lista, int killed)
        {
            bool done = false;
            bool doctor_alive = false;
            foreach(Player person in lista)
            {
                if (person.Role == "doutor" && person.Life == 1)
                {
                    doctor_alive = true;
                }
            }

            if (!doctor_alive)
            {
                return false;
            }

            if (lista[0].Role == "doutor" && lista[0].Life == 1)
            {

                while (!done)
                {
                    Console.WriteLine("\nDigite o número do player que você deseja tentar salvar: ");
                    string a = Console.ReadLine();

                    bool correct = Int32.TryParse(a, out int choice);
                    if (correct && choice > 0 && choice < 8 && lista[choice].Life == 1 && choice == killed)
                    {
                        return true;
                    }
                    else if (correct && choice > 0 && choice < 8 && lista[choice].Life == 1 && choice != killed)
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("\nEscolha inválida.");
                    }

                }
            }
            else
            {
                while (!done)
                {
                    Random rnd = new Random();
                    int choice = rnd.Next(0, lista.Count);
                    if (lista[choice].Life == 1 && choice == killed)
                    {
                        return true;
                    }
                    else if (lista[choice].Life == 1 && choice != killed)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static void XerifeWorks(List<Player> lista)
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine("\nXerife, digite o número do player que o senhor deseja investigar: ");
                string a = Console.ReadLine();
                bool correct = Int32.TryParse(a, out int choice);
                if(correct && choice > 0 && choice < 8 && lista[choice].Life == 1 && lista[choice].Role == "mafioso")
                {
                    Thread.Sleep(500);
                    Console.WriteLine("\nEle é o mafioso!!! Agora o senhor já sabe!");
                    done = true;
                }
                else if(correct && choice > 0 && choice < 8 && lista[choice].Life == 1 && lista[choice].Role != "mafioso")
                {
                    Thread.Sleep(500);
                    Console.WriteLine("\nEle não é o mafioso. Boa sorte na próxima investigação!");
                    done = true;
                }
                else
                {
                    Console.WriteLine("\nEscolha inválida.");
                }
            }

            
        }

        public static List<int> Trial(List<Player> lista, int amount_alive)
        {
            List<string> players_alive = new List<string>();
            string print_alive = "";
            List<int> after_trial = new List<int>();
            string print_name = "";
            int choice = 10;

            for (int x = 1; x < lista.Count; x++)
            {
                if (lista[x].Life == 1)
                {
                    players_alive.Add(lista[x].Id);
                }
            }

            for (int x = 0; x < players_alive.Count; x++)
            {
                if(x == players_alive.Count - 2)
                {
                    print_alive = print_alive + players_alive[x] + " e ";
                }
                else if (x == players_alive.Count - 1)
                {
                    print_alive = print_alive + players_alive[x] + ".";
                }
                else
                {
                    print_alive = print_alive + players_alive[x] + ", ";
                }
            }

            bool result = false;
            int already_charged = 2000;
            int player_charged = 0;
            Random rnd = new Random();

            if (lista[0].Life == 0)
            {
                Console.WriteLine($"\nRestam vivos estes players: {print_alive}");
                while (!result)
                {
                    bool charge = false;
                    while (!charge)
                    {

                        player_charged = rnd.Next(1, 8);
                        if(lista[player_charged].Life == 1 && player_charged != already_charged)
                        {
                            charge = true;
                            //string str_player_charged = Convert.ToString(player_charged);
                            Console.WriteLine($"\nOs jogadores decidiram acusar o player {player_charged}.\n\nHora da votação!");
                        }

                    }

                    int decision = rnd.Next(0, 3);

                    if (decision > 0 && lista[player_charged].Role != "mafioso")
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("\nVotação concluída! O player acusado foi eliminado. \n\nInfelizmente, ele não era o mafioso...");
                        amount_alive = amount_alive - 1;
                        result = true;
                    }
                    else if(decision > 0 && lista[player_charged].Role == "mafioso")
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("\nVotação concluída! O player acusado foi eliminado. \n\nEle era o mafioso!\n\n*** THE END ***\n\nA CIDADE VENCEU!!!");
                        result = true;
                        amount_alive = 0;
                    }
                    else
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("\nVotação concluída! O player acusado fez uma excelente defesa e não foi eliminado. Será necessário fazer outra acusação.");
                        already_charged = player_charged;
                    }
                }

                after_trial.Add(amount_alive);
                after_trial.Add(player_charged);

                return after_trial;
            }

            else
            {

                List<string> excuses = new List<string> {"Sou inocente!!! Por favor, não me eliminem!", "Enquanto a cidade dormia, eu estava passeando no shopping. Não cometi crime algum.", "Não fiz nada! Votem em outro!", "Eu estava dormindo na última noite.", "Virei a noite trabalhando, sou inocente."};

                Console.WriteLine($"\nRestam vivos estes players: {lista[0].Name}, {print_alive}");
                while (!result)
                {
                    bool charge = false;
                    bool human_correct = false;
                    int human_choice = 10;


                    while (!human_correct)
                    {
                        Console.WriteLine("\nDigite o número do player que você quer acusar:");
                        string a = Console.ReadLine();
                        human_correct = Int32.TryParse(a, out choice);
                        if(human_correct && (choice <= 0 || choice > 7))
                        {
                            human_correct = false;
                            Console.WriteLine("\nNúmero inválido.");

                        }
                        else if(human_correct && lista[choice].Life == 0)
                        {
                            human_correct = false;
                            Console.WriteLine("\nEsse player já está morto. Escolha outro para acusar.");
                        }
                        else if (!human_correct)
                        {

                            Console.WriteLine("\nEscolha inválida.");
                        }
                        else
                        {
                            Console.WriteLine("\nOs players vivos estão indicando quem eles querem acusar...");
                        }
                    }
                    human_choice = choice;
                    int decision = rnd.Next(0, 3);
                    if (decision > 0 && choice != already_charged)
                    {
                        Thread.Sleep(1500);
                        Console.WriteLine($"\nOutros jogadores concordaram com você, e o player {choice} está sendo acusado!");
                    }
                    else
                    {
                        Thread.Sleep(1500);
                        Console.WriteLine("\nOs outros jogadores não concordaram com você e acusarão outro player.");
                        while (!charge)
                        {
                            choice = rnd.Next(0, 8);
                            if (lista[choice].Life == 1 && choice != human_choice && choice != already_charged)
                            {
                                charge = true;
                                if (choice == 0)
                                {
                                    Thread.Sleep(1500);
                                    Console.WriteLine("\nOs outros jogadores decidiram acusar você!!!");
                                }
                                else
                                {
                                    //Talvez tenha que converter para string
                                    Thread.Sleep(1500);
                                    Console.WriteLine($"\nOs outros jogadores decidiram acusar o player {choice}.");
                                }

                            }
                        }
                    }
                        //Add above
                        if(choice == 0)
                        {
                            Thread.Sleep(500);
                            print_name = lista[0].Name;
                            Console.WriteLine("\nÉ hora da defesa. Tente demonstrar sua inocência aos outros players para que eles não te eliminem. Digite o que precisar:\n");
                            string input = Console.ReadLine();

                        }
                        else
                        {
                            Thread.Sleep(500);
                            print_name = $"O player {choice}";
                            Console.WriteLine("\nÉ hora de ouvir a defesa. Confira a declaração do player acusado para tomar sua decisão de voto:");
                            Thread.Sleep(1500);
                            int index = rnd.Next(0, excuses.Count);
                            Console.WriteLine($"\n{excuses[index]}");
                            Thread.Sleep(500);
                            Console.WriteLine("\nChegou o momento de votar. Você deseja eliminar o player acusado? Digite sua decisão:");
                            Console.ReadLine();
                            Console.WriteLine("\nOk! Os players vivos estão votando também...");
                        }

                   
                    int vote = rnd.Next(0, 2);
                    if(vote > 0 && lista[choice].Role != "mafioso")
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine($"\nVotação concluída! {print_name} foi eliminado.");
                        Thread.Sleep(1000);
                        Console.WriteLine("\nInfelizmente, ele não era o mafioso...");
                        result = true;
                        amount_alive = amount_alive - 1;
                        lista[choice].Die();
                    }
                    else if(vote > 0 && lista[choice].Role == "mafioso")
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine($"\nVotação concluída! {print_name} foi eliminado.");
                        Thread.Sleep(1000);
                        Console.WriteLine("\nEle era o mafioso!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\n*** THE END ***");
                        Thread.Sleep(500);
                        Console.WriteLine("\nA CIDADE VENCEU!!!");
                        result = true;
                        amount_alive = 0;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine($"\nVotação concluída! {print_name} fez uma excelente defesa e não foi eliminado. Será necessário fazer outra acusação.");
                        already_charged = choice;
                    }
                }
            }

            List<int> retornar = new List<int> {amount_alive, choice};
            return retornar;
        }

    }
}
