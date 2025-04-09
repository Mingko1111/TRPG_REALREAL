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

        public int Gold { get; private set; } = 500;

        public int HP { get; set; } = 100;

        public int MaxHP { get; set; } = 100;
        public int CurrentHP { get; set; } = 50;
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

        // 아이템 추가
        public void AddItem(string itemName)
        {
            Items.Add(itemName);
            Console.WriteLine($"{itemName}이(가) 인벤토리에 추가되었습니다.");
        }

        // 아이템 제거
        public void RemoveItem(string itemName)
        {
            if (Items.Contains(itemName))
            {
                Items.Remove(itemName);
                Console.WriteLine($"{itemName}이(가) 인벤토리에서 제거되었습니다.");
            }
            else
            {
                Console.WriteLine($"{itemName}은(는) 인벤토리에 없습니다.");
            }
        }

        // 현재 아이템 리스트 반환
        public List<string> GetItems()
        {
            return new List<string>(Items); // 복사본 반환 (원본 보호)
        }
    }



    class Monster
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int RewardGold { get; set; }

        public Monster(string name, int hp, int atk, int rewardGold = 50)
        {
            Name = name;
            HP = hp;
            Attack = atk;
            RewardGold = rewardGold;
        }




    }



    class BattleScene : Scene
    {

        private int returnX;
        private int returnY;
        private string fromTown;
        private Player player;
        private Monster monster;

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
                    player.GainExp(10);
                    player.AddGold(monster.RewardGold);
                    Console.ReadKey();
                    gameManager.ChangeScene(new FieldScene(fromTown, returnX, returnY));
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



    public abstract class BaseTownScene : Scene
    {
        protected Map map;
        protected Dictionary<string, int> weaponShopItems = new Dictionary<string, int>
    {
        { "나무검", 100 },
        { "나무갑옷", 120 },
        { "철검", 250 },
        { "철갑옷", 300 }
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
                    Console.WriteLine("[구매 가능한 아이템 목록]");
                    int i = 1;
                    foreach (var item in items)
                    {
                        Console.WriteLine($"{i}. {item.Key} - {item.Value} G");
                        i++;
                    }
                    Console.Write("구매할 아이템 번호 입력: ");
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
                    else Console.WriteLine("잘못된 입력입니다.");
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
                    Console.Write("판매할 아이템 번호 입력: ");
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
            Console.WriteLine("NPC: 당신, 부상을 입었군요. 치료해드릴까요?");
            player.CurrentHP = player.MaxHP;
            Console.WriteLine("당신의 체력이 모두 회복되었습니다!");
            Console.ReadKey();
        }
    }



    public class TownScene1 : BaseTownScene
    {
        private string fromWhere;

        public TownScene1(string fromWhere = "")
        {
            this.fromWhere = fromWhere;
        }

        public override void Load()
        {
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



    public class FieldScene : Scene
    {
        private Map map;
        private string fromTown;
        private int? startX;
        private int? startY;
        private List<Monster> monsterPool = new List<Monster>

        {
            new Monster("고블린", 30, 5, 50),
            new Monster("슬라임", 20, 3, 30),
            new Monster("오크", 60, 10, 80),
            new Monster("늑대", 40, 8, 60),
            new Monster("박쥐", 25, 4, 40)
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
