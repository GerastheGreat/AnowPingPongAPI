using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anow.PingPong.Api
{
    public class GameData
    {
        public string id { set; get; } = "0";
        public String player1 { set; get; } 
        public String player2 { set; get; }
        public int score1 { set; get; } = -1;
        public int score2 { set; get; } = -1;
        public string playDate { set; get; }

        
        
        public bool hasPlayer(string pl) //checks if the player is in this game
        {
            return ((pl == player1) || (pl == player2));
        }
        
    }
}
