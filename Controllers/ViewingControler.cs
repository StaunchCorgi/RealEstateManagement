using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementAPI.Dto;
using RealEstateManagementAPI.Models;



[ApiController]
[Route("api/[controller]")]
public class ViewingController : ControllerBase
{
        public readonly AppDbContext _dbContext;

        public ViewingController(AppDbContext dbContext)
        {
             _dbContext = dbContext;   
        }
     [HttpGet]
    public async Task<IActionResult> GetViewings()
    {
        var viewings = await _dbContext.Viewings.ToListAsync();
                                  

        if (!viewings.Any())
        {
            return NotFound(new { message = "No units found" });
        }

        return Ok(viewings);
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetInvoice(int id)
    // {
    //     var unit = await _dbContext.Units
    //                                 .FirstOrDefaultAsync(p => p.Id == id);

    //     if (unit == null)
    //     {
    //         return NotFound(new { message = "Unit not found" });
    //     }

    //     return Ok(unit);
    // }
        
    [HttpPost]
    public async Task<IActionResult> CreateNewViewing([FromBody] CreateViewing request)
    {
        // var existingViewing = _dbContext.Inquiries.FirstOrDefault(u => u.PropertyId == request.PropertyId);
        // if(existingInquiry != null)
        // {
        //     return BadRequest(new {message = "You have already sent an inquiry for this property"});
        // }
        var viewing = new Viewing
        {
            UserId = request.UserId,
            PropertyId = request.PropertyId,
            ScheduledDate = request.ScheduledDate
        };
        try
        {
            _dbContext.Viewings.Add(viewing);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Failed to create viewing" });
        }
        
        return Ok(new { message = "Viewing sent successfully" });
    }
     
    /*[HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginDTO login)
    {
        var user = _dbContext.RegisterUsers.FirstOrDefault(u => u.Username == login.username);
        if(user == null)
        {
            return Unauthorized(new {message = "Invalid username or password."});
        }
        
        /* var passwordMatches = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

    if (!passwordMatches)
    {
        return Unauthorized(new { message = "Invalid username or password." });
    }
        return Ok(new{messge = "Login Successful"});
    }*/
    
}