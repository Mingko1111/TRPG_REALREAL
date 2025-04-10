using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG_REALREAL
{
    class Monster
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }

        public Monster(string name, int hp, int atk, int rewardGold = 50, int rewardExp = 10)
        {
            Name = name;
            HP = hp;
            Attack = atk;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
        }

    }
}
