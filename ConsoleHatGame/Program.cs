using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleHatGame
{
    class Program
    {
        public static List<Team> listOfTeam = new List<Team>();
        public static List<Word> listOfWord = new List<Word>();
        public static int randomWordResult;
        public static string activeWord;

        public static int activeTeamInt = 0;
        public static string activeTeam;

        //таймер
        public static Timer aTimer = new Timer();
        public static bool isTimerInit = false;
        public static int secundes = 30;

        static void Main(string[] args)
        {
            /**
             * Объявление переменных
             */

            string teamCountS;
            string wordCountS;
            int teamCount;
            int wordCount;

            Console.WriteLine("Добро пожаловать в игру Шляпа");


            //получение количеств команд
            Console.WriteLine("Сколько команд будет играть в игру?");
            teamCountS = Console.ReadLine();

            while(!(int.TryParse(teamCountS, out teamCount)) && teamCount < 2 )
            {
                Console.WriteLine("Вы ввели не число или число меньше 2");
                teamCountS = Console.ReadLine();
            }

            //получение количества слов

            Console.WriteLine("Сколько будет слов?");
            wordCountS = Console.ReadLine();

            while (!(int.TryParse(wordCountS, out wordCount)) && wordCount < 2)
            {
                Console.WriteLine("Вы ввели не число или число меньше 2");
                wordCountS = Console.ReadLine();
            }

            //получение команд
            for(int i = 1; i<= teamCount; i++)
            {
                Console.WriteLine("Введите название {0} команды", i);
                string teamName = Console.ReadLine();
                listOfTeam.Add(new Team() { Id = i, Name = teamName, Score = 0});
            }

            //получение слов
            for (int i = 1; i <= wordCount; i++)
            {
                Console.WriteLine("Введите {0} слово", i);
                string wordName = Console.ReadLine();
                listOfWord.Add(new Word() { WordName = wordName });
            }

            Console.WriteLine("Чтобы начать игру, нажмите ПРОБЕЛ");

            ConsoleKeyInfo keyInput = Console.ReadKey();
            while (!(keyInput.Key == ConsoleKey.Spacebar))
            {
                keyInput = Console.ReadKey();
            }

            Console.Clear();

            GameProcess();

            Console.ReadLine();
        }

        private static void GameProcess()
        {
            Random rnd = new Random();
            randomWordResult = rnd.Next(0, listOfWord.Count);

            if (listOfWord.Count > 0 && listOfTeam.Count > activeTeamInt)
            {
                activeWord = listOfWord[randomWordResult].WordName;
                activeTeam = listOfTeam[activeTeamInt].Name;
                listOfWord.RemoveAt(randomWordResult);

                Console.WriteLine("Играет {0} команда", activeTeam);
                Console.WriteLine("Ведущий, тебе нужно объяснить слово {0}, запускаем таймер", activeWord);
                if (isTimerInit == false)
                {
                    TimerInit();
                }
                else
                {
                    aTimer.Start();
                }

                Console.WriteLine("Если игрок угадает слово, нажмите пробел");
                ConsoleKeyInfo keyInput = Console.ReadKey();
                while (!(keyInput.Key == ConsoleKey.Spacebar))
                {
                    keyInput = Console.ReadKey();
                }

                Console.WriteLine("Мы рады что вы угадали слово!");
                listOfTeam[activeTeamInt].Score++;
                aTimer.Stop();

                GameProcess();
            }
            else
            {
                GameOver();
            }
        }

        private static void GameOver()
        {
            Console.WriteLine("Игра завершена!");
            int max = listOfTeam.Max(team => team.Score);

            var result = listOfTeam.FirstOrDefault(team => team.Score == max);

            Console.WriteLine("Победила команда {0}, набравшая {1} очков", result.Name, result.Score);

            Console.WriteLine();
            Console.WriteLine("Остальные участники и их набранные баллы: ");

            foreach(var team in listOfTeam)
            {
                Console.WriteLine("КОманда {0} набрала {1} очков", team.Name, team.Score);
            }


        }

        private static void TimerInit()
        {
            aTimer.Elapsed += TimerFunc;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Interval = 1000;
            isTimerInit = true;
        }

        private static void TimerFunc(Object state, ElapsedEventArgs e)
        {
            Console.WriteLine(secundes);

            if(secundes > 0)
            {
                secundes--;
            } else
            {
                aTimer.Stop();

                secundes = 30;
                activeTeamInt++;

                listOfWord.Add(new Word() { WordName = activeWord });

                GameProcess();
            }
        }
    }
}
