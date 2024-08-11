using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Arthur.Components.Pages;
using Arthur.Models;
using Arthur.Service;

namespace Arthur.Test;

public class HomePage : TestContext
{
    [Fact]
    public void ComponentRendersCorrectly()
    {
        // Arrange
        var drawService = Substitute.For<IDrawService>();
        drawService.GetDraws().Returns(new List<Draw>());

        Services.AddSingleton(drawService);

        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var title = cut.Find("h1");
        Assert.NotNull(title);
    }

    [Fact]
    public void TableRendersWhenDrawsResponseIsNotNull()
    {
        // Arrange
        var drawService = Substitute.For<IDrawService>();
        drawService.GetDraws().Returns(new List<Draw>
        {
            new Draw { 
                DrawDate = DateOnly.FromDateTime(DateTime.Now), 
                Number1 = "1", 
                Number2 = "2", 
                Number3 = "3", 
                Number4 = "4", 
                Number5 = "5",
                Number6 = "6", 
                Id = "draw-1" 
            }
        });

        Services.AddSingleton(drawService);

        // Act
        var cut = RenderComponent<Home>();

        // Assert
        Assert.NotNull(cut.Find("table"));
    }

    [Fact]
    public void TableContainsCorrectValuesWhenDrawsResponseIsNotNull()
    {
        // Arrange
        var drawService = Substitute.For<IDrawService>();
        var draws = new List<Draw>
        {
            new Draw
            {
                DrawDate = DateOnly.FromDateTime(DateTime.Now),
                Number1 = "1",
                Number2 = "2",
                Number3 = "3",
                Number4 = "4",
                Number5 = "5",
                Number6 = "6",
                Id = "draw-1"
            }
        };

        drawService.GetDraws().Returns(draws);

        Services.AddSingleton(drawService);

        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var row = cut.Find("tbody tr");
        Assert.Contains(draws[0].DrawDate.ToString(), row.InnerHtml);
        Assert.Contains(draws[0].Number1, row.InnerHtml);
        Assert.Contains(draws[0].Number2, row.InnerHtml);
        Assert.Contains(draws[0].Number3, row.InnerHtml);
        Assert.Contains(draws[0].Number4, row.InnerHtml);
        Assert.Contains(draws[0].Number5, row.InnerHtml);
        Assert.Contains(draws[0].Number6, row.InnerHtml);
    }
}