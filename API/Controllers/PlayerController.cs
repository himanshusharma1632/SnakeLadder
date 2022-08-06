using System.Collections.Generic;
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

    [HttpGet("GetPlayers")]
    public async Task<ActionResult<List<Player>>> getPlayer() {
        var player = await _gameContext.player.ToListAsync();
        if(player== null) return BadRequest(new ProblemDetails{
            Title = "No Player inside Directory !",
            Status = 404
        });
        return Ok(player);
    }

    [HttpPost ("PostPlayer")]
    public async Task<ActionResult<Player>> postPlayer(Player player)
    {
        var addPlayer = _gameContext.player.Add(player);
        if (addPlayer == null) return BadRequest(new ProblemDetails {
            Title = "Could Not Add a Player (Please Enter Valid Details)",
            Status = 400,
        });
       
       await _gameContext.SaveChangesAsync();
       return CreatedAtAction(nameof(getPlayer), new { name = player.name }, player);
    }   

    [HttpDelete("DeletePlayer")]
public async Task<IActionResult> DeletePlayer(Player player, int id)
{
    var playerInfo = await _gameContext.player.FindAsync(id);
    if (id != player.id)
    {
        return NotFound(new ProblemDetails {
            Title="You might have already deleted this item , or it does not exits into the database.",
            Status = 404,
        });
    }

    _gameContext.player.Remove(playerInfo);
    _gameContext.player.Update(playerInfo);
    await _gameContext.SaveChangesAsync();

    return NoContent();
}
 }
}