using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TRPG_REALREAL
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Gold { get; private set; } = 500;

        public int HP { get; set; } = 100;

        public int MaxHP { get; set; } = 100;
        public int BaseAttack { get; private set; } = 10;
        public int BaseDefense { get; private set; } = 0;

        public int Attack => BaseAttack + Inventory.GetWeaponBonus();
        public int Defense => BaseDefense + Inventory.GetArmorBonus();

        public int Level { get; private set; } = 1;
        public int CurrentExp { get; private set; } = 0;
        public int ExpToNextLevel => Level * 100;

        public Inventory Inventory { get; private set; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Inventory = new Inventory();
        }


        public void Move(ConsoleKey key, Map map)
        {
            int newX = X;
            int newY = Y;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newY--;
                    break;
                case ConsoleKey.DownArrow:
                    newY++;
                    break;
                case ConsoleKey.LeftArrow:
                    newX--;
                    break;
                case ConsoleKey.RightArrow:
                    newX++;
                    break;
            }

            if (map.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
            }
        }

        public void AddGold(int amount)
        {
            Gold += amount;
            Console.WriteLine($"{amount} G를 획득했습니다. 현재 보유 골드: {Gold} G");
        }

        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                Console.WriteLine($"{amount} G를 사용했습니다. 남은 골드: {Gold} G");
                return true;
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
                return false;
            }
        }
        public void UseItemFromInventory(string itemName)
        {
            Inventory.UseItem(itemName, this);
        }

        public void GainExp(int amount)
        {
            CurrentExp += amount;
            Console.WriteLine($"{amount} 경험치를 얻었습니다! (총: {CurrentExp}/{ExpToNextLevel})");

            while (CurrentExp >= ExpToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            CurrentExp -= ExpToNextLevel;
            Level++;
            MaxHP += 20;
            HP = MaxHP; // 회복
            BaseAttack += 5;

            Console.WriteLine($"레벨업! 현재 레벨: {Level}");
            Console.WriteLine($"최대 체력 증가: {MaxHP}, 공격력 증가: {Attack}");
        }


    }
}
