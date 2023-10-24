using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DynamicGrid.Control.Models;

namespace DynamicGrid.Control.Helpers;

internal static class DynamicConfigureGridHelper
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
            UpdateGridColumns(grid, targetAmount, targetAmount,  startRows, borderStyle, information);
        }
        else
        {
            UpdateGridRows(grid, startRows, targetAmount, startColumns, borderStyle, information);
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
            for (int columnIndex = 0; columnIndex < targetAmount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < startRows; rowIndex++)
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
            for (int rowIndex = 0; rowIndex < targetAmount; rowIndex++)
            {
                for (int colIndex = 0; colIndex < startColumns; colIndex++)
                {
                    var border = grid
                        .Children
                        .Cast<FrameworkElement>()
                        .FirstOrDefault(fe =>
                            Grid.GetColumn(fe) == colIndex + 1 &&
                            Grid.GetRow(fe) == rowIndex + 1 &&
                            fe.GetType() == typeof(Border));
                    
                    grid.Children.Remove(border);
                }
            }
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

    private static void UpdateGridRows(Canvas grid, int amount, int targetAmount, int startRows, Style borderStyle,
        GridLocationInformation gridLocationInformation)
    {
        var isAdd = targetAmount > startRows;
        if (isAdd)
        {
            
        }
        else
        {
            
        }
    }
}