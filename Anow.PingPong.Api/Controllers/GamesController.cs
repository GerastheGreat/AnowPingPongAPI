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
        [HttpGet("{id}")]
        public object[] Get(string id, string player) //gets games. gets all games but can get by player or ID
        {

            object[] all = new object[Program.games.Count];

            if (id != null) //game id cant be less than one and the default value for this variable is 0
                all = filterById(id);
            else if (player != null) //filters by player
                all = filterByPlayer(player);
            else if (Program.games.Count > 0) //if no id or player is passed and there are stored games
                for (int i = 0; i < Program.games.Count; i++) //gets info from all stored games
                    all[i] = Program.games[i];

            return all; //return the array
        }

        //finds the game with the matching ID, if it exists
        public object[] filterById(string id)
        {

            object[] all = new object[1]; //array created. size is one since each id is unique only one result can exist

            if (Program.games.Count > 0) //if there are stored games
            {
                for (int i = 0; i < Program.games.Count; i++) //cycles through each game
                    if (Program.games[i].id.Equals(id)) //checks the id
                    {
                        all[0] = Program.games[i]; //when a match is found, it is placed in the array
                        i = Program.games.Count;    //to exit the loop
                    }
                    

            }
            if (all[0] == null)
            {
                all = new GameData[0];                 //sets the array to 0 size, so no nulls are returned
                this.HttpContext.Response.StatusCode = 404; //sets status code to 404, since no games with that game id are found
            }
            return all; //returns the array
        }

        //filters the Games by player
        public object[] filterByPlayer(String pl)
        {

            object[] all = new object[Program.games.Count];

            //checks to make sure there are games stored
            if (Program.games.Count > 0)
            {
                int counter = 0; //records how many valid games there are 
                for (int i = 0; i < Program.games.Count; i++) //goes through all games 
                    if (Program.games[i].hasPlayer(pl))
                    {
                        all[counter] = Program.games[i]; //stores matching games
                        counter++; //increments the counter for each matching game
                    }

                object[] temp = all; //creates a temp array
                all = new object[counter]; //creates an array that is the correct size
                for (int i = 0; i < counter; i++) 
                    all[i] = temp[i];   //transfers all games that meet the criteria to the trimmed array

            }


            return all;
        }

        [HttpPut]
        public object[] Put([FromBody] GameData gd)
        {
            if (gd != null)         //if any mistakes when creating and passing the object, gd will be null and it'll crash when trying to access it
            {
                GameData old = null; //the game to be updated
                int location = 0;   //the game location in the ist

                for (int i = 0; i < Program.games.Count; i++)
                    if (gd.id.Equals(Program.games[i].id))
                    {
                        old = Program.games[i];
                        location = i;
                        i = Program.games.Count;
                    }

                if (old != null) //if a game is found
                {   //fill in the missing data
                    if (gd.player1 == null)
                        gd.player1 = old.player1;
                    if (gd.player2 == null)
                        gd.player2 = old.player2;
                    if (gd.score1 == -1)
                        gd.score1 = old.score1;
                    if (gd.score2 == -1)
                        gd.score2 = old.score2;
                    if (gd.playDate == null)
                        gd.playDate = old.playDate;

                    if (isValid(gd))    //checks if the data is still valid
                        Program.games[location] = gd; //replaces the gamedata with the updated version
                }
                else
                    this.HttpContext.Response.StatusCode = 404; //sets status code to 404, since no games with that game id are found
                                                                //just like in the Get Id request
            }
            return Get(null, null);
        }

        [HttpPost]
        public object[] Post([FromBody] GameData gd) //gets input data
        {
            if (gd != null)         //if any mistakes when creating and passing the object, gd will be null and it'll crash when trying to access it
            { 
                gd.id = GenID();        //sets the gameID
                if (gd.playDate == null) gd.playDate = DateTime.Now.ToString(); //was being set on create when no value was given, but then it cant be verified when a put request is made
                if (isValid(gd))              //if it's valid
                {
                    Program.games.Add(gd);  //adds it to the list
                    this.HttpContext.Response.StatusCode = 201; //success returns 201
                }
            }
            return Get(null, null);     //prints all games
            
        }

        //validates input data, only checks for valid scores currently
        private bool isValid(GameData gd)
        {
            bool pass = true; //default to true, if a condition is not met, becomes false
            if ((gd.score1 < 21 && gd.score2 < 21) || gd.score1 < 0 || gd.score2 < 0) //verifiess at least one person got a minimum of 21 and that no score is less than 0
                pass = false;
            if (Math.Abs(gd.score1 - gd.score2) < 2) //verifies the winner won by at least 2
                pass = false;
            if (gd.player1.Length == 0 || gd.player2.Length == 0)
                pass = false;
            return pass;
        }
        

        [HttpDelete]
        public object[] Del(string id) //delete game by id
        {
            int temp = Program.games.Count;
            for (int i = 0; i < temp; i++) //cycles through all games
                if (Program.games[i].id.Equals(id)) //when matching id is found
                {
                    Program.games.RemoveAt(i);          //the game is removed
                    i = temp;                           //and the loop is stopped
                }
            return Get(null, null);     //after game is deleted, returns remaining games if any exist
        }

        private string GenID() //ID generation, just a simple int but can be modified
        {
            if (Program.games.Count == 0)
                return "1";
            else
                return ""+(Convert.ToInt32(Program.games[Program.games.Count - 1].id)+1);
        }

    }
}