using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TRPG_REALREAL
{
    public class FieldScene : Scene
    {
        private Map map;
        private string fromTown;
        private int? startX;
        private int? startY;
        List<Monster> monsterPool = new List<Monster>
        {
            new Monster("고블린", 30, 5, 50, 15),
            new Monster("슬라임", 20, 3, 30, 10),
            new Monster("오크", 60, 10, 80, 30),
            new Monster("늑대", 40, 8, 60, 25),
            new Monster("박쥐", 25, 4, 40, 12)
        };

        public FieldScene(string fromTown = "Town1", int? startX = null, int? startY = null)
        {
            this.fromTown = fromTown;
            this.startX = startX;
            this.startY = startY;
            map = new Map(true, fromTown);
        }

        public override void Load()
        {
            Console.Clear();

            // 여기에서 player 위치 설정
            if (startX.HasValue && startY.HasValue)
            {
                player.X = startX.Value;
                player.Y = startY.Value;
            }
            else if (fromTown == "Town1")
            {
                player.X = 1;
                player.Y = 1;
            }
            else if (fromTown == "Town2")
            {
                player.X = Map.Width - 2;
                player.Y = Map.Height - 2;
            }
        }

        public override void Update()
        {

            while (true)
            {
                map.Draw(player);
                Console.WriteLine("I: 인벤토리 | ESC: 게임 종료 | SPACE: 상호작용");
                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;
                var keyChar = keyInfo.KeyChar;

                if (key == ConsoleKey.Escape)
                {
                    gameManager.ChangeScene(new TownScene1("Field"));
                    break;
                }
                else if (key == ConsoleKey.I || keyChar == 'ㅑ' || keyChar == 'i' || keyChar == 'I')
                {
                    Console.Clear();
                    player.Inventory.ShowInventoryWithEquipOption(player);
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    player.Move(key, map);
                    char tile = map.GetTile(player.X, player.Y);
                    if (tile == 'M')
                    {
                        Random rand = new Random();
                        int index = rand.Next(monsterPool.Count);
                        Monster selected = monsterPool[index];

                        Console.WriteLine($"{selected.Name}이(가) 나타났다!");
                        Console.ReadKey();

                        gameManager.ChangeScene(new BattleScene(player, selected, fromTown, player.X, player.Y));
                        break;
                    }
                    else if (tile == 'O')
                    {

                        if (player.X == 1 && player.Y == 1)
                            gameManager.ChangeScene(new TownScene1("Field"));
                        else if (player.X == Map.Width - 2 && player.Y == Map.Height - 2)
                            gameManager.ChangeScene(new TownScene2());
                        return;
                    }
                }
                else
                {
                    player.Move(key, map);
                }
            }
        }


    }
}
