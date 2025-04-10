using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG_REALREAL
{
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
}
