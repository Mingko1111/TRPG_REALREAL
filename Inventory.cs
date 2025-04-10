using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG_REALREAL
{
    public class Inventory
    {
        public List<string> Items { get; private set; }
        public string EquippedWeapon { get; private set; }
        public string EquippedArmor { get; private set; }

        private Player player;



        private bool IsConsumable(string item)
        {
            return item == "회복약" || item == "고급 회복약" || item == "정령의 가호";
        }

        public Inventory()
        {
            Items = new List<string> { "흔한검", "흔한갑옷", "회복약" };
            EquippedWeapon = null;
            EquippedArmor = null;
        }


        private Dictionary<string, int> weaponStats = new Dictionary<string, int>
        {
            { "흔한검", 5 },
            { "철검", 10 },
            { "영웅검", 50 }
        };

        private Dictionary<string, int> armorStats = new Dictionary<string, int>
        {
            { "흔한갑옷", 3 },
            { "철갑옷", 8 },
            { "영웅갑옷", 30 }
        };

        public int GetWeaponBonus()
        {
            if (EquippedWeapon != null && weaponStats.ContainsKey(EquippedWeapon))
                return weaponStats[EquippedWeapon];
            return 0;
        }

        public int GetArmorBonus()
        {
            if (EquippedArmor != null && armorStats.ContainsKey(EquippedArmor))
                return armorStats[EquippedArmor];
            return 0;
        }





        public void ShowInventoryWithEquipOption(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리:");
                foreach (var item in Items)
                {
                    Console.WriteLine("- " + item);
                }

                Console.WriteLine($"장착 중인 무기: {EquippedWeapon ?? "없음"}");
                Console.WriteLine($"장착 중인 방어구: {EquippedArmor ?? "없음"}");

                Console.WriteLine();
                Console.WriteLine("아이템을 입력하세요 (무기 해제 / 방어구 해제 / 종료는 엔터)");
                Console.Write("> ");

                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    break; // 종료

                if (input == "무기 해제")
                {
                    UnequipItem("무기");
                }
                else if (input == "방어구 해제")
                {
                    UnequipItem("방어구");
                }
                else if (IsConsumable(input))
                {
                    UseItem(input, player); // 소비 아이템이면 사용
                }
                else
                {
                    EquipItem(input); // 장비 아이템이면 장착
                }

                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true);
            }
        }
        private bool IsWeapon(string item)
        {
            return item.Contains("검");
        }

        private bool IsArmor(string item)
        {
            return item.Contains("갑옷");
        }

        public void AddItem(string itemName)
        {
            Items.Add(itemName);
            Console.WriteLine($"{itemName}이(가) 인벤토리에 추가되었습니다.");
        }

        public void RemoveItem(string itemName)
        {
            if (Items.Contains(itemName))
            {
                // 장착 중인 아이템이면 장착 해제
                if (EquippedWeapon == itemName || EquippedArmor == itemName)
                {
                    UnequipItem("장비");
                }

                Items.Remove(itemName);
                Console.WriteLine($"{itemName}이(가) 인벤토리에서 제거되었습니다.");
            }
            else
            {
                Console.WriteLine($"{itemName}은(는) 인벤토리에 없습니다.");
            }
        }

        public void EquipItem(string itemName)
        {
            if (!Items.Contains(itemName))
            {
                Console.WriteLine($"{itemName}은(는) 인벤토리에 없습니다.");
                return;
            }

            if (IsWeapon(itemName))
            {
                EquippedWeapon = itemName;
                Console.WriteLine($"{itemName} (무기)을(를) 장착했습니다.");
            }
            else if (IsArmor(itemName))
            {
                EquippedArmor = itemName;
                Console.WriteLine($"{itemName} (방어구)을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine($"{itemName}은(는) 장비할 수 없는 아이템입니다.");
            }
        }

        public void HandleEquip()
        {
            ShowInventoryWithEquipOption(player);
            Console.WriteLine("장착/사용할 아이템 이름을 입력하세요 (또는 '해제' 입력 시 장착 해제):");
            string itemName = Console.ReadLine();

            if (itemName == "해제")
            {
                UnequipItem("");
            }
            else
            {
                EquipItem(itemName);
            }
        }

        public void UnequipItem(string type)
        {
            if (type == "무기")
            {
                if (EquippedWeapon != null)
                {
                    Console.WriteLine($"{EquippedWeapon} (무기) 장착 해제됨.");
                    EquippedWeapon = null;
                }
                else
                {
                    Console.WriteLine("장착 중인 무기가 없습니다.");
                }
            }
            else if (type == "방어구")
            {
                if (EquippedArmor != null)
                {
                    Console.WriteLine($"{EquippedArmor} (방어구) 장착 해제됨.");
                    EquippedArmor = null;
                }
                else
                {
                    Console.WriteLine("장착 중인 방어구가 없습니다.");
                }
            }
        }

        public List<string> GetItems()
        {
            return new List<string>(Items);
        }

        public void UseItem(string itemName, Player player)
        {
            if (!Items.Contains(itemName))
            {
                Console.WriteLine($"{itemName}은(는) 인벤토리에 없습니다.");
                return;
            }

            int healAmount = 0;

            switch (itemName)
            {
                case "회복약":
                    healAmount = 30;
                    break;
                case "고급 회복약":
                    healAmount = 60;
                    break;
                case "정령의 가호":
                    healAmount = 100;
                    break;
                default:
                    Console.WriteLine($"{itemName}은(는) 사용할 수 없는 아이템입니다.");
                    return;
            }

            if (player.HP >= player.MaxHP)
            {
                Console.WriteLine("체력이 이미 가득 찼습니다.");
                return;
            }

            player.HP = Math.Min(player.HP + healAmount, player.MaxHP);
            Items.Remove(itemName);
            Console.WriteLine($"{itemName}을(를) 사용하여 체력을 {healAmount} 회복했습니다. 현재 체력: {player.HP}/{player.MaxHP}");
        }

    }

}
