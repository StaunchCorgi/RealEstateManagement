using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[ApiController]
[Route("api/[controller]")]
public class UnitController : ControllerBase
{
    // public readonly AppDbContext _dbContext;

    // public UnitController(AppDbContext dbContext)
    // {
    //     _dbContext = dbContext;   
    // }
    //  [HttpGet]
    // public async Task<IActionResult> GetUnits()
    // {
    //     var units = await _dbContext.Units.ToListAsync();
                                  

    //     if (!units.Any())
    //     {
    //         return NotFound(new { message = "No units found" });
    //     }

    //     return Ok(units);
    // }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetUnit(int id)
    // {
    //     var unit = await _dbContext.Units
    //                                 .FirstOrDefaultAsync(p => p.Id == id);

    //     if (unit == null)
    //     {
    //         return NotFound(new { message = "Unit not found" });
    //     }

    //     return Ok(unit);
    // }
        
    // [HttpPost]
    // public async Task<IActionResult> CreateNewUnit([FromBody] Unit request)
    // {
    //     var existingUnit = _dbContext.Units.FirstOrDefault(u => u.UnitNumber == request.UnitNumber);
    //     if(existingUnit != null)
    //     {
    //         return BadRequest(new {message = "Unit already exists"});
    //     }
    //     var unit = new Unit
    //     {
    //         PropertyId = request.PropertyId,
    //         UnitNumber = request.UnitNumber,
    //         RentAmount = request.RentAmount,
    //         Status = request.Status,
    //     };
    //     try
    //     {
    //         _dbContext.Units.Add(unit);
    //         await _dbContext.SaveChangesAsync();
    //     }
    //     catch (Exception)
    //     {
    //         return BadRequest(new { message = "Failed to add unit" });
    //     }
        
    //     return Ok(new { message = "Unit added successfully" });
    // }
     
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