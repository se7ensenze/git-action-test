using System.Data;
using System.Web;
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

    [HttpGet("echo")]
    public async Task<IActionResult> GetEcho([FromQuery]string personName,
        [FromServices]IHttpClientFactory httpClientFactory)
    {
        var httpClient = httpClientFactory.CreateClient("EchoApi");

        var responseText = await httpClient.GetStringAsync("/echo?personName=" + HttpUtility.UrlEncode(personName));

        return Ok(responseText);
    }

}

public class PersonDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime Updated { get; set; }
    public DateTime Created { get; set; }
}