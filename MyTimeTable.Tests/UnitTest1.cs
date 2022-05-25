using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTimeTable.Controllers;
using MyTimeTable.ModelsDTO;
using Xunit;

namespace MyTimeTable.Tests;

public class UnitTest1
{
    private static MyTimeTableContext CreateContext()
    {
        // створюємо підроброблений (мок) контекст для передавання в контроллер
        var options = new DbContextOptionsBuilder<MyTimeTableContext>().UseInMemoryDatabase(databaseName: "MyTimeTable")
            .Options;
        MyTimeTableContext abc = new MyTimeTableContext(options);
        return abc;

    } 
    
    [Fact]
    public async Task CreateOrganization()
    {
        //Створює організацію
        //ARRANGE
        using var context = CreateContext();
        OrganizationsController organizationsController = new OrganizationsController(context);
        
        //ACT
        OrganizationDtoWrite organization = new OrganizationDtoWrite()
        {
            Name = "КНЛУ"
        };
        var result  = await organizationsController.PostOrganization(organization);
        
        //ASSERT
        Assert.True(result.Result is RedirectToActionResult);
    }
    [Fact]
    public async Task OrganizationValidation()
    {
        // перевіряє, чи буде помилка валідації, якщо передамо неправильний об'єкт до POST методу
        //ARRANGE
        using var context = CreateContext();
        OrganizationsController organizationsController = new OrganizationsController(context);
        
        //ACT
        OrganizationDtoWrite organization = new OrganizationDtoWrite();
        
        //ASSERT
        await Assert.ThrowsAnyAsync<Exception>(async () => await organizationsController.PostOrganization(organization));
    }
}