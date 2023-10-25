using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using DynamicGrid.Control.Models;

namespace DynamicGrid.Control.Helpers;

internal static class RowColumnManager
{
    public static GridLocationInformation CalculateLocationInformation(this Canvas grid,
        double buttonWidth, double gap, int columns, int rows)
    {
        var availbleWidth = grid.ActualWidth - (buttonWidth * 2) - gap;
        var availbleHeight = grid.ActualHeight - (buttonWidth * 2) - gap;
        var gridSquareWidth = (availbleWidth- (columns * gap))/ columns;
        var gridSquareHeight = (availbleHeight - (rows * gap) )/ rows;
        return new (gridSquareWidth, gridSquareHeight, gap, buttonWidth, rows, columns);
    }

    public static void InitDynamicGrid(this Canvas grid,
        GridLocationInformation information,
        Style borderStyle)
    {
        var objWidth = information.ObjectWidth;
        var objHeight = information.ObjectHeight;
        var heightLocation = information.GridGap + information.AddButtonWidth;
        var widthLocation = information.GridGap + information.AddButtonWidth;
        for (var row = 0; row < information.Rows; row++)
        {
            for (var column = 0; column < information.Columns; column++)
            {
                var border = CreateBorder(information, borderStyle, row + 1, column + 1);
                Canvas.SetLeft(border, widthLocation);
                Canvas.SetTop(border, heightLocation);

                grid.Children.Add(border);

                widthLocation += objWidth + information.GridGap;
            }

            heightLocation += objHeight + information.GridGap;
            widthLocation = information.GridGap + information.AddButtonWidth;
        } 
        DynamicGridControl.SetRows(grid, information.Rows);
        DynamicGridControl.SetColumns(grid, information.Columns);
    }

    public static void UpdateGridObjectCount(
        this Canvas grid,
        GridObjectType objectType,
        int targetAmount,
        Style borderStyle,
        GridLocationInformation information)
    {
        var startRows = DynamicGridControl.GetRows(grid);
        var startColumns = DynamicGridControl.GetColumns(grid);

        if (objectType == GridObjectType.Column)
        {
            UpdateGridColumns(grid, startColumns, targetAmount,  startRows, borderStyle, information);
            DynamicGridControl.SetColumns(grid, targetAmount);
        }
        else
        {
            UpdateGridRows(grid, startRows, targetAmount, startColumns, borderStyle, information);
            DynamicGridControl.SetRows(grid, targetAmount);
        }
    }

