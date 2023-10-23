using System;
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
                var border = new Border() {Style = borderStyle};
                border.Width = objWidth;
                border.Height = objHeight;
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
        int targetAmount)
    {
        var startRows = DynamicGridControl.GetRows(grid);
        var startColumns = DynamicGridControl.GetColumns(grid);

        var isAdd = targetAmount > startRows;

        if (isAdd)
        {
            
        }
        else
        {
            
        }
        
    }
}