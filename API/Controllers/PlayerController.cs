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


    [HttpPost ("postPlayer")]
    public async Task<ActionResult> postPlayer([FromBody]PlayerDTO playerDTO)
    {
        var addPlayer = new Player {
            name = playerDTO.name,
            avatarURL = playerDTO.avatarURL,
            pictureURL = playerDTO.pictureURL,
            description = playerDTO.description,
        };
       
       //extra precaution
      /* if (addPlayer.name == playerDTO.name) return BadRequest(new ProblemDetails {
            Title = "This named player already exist !",
            Status = 400,
        });*/
        //add a DTO player
        _gameContext.player.Add(addPlayer);
        //saving the changes
        await _gameContext.SaveChangesAsync();
        return CreatedAtAction(nameof(getPlayer), new { id = addPlayer.id }, ItemToDTO(addPlayer));
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

//converting the player modal to the playerDTO for avoiding cyclic object + Improve Security
private static PlayerDTO ItemToDTO(Player player) => 
    new PlayerDTO {
        id = player.id,
        name = player.name,
        avatarURL = player.avatarURL,
        pictureURL = player.pictureURL,
        description = player.description,
    };
 }
}