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
        public object[] Get(int id, String player) //gets games. gets all games but can get by player or ID
        {
            if (id > 0) //game id cant be less than one and the default value for this variable is 0
                return filterById(id);
            else if (player != null) //filters by player
                return filterByPlayer(player);
            else if (Program.games.Count > 0) //if no id or player is passed and there are stored games
            {
                object[] all = new object[Program.games.Count]; 
                for (int i = 0; i < Program.games.Count; i++) //gets info from all stored games
                    all[i] = Program.games[i].Print();


                return all;

            }
            else
                return new[] { "no games recorded yet." };
        }


        [HttpGet("{id}")]
        public object[] filterById(int id)
        {

            if (Program.games.Count > 0) //if there are stored games
            {
                object[] all = new object[1]; //array created. size is one since each id is unique only one result can exist
                for (int i = 0; i < Program.games.Count; i++) //cycles through each game
                    if (Program.games[i].getID().Equals(id)) //checks the id
                        all[0] = Program.games[i].Print(); //when a match is found, it is placed in the array

                if (all[0] != null) //if there is a result
                    return all; //returns the array
                else
                    return new object[] {"no games with that '"+id+"' as id."}; //otherwise returns message. 
                                                                      //deleting the if statement will return an empty array

            }
            else
                return new object[] {"no games recorded yet." };
        }

        //filters the Games by player
        public object[] filterByPlayer(String pl)
        {   //checks to make sure there are games stored
            if (Program.games.Count > 0)
            {
                int counter = 0; //records how many valid games there are 
                object[] all = new object[Program.games.Count];
                for (int i = 0; i < Program.games.Count; i++) //goes through all games 
                    if (Program.games[i].hasPlayer(pl))
                    {
                        all[counter] = Program.games[i].Print(); //stores matching games
                        counter++; //increments the counter for each matching game
                    }

                object[] temp = all; //creates a temp array
                all = new object[Program.games.Count]; //creates an array that is the correct size
                for (int i = 0; i < counter; i++) 
                    all[i] = temp[i];   //transfers all games that meet the criteria to the trimmed array

                if(counter > 0) //if there are games to show
                    return all;
                else        //if there are no games played by the person. to return an empty array simply remove the if statement and return the array
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
        public object Post(String player1, int score1, String player2, int score2, string playDate) //gets input data
        {
            if (isValid(player1, player2, score1, score2, playDate)) //validates input data
            {

                Program.games.Add(new GameData(GenID(), player1, player2, score1, score2, playDate)); //adds the game to the list
                return Get(0, null); //displays all games in list
            }
            else return new { Error = "Invalid data" };
        }

        //validates input data, only checks for valid scores currently
        private bool isValid(String p1, String p2, int sc1, int sc2, string dt)
        {
            bool pass = true; //default to true, if a condition is not met, becomes false
            if (sc1 < 21 && sc2 < 21) //verifiess at least one person got a minimum of 21
                pass = false;
            if (Math.Abs(sc1 - sc2) < 2) //verifies the winner won by at least 2
                pass = false;
            return pass;
        }

        [HttpDelete]
        public object Del(int id) //delete game by id
        {
            int temp = Program.games.Count;
            for (int i = 0; i < temp; i++) //cycles through all games
                if (Program.games[i].getID().Equals(id)) //when matching id is found
                {
                    Program.games.RemoveAt(i);          //the game is removed
                    i = temp;                           //and the loop is stopped
                }
            return Get(0,null);     //after game is deleted, returns remaining games if any exist
        }

        private int GenID() //ID generation, just a simple int but can be modified
        {
            if (Program.games.Count == 0)
                return 1;
            else
                return Program.games[Program.games.Count - 1].getID()+1;
        }

    }
}