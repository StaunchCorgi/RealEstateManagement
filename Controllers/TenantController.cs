// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using RealEstateManagementAPI.Models;


// [ApiController]
// [Route("api/[controller]")]
// public class TenantController : ControllerBase
// {
//     public readonly AppDbContext _dbContext;

//     public TenantController(AppDbContext dbContext)
//     {
//         _dbContext = dbContext;   
//     }
//      [HttpGet]
//     public async Task<IActionResult> GetTenants()
//     {
//         var tenants = await _dbContext.Tenants.ToListAsync();
                                  

//         if (!tenants.Any())
//         {
//             return NotFound(new { message = "No tenants found" });
//         }

//         return Ok(tenants);
//     }

//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetTenant(int id)
//     {
//         var tenant = await _dbContext.Tenants
//                                     .FirstOrDefaultAsync(p => p.Id == id);

//         if (tenant == null)
//         {
//             return NotFound(new { message = "Tenant not found" });
//         }

//         return Ok(tenant);
//     }
        
//     [HttpPost("AddTentant")]
//     public async Task<IActionResult> CreateNewTenant([FromBody] Tenant request)
//     {
//         var existingTenant = _dbContext.Tenants.FirstOrDefault(a => a.Email == request.Email);
//         if(existingTenant != null)
//         {
//             return BadRequest(new {message = "Property already exists"});
//         }
//         var tenant = new Tenant
//         {
//             FirstName = request.FirstName,
//             LastName = request.LastName,
//             Email = request.Email,
//             Phone = request.Phone,
//         };
//         try
//         {
//             _dbContext.Tenants.Add(tenant);
//             await _dbContext.SaveChangesAsync();
//         }
//         catch (Exception)
//         {
//             return BadRequest(new { message = "Failed to add tenant" });
//         }
        
//         return Ok(new { message = "Tenant added successfully" });
//     }
     
//     /*[HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody]LoginDTO login)
//     {
//         var user = _dbContext.RegisterUsers.FirstOrDefault(u => u.Username == login.username);
//         if(user == null)
//         {
//             return Unauthorized(new {message = "Invalid username or password."});
//         }
        
//         /* var passwordMatches = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

//     if (!passwordMatches)
//     {
//         return Unauthorized(new { message = "Invalid username or password." });
//     }
//         return Ok(new{messge = "Login Successful"});
//     }*/
    
// }