using System.Collections;
using System.Windows.Controls;
using DynamicGrid.Control.Behaviors;
using FluentAssertions;

namespace DynamicGrid.Tests.Behaviors;

public class GridColumnBehaviorTests
{
    [StaTheory]
    [ClassData(typeof(GridColumnCountTestData))]
    public void Count_ShouldChangeTheAmountOfColumnsInGrid_WhenValueChanges(int startColumns, int changeTo, int expectedResult)
    {
        //Arrange
        var grid = new Grid();
        var columnBehavior = new GridColumnBehavior() { Count = startColumns };
        columnBehavior.Attach(grid);
        grid.ColumnDefinitions.Count.Should().Be(startColumns);
        
        //Act
        columnBehavior.Count = changeTo;

        //Assert
        grid.ColumnDefinitions.Count.Should().Be(expectedResult);
    }
}

class GridColumnCountTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 2, 5, 5 };
        yield return new object[] { 5, 2, 2 };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}