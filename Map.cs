using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG_REALREAL
{
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

                tiles[2, 10] = 'N'; // 무기상점
                tiles[Height / 2, Width / 2] = 'S'; // 물약상점
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
}
