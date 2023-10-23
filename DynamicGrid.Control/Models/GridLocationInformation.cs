namespace DynamicGrid.Control.Models;

internal record GridLocationInformation(
    double ObjectWidth,
    double ObjectHeight,
    double GridGap,
    double AddButtonWidth,
    int Rows,
    int Columns
    );