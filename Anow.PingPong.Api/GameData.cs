using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anow.PingPong.Api
{
    public class GameData
    {
        private int gameID;
        private String player1 = "";
        private String player2 = "";
        private int pl1_score = 0;
        private int pl2_score = 0;
        private string dt = DateTime.Now.ToShortDateString();

        public GameData(int id, String one, String two, int sc_one, int sc_two, string datetime)
        {
            gameID = id;
            player1 = one;
            player2 = two;
            pl1_score = sc_one;
            pl2_score = sc_two;
            if (datetime != null)
                dt = datetime;
            else
                dt = DateTime.Now.ToString();
        }
        
        public int getID() //gets id
        {
            return gameID;
        }

        public bool hasPlayer(string pl) //checks if the player is in this game
        {
            return ((pl == player1) || (pl == player2));
        }

        public Object Print() //prints the data, format can be changed if different output is desired
        {
            return new
            {
                ID = gameID,
                player_one = player1,
                player_one_score = pl1_score,
                player_two = player2,
                player_two_score = pl2_score,
                time = dt
            };
        }
    }
}
