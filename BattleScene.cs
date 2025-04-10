using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TRPG_REALREAL
{
    class BattleScene : Scene
    {

        private int returnX;
        private int returnY;
        private string fromTown;
        private Player player;
        private Monster monster;
        public int RewardExp { get; set; } = 10;

        public BattleScene(Player player, Monster monster, string fromTown, int returnX, int returnY)
        {
            this.player = player;
            this.monster = monster;
            this.fromTown = fromTown;
            this.returnX = returnX;
            this.returnY = returnY;
        }

        public override void Load()
        {
            Console.Clear();
            Console.WriteLine($"전투 시작! {monster.Name} 출현!");
        }

        public override void Update()
        {
            while (player.HP > 0 && monster.HP > 0)
            {
                Console.WriteLine($"플레이어 HP: {player.HP}  몬스터 HP: {monster.HP}");
                Console.WriteLine("공격하려면 아무 키나 누르세요.");
                Console.ReadKey();

                monster.HP -= player.Attack;
                Console.WriteLine($"{monster.Name}에게 {player.Attack} 데미지를 입혔습니다!");

                if (monster.HP <= 0)
                {
                    Console.WriteLine("몬스터 처치!");
                    player.GainExp(monster.RewardExp);
                    player.AddGold(monster.RewardGold);
                    Console.ReadKey();
                    gameManager.ChangeScene(new FieldScene(fromTown, returnX, returnY));
                    return;

                }

                int damage = Math.Max(monster.Attack - player.Defense, 0);
                player.HP -= damage;
                Console.WriteLine($"{monster.Name}이(가) {damage} 데미지를 입혔습니다! (방어력 {player.Defense} 적용)");
                if (player.HP <= 0)
                {
                    Console.WriteLine("사망했습니다. 게임 오버.");
                    Console.ReadKey();
                    gameManager.ExitGame();
                }
            }
        }


        public override void Enter(Player player, GameManager gameManager)
        {
            this.player = player;
            this.gameManager = gameManager;
            Load();
            Update();
        }
    }
}