    private static void UpdateGridColumns(Canvas grid, int startColumns, int targetAmount, int startRows,
        Style borderStyle, GridLocationInformation information)
    {
        double xPosition = 0;
        double yPosition = 0;
        var isAdd = targetAmount > startColumns;
        if (isAdd)
        {
            yPosition = information.GridGap + information.AddButtonWidth;
            xPosition = (information.Columns * information.ObjectWidth) + information.AddButtonWidth;
            for (int columnIndex = startColumns; columnIndex < targetAmount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < startRows; rowIndex++)
                {
                    var border = CreateBorder(information, borderStyle, rowIndex+1, columnIndex+1);
                    Canvas.SetLeft(border, xPosition);
                    Canvas.SetTop(border, yPosition);
                    yPosition += information.ObjectHeight + information.GridGap;
                    grid.Children.Add(border);
                }
                xPosition += information.ObjectWidth + information.GridGap;
            }
        }
        else
        {
            if (targetAmount == startColumns)
            {
                return;
            }
            var bordersToRemove = grid
                .ConfigureGridBorders()
                .Where(border => Grid.GetColumn(border) == startColumns)
                .ToArray();
            foreach (var b in bordersToRemove)
            {
                grid.Children.Remove(b);
            }
        }
        grid.UpdateAllBordersToCorrectLocation(information);
    }

    public static Border? GetConfigureBorderByColAndRow(this Canvas grid,int rowIndex, int colIndex)
    {
        return grid
            .ConfigureGridBorders()
            .FirstOrDefault(fe =>
                Grid.GetColumn(fe) == colIndex  &&
                Grid.GetRow(fe) == rowIndex);
    }
    public static IEnumerable<Border> ConfigureGridBorders(this Canvas grid)
    {
        return grid
            .Children
            .Cast<FrameworkElement>()
            .Where(fe => fe.GetType() == typeof(Border))
            .Cast<Border>()
            .ToArray();
    }
    private static void UpdateAllBordersToCorrectLocation(this Canvas grid, GridLocationInformation information)
    {
        var objWidth = information.ObjectWidth;
        var objHeight = information.ObjectHeight;
        var heightLocation = information.GridGap + information.AddButtonWidth;
        var widthLocation = information.GridGap + information.AddButtonWidth;
        
        for (int row = 0; row < information.Rows; row++)
        {
            for (int column = 0; column < information.Columns; column++)
            {
                var border = grid.GetConfigureBorderByColAndRow(row+1, column+1 );
                if (border is null) continue;

                border.Width = information.ObjectWidth;
                border.Height = information.ObjectHeight;
                var currentLeft = Canvas.GetLeft(border);
                var currentTop = Canvas.GetTop(border);
                var leftAnimation = new DoubleAnimation()
                {
                    From = currentLeft,
                    To = widthLocation,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300))
                };
                var topAnimation = new DoubleAnimation()
                {
                    From = currentTop,
                    To = heightLocation,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300))
                };
                Storyboard.SetTarget(leftAnimation, border);
                Storyboard.SetTarget(topAnimation, border);
                Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(Canvas.Left)"));
                Storyboard.SetTargetProperty(topAnimation, new PropertyPath("(Canvas.Top)"));
                var leftStoryboard = new Storyboard();
                var topStoryboard = new Storyboard();
                leftStoryboard.Children.Add(leftAnimation);
                topStoryboard.Children.Add(topAnimation);
                leftStoryboard.Begin();
                topStoryboard.Begin();
                widthLocation += objWidth + information.GridGap;
            }
            
            heightLocation += objHeight + information.GridGap;
            widthLocation = information.GridGap + information.AddButtonWidth;
        }
    }

    private static Border CreateBorder(
        GridLocationInformation information, 
        Style borderStyle,
        int row,
        int column)
    {
        var border = new Border()
        {
            Width = information.ObjectWidth,
            Height = information.ObjectHeight,
            Style = borderStyle
        };
        
        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        return border;
    }

    private static void UpdateGridRows(Canvas grid, int startRows, int targetAmount, int startColumns, Style borderStyle,
        GridLocationInformation information)
    {
        double xPosition = 0;
        double yPosition = 0;
        var isAdd = targetAmount > startRows;
        if (isAdd)
        {
            yPosition = information.GridGap + information.AddButtonWidth;
            xPosition = (information.Columns * information.ObjectWidth) + information.AddButtonWidth;
            for (int rowIndex = startRows; rowIndex < targetAmount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < information.Columns; columnIndex++)
                {
                    var border = CreateBorder(information, borderStyle, rowIndex+1, columnIndex+1);
                    Canvas.SetLeft(border, xPosition);
                    Canvas.SetTop(border, yPosition);
                    xPosition += information.ObjectWidth + information.GridGap;
                    grid.Children.Add(border);
                }
                yPosition += information.ObjectHeight + information.GridGap;
            }
        }
        else
        {
            if (targetAmount == startRows)
            {
                return;
            }
            var bordersToRemove = grid
                .ConfigureGridBorders()
                .Where(border => Grid.GetRow(border) == startRows)
                .ToArray();
            foreach (var b in bordersToRemove)
            {
                grid.Children.Remove(b);
            }
        }
        grid.UpdateAllBordersToCorrectLocation(information);
    }
}