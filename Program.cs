namespace TRPGConsoleGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            ShowTitleMenu();
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

    public abstract class Scene
    {

        protected Player player;
        protected GameManager gameManager;

        public virtual void Load()
        {
            Console.WriteLine();
        }

        public virtual void Update()
        {
            Console.WriteLine();
        }

        public virtual void Enter(Player player, GameManager gameManager)
        {
            this.player = player;
            this.gameManager = gameManager;
            Load();
            Update();
        }
    }

    public class Map
    {
        public const int Width = 20;
        public const int Height = 20;
        private char[,] tiles;

        public Map(bool isField = false, string townName = "Town1")
        {
            tiles = new char[Height, Width];
            GenerateMap(isField, townName);
        }

        private void GenerateMap(bool isField, string townName)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (y == 0 || y == Height - 1 || x == 0 || x == Width - 1)
                        tiles[y, x] = '#';
                    else
                        tiles[y, x] = ' ';
                }
            }


            if (isField)
            {

                Random rand = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int mx, my;
                    do
                    {
                        mx = rand.Next(1, Width - 1);
                        my = rand.Next(1, Height - 1);
                    } while (tiles[my, mx] != ' ');
                    tiles[my, mx] = 'M';
                }




                tiles[1, 1] = 'O'; // 마을 1로 가는 문

                tiles[Height - 2, Width - 2] = 'O'; // 마을2로 가는 문
            }

            else
            {
                if (townName != "Town2")
                    tiles[1, Width - 2] = 'O'; // 사냥터로 가는 문

                tiles[2, 10] = 'N'; // NPC
                tiles[Height / 2, Width / 2] = 'S'; // 상점
                tiles[18, 1] = 'H'; // 병원
            }
        }

        public void Draw(Player player)
        {
            Console.Clear();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (player.X == x && player.Y == y)
                        Console.Write('P');
                    else
                        Console.Write(tiles[y, x]);
                }
                Console.WriteLine();
            }
        }

        public char GetTile(int x, int y)
        {
            return tiles[y, x];
        }

        public bool IsWalkable(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height && tiles[y, x] != '#';
        }

        public void SetTile(int x, int y, char tile)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                tiles[y, x] = tile;
            }
        }
    }

    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int HP { get; set; } = 100;
        public int Attack { get; set; } = 10;

        public void GainExp(int amount)
        {
            Console.WriteLine($"{amount} 경험치를 얻었습니다!");
        }

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
    }

    public class Inventory
    {
        public List<string> Items { get; private set; }

        public Inventory()
        {
            Items = new List<string> { "검", "포션" };
        }

        public void ShowInventory()
        {
            Console.WriteLine("인벤토리:");
            foreach (var item in Items)
            {
                Console.WriteLine("- " + item);
            }
        }
    }


    class Monster
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }

        public Monster(string name, int hp, int atk)
        {
            Name = name;
            HP = hp;
            Attack = atk;
        }
    }

    // BattleScene.cs
    class BattleScene : Scene
    {
        private Player player;
        private Monster monster;

        public BattleScene(Player player, Monster monster)
        {
            this.player = player;
            this.monster = monster;
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
                    player.GainExp(10);
                    Console.ReadKey();
                    gameManager.ChangeScene(new FieldScene());
                    return;
                }

                player.HP -= monster.Attack;
                Console.WriteLine($"{monster.Name}이(가) {monster.Attack} 데미지를 입혔습니다!");
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

    public class TownScene1 : Scene
    {
        private Map map;
        private string fromWhere;

        public TownScene1(string fromWhere = "")
        {
            this.fromWhere = fromWhere;
        }

        public override void Load()
        {
            Console.Clear();
            map = new Map(false, "Town1");

            if (fromWhere == "Field")
            {
                player.X = 18;
                player.Y = 1;
            }
            else
            {
                player.X = 1;
                player.Y = 1;
            }
        }


        public override void Update()
        {

            

            while (true)
            {
                map.Draw(player);
                Console.WriteLine("I: 인벤토리 | ESC: 종료 | SPACE: 상호작용");
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                    break;
                else if (key == ConsoleKey.I)
                {
                    Console.Clear();
                    player.Inventory.ShowInventory();
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    char tile = map.GetTile(player.X, player.Y);
                    if (tile == 'O')
                    {
                        gameManager.ChangeScene(new FieldScene("Town1"));
                        break;
                    }
                    else if (tile == 'N')
                    {
                        Console.Clear();
                        Console.WriteLine("NPC: 안녕하세요, 용사님! 좋은 무기와 방어구가 준비되어 있어요.");
                        Console.WriteLine("- 무기: 검, 활\n- 방어구: 갑옷, 방패");
                        Console.ReadKey();
                    }
                    else if (tile == 'S')
                    {
                        Console.Clear();
                        Console.WriteLine("상점: 물건을 사시겠습니까?");
                        Console.ReadKey();
                    }
                    else if (tile == 'H')
                    {
                        Console.Clear();
                        Console.WriteLine("NPC: 당신, 부상을 입었군요? 치료해드릴까요?");
                        Console.ReadKey();
                    }
                }
                else
                {
                    player.Move(key, map);
                }
            }
        }
       

      
        
    }

    public class FieldScene : Scene
    {
        private Map map;
        private string fromTown;

        public FieldScene(string fromTown = "Town1")
        {
            this.fromTown = fromTown;
            map = new Map(true, fromTown);
        }

        public override void Load()
        {
            Console.Clear();

            // 여기에서 player 위치 설정
            if (fromTown == "Town1")
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
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                {
                    gameManager.ChangeScene(new TownScene1("Field"));
                    break;
                }
                else if (key == ConsoleKey.I)
                {
                    Console.Clear();
                    player.Inventory.ShowInventory();
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    player.Move(key, map);
                    char tile = map.GetTile(player.X, player.Y);
                    if (tile == 'M')
                    {
                        gameManager.ChangeScene(new BattleScene(player, new Monster("고블린", 30, 5)));
                        break;
                    }
                    else if (tile == 'O')
                    {
                        // O를 밟고 이동할 때, 현재 위치로부터 어느 마을로 가는지 결정
                        if (player.X == 1 && player.Y == 1)
                            gameManager.ChangeScene(new TownScene1("Field"));
                        else if (player.X == Map.Width - 2 && player.Y == Map.Height - 2)
                            gameManager.ChangeScene(new TownScene2()); // 오른쪽 아래 → Town2
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

    public class TownScene2 : Scene
    {

        
        private Map map = new Map(false, "Town2");

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
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                    break;
                else if (key == ConsoleKey.I)
                {
                    Console.Clear();
                    player.Inventory.ShowInventory();
                    Console.ReadKey();
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    player.Move(key, map);
                    char tile = map.GetTile(player.X, player.Y);
                    if (tile == 'O')
                    {
                        gameManager.ChangeScene(new FieldScene("Town2"));
                        return;
                    }
                    else if (tile == 'N')
                    {
                        Console.Clear();
                        Console.WriteLine("NPC: 환영합니다, 용사님! 좋은 무기와 방어구가 준비되어 있어요.");
                        Console.WriteLine("- 무기: 검, 활\n- 방어구: 갑옷, 방패");
                        Console.ReadKey();
                    }
                    else if (tile == 'S')
                    {
                        Console.Clear();
                        Console.WriteLine("상점: 필요한 물건이 있나요?");
                        Console.ReadKey();
                    }
                    else if (tile == 'H')
                    {
                        Console.Clear();
                        Console.WriteLine("NPC: 당신, 부상을 입었군요. 치료해드릴까요?");
                        Console.ReadKey();
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
