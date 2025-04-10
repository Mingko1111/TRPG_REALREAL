using System.Numerics;
using TRPG_REALREAL;
namespace TRPGConsoleGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            ShowTitleMenu();

            Player player = new Player(0, 0);
            Map map = new Map();
        }

        static void ShowTitleMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("  ##################################");
                Console.WriteLine(" ###오리진 네오다크세이버 온라인###");
                Console.WriteLine("##################################");
                Console.WriteLine();
                Console.WriteLine("1. 시작하기");
                Console.WriteLine("2. 종료하기");
                Console.Write("선택: ");
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D1 || input == ConsoleKey.NumPad1)
                {
                    GameManager.Instance.StartGame();
                    break;
                }
                else if (input == ConsoleKey.D2 || input == ConsoleKey.NumPad2)
                {
                    Console.WriteLine("\n게임을 종료합니다. 감사합니다!");
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    Console.WriteLine("\n잘못된 입력입니다. 다시 선택해주세요.");
                    Thread.Sleep(1000);
                }
            }
        }
    }

}