using System;
namespace Jogo
{
    public class Player
    {
        public string Role;
        public string Id;
        public int Life;
        public string Name;

        public Player(string id, string name = "npc")
        {
            Id = id;
            Name = name;
            Life = 1;
        }

        public void SetRole(string selected)
        {
            if (selected == "c")
            {
                Role = "cidadao";
            }
            else if (selected == "m")
            {
                Role = "mafioso";
            }
            else if (selected == "d")
            {
                Role = "doutor";
            }
            else if(selected == "x")
            {
                Role = "xerife";
            }
        }

        public void Die()
        {
            Life = 0;
        }

        public void Rescue()
        {
            Life = 1;
        }
    }
}
