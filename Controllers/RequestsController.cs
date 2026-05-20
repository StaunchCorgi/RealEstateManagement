using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateManagementAPI.Dto;
using RealEstateManagementAPI.Models;



[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    public readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public RequestsController(AppDbContext dbContext,IConfiguration config)
    {
        _dbContext = dbContext;   
        _configuration = config;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("approve-agent-access-request/{id}")]
    public async Task<IActionResult> ApproveAgentRequest(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id == id);
        
        if(user == null)
        {
            return NotFound(new
            {
                message ="User not found"
            });
        }

        if (!user.AgentRequestPending)
        {
            return BadRequest(new
            {
                message="User has no pending agent request"
            });
        }

        user.Role = Role.Agent;
        user.AgentRequestPending = false;

        await _dbContext.SaveChangesAsync();
   
        return Ok(new
        {
            message = "Agent access approved",
            user = new
            {
                user.Id,
                user.FullName,
                user.Email,
                Role = user.Role.ToString()
            }
        });
    }
    //[Authorize(Roles = "Agent")]
    [HttpGet("agent-access-requests")]
    public async Task<IActionResult> GetAgentRequests()
    {
        var usersRequestingAccess = await _dbContext.Users
        .Where(u=>u.AgentRequestPending)
        .Select(u=> new
        {
            u.Id,
            u.FullName,
            u.Email,
            u.Phone,
            Role = u.Role.ToString()
        }).ToListAsync();

        if (!usersRequestingAccess.Any())
        {
            return NotFound(new{
                 message = "No pending agent access requests found"
            });
        }
        return Ok(usersRequestingAccess);
      
    }

}