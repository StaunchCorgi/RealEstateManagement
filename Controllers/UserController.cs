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
public class UserController : ControllerBase
{
    public readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public UserController(AppDbContext dbContext,IConfiguration config)
    {
        _dbContext = dbContext;   
        _configuration = config;
    }
     [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var units = await _dbContext.Users.ToListAsync();   

        if (!units.Any())
        {
            return NotFound(new { message = "No users found" });
        }

        return Ok(units);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _dbContext.Users
                                    .FirstOrDefaultAsync(p => p.Id == id);

        if (user == null)
        {
            return NotFound(new { message = "Unit not found" });
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] Login login)
    {
        if(login == null || string.IsNullOrEmpty(login.Email)||string.IsNullOrEmpty(login.Password))
        {
             return BadRequest(new { message = "Email or Password are required" });
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Email == login.Email);
        if(user == null)
        {
            return Unauthorized(new { message = "Invalid Email or Password" });
        }
         bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password,user.PasswordHash);

        if (!isPasswordValid)
        {
            return Unauthorized(new { message = "Invalid Email or Password" });            
        }
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var jwtKey = _configuration["Jwt:Key"];
        if(jwtKey == null)
        {
            return BadRequest();
        }
         
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var expiryMinutes = Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );
       var jwt = new JwtSecurityTokenHandler().WriteToken(token);


        return Ok(new
        {
            message = "Login successful",
            token = jwt,
            expires = token.ValidTo,
            user = new
           {
                user.Id,
                user.FullName,
                user.Email,
                user.Role
           }
    });
    }    

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
       
        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new {message = "Password cannot be empty"});
        }
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = hashedPassword,
            Role = Role.Buyer
        };
        try
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(new 
            { message =ex.InnerException?.Message ?? ex.Message});
        }
        
        return Ok(new { message = "User created successfully" });
    }

    [Authorize]
    [HttpPost("request-agent-access")]
    public async Task<IActionResult> RequestAgentAccess()
    {
       // var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
         var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
          if (string.IsNullOrEmpty(userIdClaim))
          {     
            return Unauthorized(new { message = "Invalid token - missing user id" });
          }

        var userId = int.Parse(userIdClaim);


        var user = await _dbContext.Users.FindAsync(userId);
        if(user == null)
        {
            return NotFound();
        }
        if (user.AgentRequestPending)
        {
            return BadRequest(new
            {
                message = "Request already pending"
            });
        }
        if(user.Role == Role.Agent)
        {
            return BadRequest(new
            {
                message = "Already Registered as Agent"
            });
        }

        user.AgentRequestPending = true;

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Agent request submitted"
        });
      
    }

}