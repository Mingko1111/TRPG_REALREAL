using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TRPG_REALREAL
{
    public class GameManager
    {

        private static GameManager? instance;
        public static GameManager Instance => instance ??= new GameManager();

        private Scene currentScene;
        public Player Player { get; private set; }

        public void StartGame()
        {
            Console.WriteLine("게임을 시작합니다!");
            Player = new Player(1, 1);
            ChangeScene(new TownScene1());
        }

        public void ChangeScene(Scene scene)
        {
            currentScene = scene;
            currentScene.Enter(Player, this);

        }

        public void ExitGame()
        {
            Console.WriteLine("게임을 종료합니다.");
            Environment.Exit(0);

        }
    }
}
