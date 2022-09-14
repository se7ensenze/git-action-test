using System.Data;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{

    [HttpGet()]
    public async Task<IActionResult> GetPersons([FromServices]IDbConnection connection)
    {
        var result = await connection.QueryAsync<PersonDto>(
            sql: "SELECT userid, fullname, updated, created FROM persons");

        return Ok(new
        {
            Persons = result.ToArray() 
        });
    }
    
}

public class PersonDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime Updated { get; set; }
    public DateTime Created { get; set; }
}