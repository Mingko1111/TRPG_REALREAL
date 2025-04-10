using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG_REALREAL
{
    public class TownScene2 : BaseTownScene
    {
        public override void Load()
        {
            map = new Map(false, "Town2");
            map.SetTile(1, 1, 'O');
            player.X = 1;
            player.Y = 1;
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
                    break;
                else if (key == ConsoleKey.I || keyChar == 'ㅑ' || keyChar == 'i' || keyChar == 'I')
                {
                    Console.Clear();
                    player.Inventory.ShowInventoryWithEquipOption(player);
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    char tile = map.GetTile(player.X, player.Y);
                    if (tile == 'O')
                    {
                        gameManager.ChangeScene(new FieldScene("Town2"));
                        return;
                    }
                    else if (tile == 'N')
                    {
                        HandleShopInteraction(weaponShopItems, "무기/방어구 상점");
                    }
                    else if (tile == 'S')
                    {
                        HandleShopInteraction(potionShopItems, "포션 상점");
                    }
                    else if (tile == 'H')
                    {
                        HandleHospital();
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
