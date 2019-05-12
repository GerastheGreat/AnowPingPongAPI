using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anow.PingPong.Api.Controllers
{
    [Produces("application/json")]
    [Route("Games")]
    public class GamesController : Controller
    {

        [HttpGet]
        public object[] Get(int id, String pl)
        {
            if (id > 0)
                return filterById(id);
            else if (pl != null)
                return filterByPlayer(pl);
            else if (Program.games.Count > 0)
            {
                object[] all = new object[Program.games.Count];
                for (int i = 0; i < Program.games.Count; i++)
                    all[i] = Program.games[i].Print();
                

                return all;

            }
            else
                return new []{"no games recorded yet."};
        }


        private object[] filterById(int id)
        {
            if (Program.games.Count > 0)
            {
                object[] all = new object[1];
                for (int i = 0; i < Program.games.Count; i++)
                    if (Program.games[i].getID() == id)
                        all[0] = Program.games[i].Print();

                if (all.Length > 1)
                    return all;
                else
                    return new object[] {"this person has played no games."};

            }
            else
                return new object[] {"no games recorded yet." };
        }

        private object[] filterByPlayer(String pl)
        {
            if (Program.games.Count > 0)
            {
                object[] all = new object[Program.games.Count];
                for (int i = 0; i < Program.games.Count; i++)
                    if(Program.games[i].hasPlayer(pl))
                        all[i] = Program.games[i].Print();

                if(all.Length > 1)
                    return all;
                else
                    return new object[] {"this person has played no games."};
            }
            else
                return new object[] {"no games recorded yet." };
        }

        [HttpPut]
        public object Put()
        {
             return new { Method = "not usable yet."};
        }

        [HttpPost]
        public object Post(String p1,String p2, int sc1, int sc2)
        {
            if (isValid(p1,p2,sc1,sc2))
            {

                Program.games.Add(new GameData(GenID(),p1, p2, sc1, sc2));
                return Get(0, null);
            }
            else return new { Error = "Invalid data" };
        }

        private bool isValid(String p1, String p2, int sc1, int sc2)
        {
            bool pass = true;
            if (sc1 < 21 && sc2 < 21)
                pass = false;
            if (Math.Abs(sc1 - sc2) < 2)
                pass = false;
            return pass;
        }

        [HttpDelete]
        public object Del(int id)
        {
            int temp = Program.games.Count;
            for (int i = 0; i < temp; i++)
                if (Program.games[i].getID().Equals(id))
                {
                    Program.games.RemoveAt(i);
                    i = temp;
                }
            return Get(0,null);
        }

        private int GenID()
        {
            if (Program.games.Count == 0)
                return 1;
            else
                return Program.games[Program.games.Count - 1].getID()+1;
        }

    }
}