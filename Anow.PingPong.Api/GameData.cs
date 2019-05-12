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
        private DateTime dt = DateTime.Now;

        public GameData(int id, String one, String two, int sc_one, int sc_two)
        {
            gameID = id;
            player1 = one;
            player2 = two;
            pl1_score = sc_one;
            pl2_score = sc_two;
            dt = DateTime.Now;
        }
        
        public int getID()
        {
            return gameID;
        }

        public bool hasPlayer(string pl)
        {
            return ((pl == player1) || (pl == player2));
        }

        public Object Print()
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
