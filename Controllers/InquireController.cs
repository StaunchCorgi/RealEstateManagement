using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementAPI.Dto;
using RealEstateManagementAPI.Models;



[ApiController]
[Route("api/[controller]")]
public class InquireController : ControllerBase
{
    public readonly AppDbContext _dbContext;

    public InquireController(AppDbContext dbContext)
    {
        _dbContext = dbContext;   
    }


[Authorize]
    [HttpGet("agent-inquiries")]
    public async Task<IActionResult> GetAgentInquiries()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         
         if(agentId == null)
        {
            return Unauthorized(new
            {
                message = "Invalid Token"
            });
        }

        var id = int.Parse(agentId);

        var propertyIds = await _dbContext.Properties
        .Where(p => p.AgentId == id)
        .Select(p => p.Id)
        .ToListAsync();

        var agentInquiries = await _dbContext.Inquiries
        .Where(i=>propertyIds.Contains(i.PropertyId))
        .Select(i => new
        {
            InquiryId = i.Id,
            PropId = i.PropertyId,
            PropertyTitle =i.Property.Title,
            PropertyLocation = i.Property.Location,
            PropertPrice = i.Property.Price,

            InquiringUser = new
            {
                i.User.Id,
                i.User.FullName,
                i.User.Email,
                i.User.Phone
            },
            i.Message,
            i.CreatedAt

        }).ToListAsync();

    
        if (!agentInquiries.Any())
        {
            return NotFound(new{
                 message = "No inquiries found"
            });
        }
        return Ok(agentInquiries);
      
    }
  
    [Authorize]    
    [HttpPost]
    public async Task<IActionResult> CreateNewInquiry([FromBody] CreateInquiry request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {     
            return Unauthorized(new { message = "Invalid token - missing user id" });
        }

        var userId = int.Parse(userIdClaim);
    

        var existingInquiry = _dbContext.Inquiries.FirstOrDefault(u => u.PropertyId == request.PropertyId);
        if(existingInquiry != null)
        {
            return BadRequest(new {message = "You have already sent an inquiry for this property"});
        }
        var inquiry = new Inquiry
        {
            UserId = userId,
            PropertyId = request.PropertyId,
            Message = request.Message,
        };
        try
        {
            _dbContext.Inquiries.Add(inquiry);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Failed to create inquiry" });
        }
        
        return Ok(new { message = "Inquiry sent successfully" });
    }
    
}