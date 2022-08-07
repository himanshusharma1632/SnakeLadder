using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class PlayerController : BaseAPIController
    {
        private readonly GameContext _gameContext;
        public PlayerController(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

    //getting stored players
    [HttpGet("getPlayers")]
    public async Task<ActionResult<IEnumerable<PlayerDTO>>> getPlayer() {
        var player = await _gameContext.player
        .Select(x => ItemToDTO(x))
        .ToListAsync();

        if(player== null) return BadRequest(new ProblemDetails{
            Title = "No Player inside Directory !",
            Status = 404
        });
    return Ok(player);
    }

    //adding a new player
    [HttpPost ("postPlayer")]
    public async Task<ActionResult> postPlayer([FromBody]PlayerDTO playerDTO)
    {
     
   var storedPlayer = _gameContext.player.Where(x => x.userName == playerDTO.userName).FirstOrDefault();
   //checking the condition
    if (storedPlayer != null)
    {
        return BadRequest(new ProblemDetails {
            Title="This is a bad request",
            Status = 400,
            Detail = "The player"+ " " + storedPlayer.name.ToString() + " " + "is already using" + " " + storedPlayer.userName.ToString() + " " + "as username! Try entering another username instead!",
        });
    }
    var addPlayer = new Player {
            name = playerDTO.name,
            userName = playerDTO.userName,
            avatarURL = playerDTO.avatarURL,
            pictureURL = playerDTO.pictureURL,
            description = playerDTO.description,
        };
        //add a DTO player
        _gameContext.player.Add(addPlayer);
        //saving the changes
        await _gameContext.SaveChangesAsync();
        return CreatedAtAction(nameof(getPlayer), new { id = addPlayer.id }, ItemToDTO(addPlayer));
    }   

//editing an existing player
[HttpPut("editPlayer")]
public async Task<IActionResult> editPlayer([FromBody] PlayerDTO playerDTO, int id)
{
    //checking if id exist or not into database
/*if (id != playerDTO.id) return BadRequest(new ProblemDetails {
    Title="This is a bad Request !",
    Status = 400, 
    Detail = "Sorry! The player you are trying to edit,  never exist into database.",
});*/

var updatePlayer = await _gameContext.player.FindAsync(id);
//checking if player is null
if (updatePlayer == null) return NotFound(new ProblemDetails {
    Title="Player Not Found.",
    Status = 404, 
    Detail ="Sorry! The player you are trying to reach is not found.",
});

//updating the player
updatePlayer.name = playerDTO.name;
updatePlayer.userName = playerDTO.userName;
updatePlayer.avatarURL = playerDTO.avatarURL;
updatePlayer.pictureURL = playerDTO.pictureURL;
updatePlayer.description = playerDTO.description;

try {
  await _gameContext.SaveChangesAsync();
}catch(DbUpdateConcurrencyException) when (!playerDTOExist(id)){
return NotFound(new ProblemDetails {
    Title ="Player not found",
    Status = 404, 
    Detail = "Sorry! please try again with other details."
});
}
return Ok(new ProblemDetails {
    Title ="Success !",
    Status = 200,
    Detail ="player" + " " + playerDTO.name.ToString() + " " + "updated successfully!",
});
}

[HttpDelete("deletePlayer")]
public async Task<IActionResult> DeletePlayer(int id)
{
    var playerInfo = await _gameContext.player.FindAsync(id);
    if (playerInfo == null) 
    return NotFound(new ProblemDetails {
        Title = "Could not delete. Requested item does not exists.",
        Status = 404,
    });

    _gameContext.player.Remove(playerInfo);
    await _gameContext.SaveChangesAsync();

    return Ok(new ProblemDetails {
        Title = "Deleted Successfully !",
        Status = 200,
    });
   }

private bool playerDTOExist(int id)
{
    return _gameContext.player.Any(element => element.id == id);   
    }

//converting the player modal to the playerDTO for avoiding cyclic object + Improve Security
private static PlayerDTO ItemToDTO(Player player) => 
    new PlayerDTO {
        id = player.id,
        name = player.name,
        userName = player.userName,
        avatarURL = player.avatarURL,
        pictureURL = player.pictureURL,
        description = player.description,
    };
 }
    }