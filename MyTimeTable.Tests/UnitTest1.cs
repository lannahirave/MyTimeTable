using System;
using System.Collections.Generic;
using System.Net;
using Xunit;
using MyTimeTable;
using Moq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;
using MyTimeTable.Models;
using MyTimeTable.ModelsDTO;

namespace MyTimeTable.Tests
{
    public class LectorsControllerTests
    {
        static HttpClient client = new HttpClient();
        static LectorDtoRead? lector;

        public LectorsControllerTests()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        static async Task<HttpStatusCode> CreateLectorAsync(LectorsDtoWrite lectorDtoWrite)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "https://localhost:7140/api/Lectors", lectorDtoWrite);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.StatusCode;
        }
        static async Task<HttpStatusCode> GetLectorsAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7140/api/Lectors");
            response.EnsureSuccessStatusCode();
            // return URI of the created resource.
            return response.StatusCode;
        }
        
        
        static async Task<HttpStatusCode> DeleteLectorAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"https://localhost:7140/api/Lectors/{id}");
            return response.StatusCode;
        }

        [Fact]
        static void Test1()
        {

            // Create a new product
            LectorsDtoWrite lectorDtoWrite = new LectorsDtoWrite()
            {
                FullName = "Gizmo",
                Phone = 500348388,
                Degree = "Widgets",
                OrganizationsIds = new List<int>(){1}
            };

            var statusCode = CreateLectorAsync(lectorDtoWrite).Result;
            
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        
        [Fact]
        static void Test2()
        {
            // CHECK IF LECTORS GET METHOD WORKS
            var statusCode = GetLectorsAsync().Result;
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
    }
}