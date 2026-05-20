using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementAPI.Dto;
using RealEstateManagementAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    public readonly AppDbContext _dbContext;

    public PropertiesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;   
    }

    [HttpGet("my-listings")]
    public async Task<IActionResult> GetAgentListings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var listings = await _dbContext.Properties.Where(p=>p.AgentId == userId).ToListAsync();

        return Ok(listings);
    }

    [HttpGet]
    public async Task<IActionResult> GetProperties()
    {
        var properties = await _dbContext.Properties
                                    .ToListAsync();

        if (!properties.Any())
        {
            return NotFound(new { message = "No properties found" });
        }

        return Ok(properties);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProperty(int id)
    {
        var property = await _dbContext.Properties
                                    .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null)
        {
            return NotFound(new { message = "Property not found" });
        }

        return Ok(property);
    }

    [Authorize(Roles = "Agent")]   
    [HttpPost("create")]
    public async Task<IActionResult> CreateNewProperty([FromBody] CreatePropertyDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
        {     
            return Unauthorized(new { message = "Invalid token - missing user id" });
        }

         var userId = int.Parse(userIdClaim);


        var existingProperty = _dbContext.Properties.FirstOrDefault(a => a.Title == request.Title);
        if(existingProperty != null)
        {
            return BadRequest(new {message = "Property already exists"});
        }
        var property = new Property
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Location = request.Location,
            PropertyType = request.PropertyType,
            Status = request.Status,
            AgentId = userId,
        };
        try
        {
            _dbContext.Properties.Add(property);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var error = ex.Message;
            return BadRequest(new { message = "Failed to add property", ex.Message});
        }
        
        return Ok(new { message = "Property added successfully" });
    }

}