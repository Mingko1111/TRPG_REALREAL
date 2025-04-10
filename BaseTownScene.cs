using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG_REALREAL
{
    public abstract class BaseTownScene : Scene
    {
        protected Map map;
        protected Dictionary<string, int> weaponShopItems = new Dictionary<string, int>
    {
        { "나무검", 100 },
        { "나무갑옷", 120 },
        { "철검", 250 },
        { "철갑옷", 300 },
        { "영웅검", 1000 },
        { "영웅갑옷", 1200 }
    };

        protected Dictionary<string, int> potionShopItems = new Dictionary<string, int>
    {
        { "회복약", 50 },
        { "고급 회복약", 120 },
        { "정령의 가호", 300 }
    };

        protected void HandleShopInteraction(Dictionary<string, int> items, string shopName)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"== {shopName} ==");
                Console.WriteLine("1. 구매하기\n2. 판매하기\n3. 나가기");
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D1)
                {
                    Console.Clear();
                    Console.WriteLine($"[구매 가능한 아이템 목록]   (보유 골드: {player.Gold} G)");  // 💰 현재 골드 출력!

                    int i = 1;
                    foreach (var item in items)
                    {
                        Console.WriteLine($"{i}. {item.Key} - {item.Value} G");
                        i++;
                    }

                    Console.Write("구매할 아이템 번호 입력(돌아가기는 Enter 두번): ");
                    if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= items.Count)
                    {
                        string itemName = items.ElementAt(choice - 1).Key;
                        int price = items[itemName];

                        if (player.SpendGold(price))
                        {
                            player.Inventory.AddItem(itemName);
                            Console.WriteLine($"{itemName}을(를) 구매했습니다!");
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                        }
                    }
                    else Console.WriteLine("잘못된 입력입니다.(이전 메뉴로 돌아가려면 Enter를 누르세요)");
                    Console.ReadKey();
                }
                else if (input == ConsoleKey.D2)
                {
                    Console.Clear();
                    Console.WriteLine("[판매 가능한 아이템 목록]");
                    var inventoryItems = player.Inventory.GetItems();
                    for (int i = 0; i < inventoryItems.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {inventoryItems[i]}");
                    }
                    Console.Write("판매할 아이템 번호 입력(돌아가기는 Enter 두번): ");
                    if (int.TryParse(Console.ReadLine(), out int sellChoice) && sellChoice > 0 && sellChoice <= inventoryItems.Count)
                    {
                        string itemName = inventoryItems[sellChoice - 1];

                        if (items.TryGetValue(itemName, out int originalPrice))
                        {
                            int sellPrice = (int)(originalPrice * 0.6);
                            player.AddGold(sellPrice);
                            player.Inventory.RemoveItem(itemName);
                            Console.WriteLine($"{itemName}을(를) {sellPrice} G에 판매했습니다!");
                        }
                        else
                        {
                            Console.WriteLine("이 상점에서는 이 아이템을 구매하지 않습니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                    Console.ReadKey();
                }
                else if (input == ConsoleKey.D3)
                    break;
            }
        }

        protected void HandleHospital()
        {
            Console.Clear();
            Console.WriteLine("NPC: 당신, 부상을 입었군요?");
            Console.WriteLine();
            Console.WriteLine("지금 치료해드릴게요!");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("          ■■■   ");
            Console.WriteLine("          ■■■   ");
            Console.WriteLine("      ■■■■■■■■■■■     ");
            Console.WriteLine("      ■■■■■■■■■■■     ");
            Console.WriteLine("          ■■■     ");
            Console.WriteLine("          ■■■     ");
            player.HP = player.MaxHP;
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("당신의 체력이 모두 회복되었습니다!");
            Console.ReadKey();
        }
    }
}
