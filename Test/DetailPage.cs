using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Arthur.Components.Pages;
using Arthur.Models;
using Arthur.Service;
using AngleSharp.Dom;

namespace Arthur.Test;
public class DetailPage : TestContext
{
    [Fact]
    public void ComponentHandlesNullDraw()
    {
        // Arrange
        var testId = "draw-null";
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(null as Draw);
        Services.AddSingleton(mockDrawService);

        Services.AddSingleton<IRandomNumberService>(new RandomNumberService());


        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));

        // Assert
        var noDrawMessage = component.Find("em").Text();
        Assert.NotNull(noDrawMessage);
        Assert.True(noDrawMessage == $"No draw with id \"{testId}\"");
    }

    [Fact]
    public void ComponentDisplaysDrawDetails()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);

        Services.AddSingleton<IRandomNumberService>(new RandomNumberService());

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));

        // Assert
        var drawName = component.Find("h1").Text();
        Assert.NotNull(drawName);
        Assert.True(drawName == $"Draw detail for Saturday 10 August") ;
    }

    [Fact]
    public void ComponentDisplaysButtonWhenDrawDetails()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);

        Services.AddSingleton<IRandomNumberService>(new RandomNumberService());

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));

        // Assert
        var cut = component.Find("button");
        Assert.NotNull(cut);
    }


    [Fact]
    public void ClickingButtonAssignsNumbersToMyNumbers()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);

        Services.AddSingleton<IRandomNumberService>(new RandomNumberService());

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));
        component.Find("button").Click(); 

        // Assert
        var myNumbers = component.Instance.myNumbers; 
        Assert.NotNull(myNumbers); 
        Assert.NotEmpty(myNumbers); 
    }


    [Fact]
    public void CheckIfMyNumberLoser()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);
        var mockRandomNumService = Substitute.For<IRandomNumberService>();
        var expectedFailNumbers = new List<int> { 17, 11, 12, 13, 14, 15, 16 };
        mockRandomNumService.PickNumbers().Returns(expectedFailNumbers);
        Services.AddSingleton(mockRandomNumService);

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));
        component.Find("button").Click(); 

        // Assert
        Assert.False(component.Instance.isWinner);  
    }

    [Fact]
    public void CheckIfRenderLosingText()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);
        var mockRandomNumService = Substitute.For<IRandomNumberService>();
        var expectedFailNumbers = new List<int> { 17, 11, 12, 13, 14, 15, 16 };
        mockRandomNumService.PickNumbers().Returns(expectedFailNumbers);
        Services.AddSingleton(mockRandomNumService);

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));
        component.Find("button").Click();
        var losingText = component.Find("[data-id-loss]").Text();

        // Assert
        Assert.NotNull(losingText);
        Assert.True(losingText == "Sorry. Better luck next time");
    }

    [Fact]
    public void CheckIfMyNumberWinner()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);
        var mockRandomNumService = Substitute.For<IRandomNumberService>();
        var expectedFailNumbers = new List<int> { 7, 1, 2, 3, 4, 5, 6 };
        mockRandomNumService.PickNumbers().Returns(expectedFailNumbers);
        Services.AddSingleton(mockRandomNumService);

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));
        component.Find("button").Click();

        // Assert
        Assert.True(component.Instance.isWinner);
    }

    [Fact]
    public void CheckIfRenderWinText()
    {
        // Arrange
        var testId = "draw-success";
        var draw = new Draw
        {
            DrawDate = DateOnly.Parse("2024-08-10"),
            Number1 = "1",
            Number2 = "2",
            Number3 = "3",
            Number4 = "4",
            Number5 = "5",
            Number6 = "6",
            BonusBall = "7",
            Id = "draw-1"
        };
        var mockDrawService = Substitute.For<IDrawService>();
        mockDrawService.GetDrawById(testId).Returns(draw);
        Services.AddSingleton(mockDrawService);
        var mockRandomNumService = Substitute.For<IRandomNumberService>();
        var expectedFailNumbers = new List<int> { 7, 1, 2, 3, 4, 5, 6 };
        mockRandomNumService.PickNumbers().Returns(expectedFailNumbers);
        Services.AddSingleton(mockRandomNumService);

        // Act
        var component = RenderComponent<Detail>(parameters => parameters.Add(p => p.Id, testId));
        component.Find("button").Click();
        var winText = component.Find("[data-id-won]").Text();

        // Assert
        Assert.NotNull(winText);
        Assert.True(winText == "Congratulations, you are a winner!");
    }
}
