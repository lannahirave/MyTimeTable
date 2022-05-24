using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyTimeTable.ModelsDTO;
using Xunit;

namespace MyTimeTable.Tests;

public class LectorsControllerTests
{
    private static readonly HttpClient Client = new();
    
    public LectorsControllerTests()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static async Task<Task<string>> CreateLectorAsync(LectorsDtoWrite lectorDtoWrite)
    {
        var response = await Client.PostAsJsonAsync(
            "https://localhost:7140/api/Lectors/", lectorDtoWrite);
        Console.WriteLine(response.ToString());
        response.EnsureSuccessStatusCode();

        // return URI of the created resource.
        return response.Content.ReadAsStringAsync();
    }

    private static async Task<Task<string>> GetLectorsAsync()
    {
        var response = await Client.GetAsync("https://localhost:7140/api/Lectors");
        response.EnsureSuccessStatusCode();
        // return URI of the created resource.
        return response.Content.ReadAsStringAsync();
    }
    

    [Fact]
    private static void Test1()
    {
        // Create a new product
        var lectorDtoWrite = new LectorsDtoWrite
        {
            FullName = "Gizmo",
            Phone = 500348388,
            Degree = "Widgets",
            OrganizationsIds = new List<int> {1}
        };

        var result = CreateLectorAsync(lectorDtoWrite).Result.Result;

        Assert.True(result.StartsWith("{") && result.EndsWith("}"));
    }

    [Fact]
    private static void Test2()
    {
        // CHECK IF LECTORS GET METHOD WORKS
        var result = GetLectorsAsync().Result.Result;
        Assert.True(result.StartsWith("[") && result.EndsWith("]"));
    }
}