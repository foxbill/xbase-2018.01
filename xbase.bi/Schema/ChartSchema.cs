using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using xbase;

namespace xbase.bi.schema
{
    public static class ChartConst
    {
        public const string Default = "Default";
    }

    public enum ChartType
    {
        Point = 0,
        //
        // 摘要:
        //     FastPoint chart type.
        FastPoint = 1,
        //
        // 摘要:
        //     Bubble chart type.
        Bubble = 2,
        //
        // 摘要:
        //     Line chart type.
        Line = 3,
        //
        // 摘要:
        //     Spline chart type.
        Spline = 4,
        //
        // 摘要:
        //     StepLine chart type.
        StepLine = 5,
        //
        // 摘要:
        //     FastLine chart type.
        FastLine = 6,
        //
        // 摘要:
        //     Bar chart type.
        Bar = 7,
        //
        // 摘要:
        //     Stacked bar chart type.
        StackedBar = 8,
        //
        // 摘要:
        //     Hundred-percent stacked bar chart type.
        StackedBar100 = 9,
        //
        // 摘要:
        //     DataListColumn chart type.
        Column = 10,
        //
        // 摘要:
        //     Stacked column chart type.
        StackedColumn = 11,
        //
        // 摘要:
        //     Hundred-percent stacked column chart type.
        StackedColumn100 = 12,
        //
        // 摘要:
        //     Area chart type.
        Area = 13,
        //
        // 摘要:
        //     Spline area chart type.
        SplineArea = 14,
        //
        // 摘要:
        //     Stacked area chart type.
        StackedArea = 15,
        //
        // 摘要:
        //     Hundred-percent stacked area chart type.
        StackedArea100 = 16,
        //
        // 摘要:
        //     Pie chart type.
        Pie = 17,
        //
        // 摘要:
        //     Doughnut chart type.
        Doughnut = 18,
        //
        // 摘要:
        //     Stock chart type.
        Stock = 19,
        //
        // 摘要:
        //     Candlestick chart type.
        Candlestick = 20,
        //
        // 摘要:
        //     Range chart type.
        Range = 21,
        //
        // 摘要:
        //     Spline range chart type.
        SplineRange = 22,
        //
        // 摘要:
        //     RangeBar chart type.
        RangeBar = 23,
        //
        // 摘要:
        //     Range column chart type.
        RangeColumn = 24,
        //
        // 摘要:
        //     Radar chart type.
        Radar = 25,
        //
        // 摘要:
        //     Polar chart type.
        Polar = 26,
        //
        // 摘要:
        //     Error bar chart type.
        ErrorBar = 27,
        //
        // 摘要:
        //     Box plot chart type.
        BoxPlot = 28,
        //
        // 摘要:
        //     Renko chart type.
        Renko = 29,
        //
        // 摘要:
        //     ThreeLineBreak chart type.
        ThreeLineBreak = 30,
        //
        // 摘要:
        //     Kagi chart type.
        Kagi = 31,
        //
        // 摘要:
        //     PointAndFigure chart type.
        PointAndFigure = 32,
        //
        // 摘要:
        //     Funnel chart type.
        Funnel = 33,
        //
        // 摘要:
        //     Pyramid chart type.
        Pyramid = 34,
    }

    public enum MarkerStyle
    {
        // 摘要:
        //     No marker is displayed for the series or data point.
        None = 0,
        //
        // 摘要:
        //     A square marker is displayed.
        Square = 1,
        //
        // 摘要:
        //     A circular marker is displayed.
        Circle = 2,
        //
        // 摘要:
        //     A diamond-shaped marker is displayed.
        Diamond = 3,
        //
        // 摘要:
        //     A triangular marker is displayed.
        Triangle = 4,
        //
        // 摘要:
        //     A cross-shaped marker is displayed.
        Cross = 5,
        //
        // 摘要:
        //     A 4-point star-shaped marker is displayed.
        Star4 = 6,
        //
        // 摘要:
        //     A 5-point star-shaped marker is displayed.
        Star5 = 7,
        //
        // 摘要:
        //     A 6-point star-shaped marker is displayed.
        Star6 = 8,
        //
        // 摘要:
        //     A 10-point star-shaped marker is displayed.
        Star10 = 9,
    }

    public class AxisSchema : xbase.Schema
    {
        private string field;
    }

    public class ChartArea : xbase.Schema
    {
        private ChartType chartType;
        private AxisSchema axisX;
        private bool is3D;
        private float minY;
        private float maxY;
        private string backColor;

        public string BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public float MinY
        {
            get { return minY; }
            set { minY = value; }
        }

        public float MaxY
        {
            get { return maxY; }
            set { maxY = value; }
        }


        public ChartType ChartType
        {
            get { return chartType; }
            set { chartType = value; }
        }
        public AxisSchema AxisX
        {
            get { return axisX; }
            set { axisX = value; }
        }

        public bool Is3D
        {
            get { return is3D; }
            set { is3D = value; }
        }
    }

    public class SeriesSchema : xbase.Schema
    {
        private string field;
        private int lineWidth;
        private ChartType chartType;
        private string color;
        private MarkerStyle markerStyle;
        private string area;
        private string upSeries;
        private string downSeries;
        private bool createAxisY;
        private bool usingY2;
        private bool isFromY0;

        public bool IsFromY0
        {
            get { return isFromY0; }
            set { isFromY0 = value; }
        }

        public bool UsingY2
        {
            get { return usingY2; }
            set { usingY2 = value; }
        }

        public bool CreateAxisY
        {
            get { return createAxisY; }
            set { createAxisY = value; }
        }

        public string UpSeries
        {
            get { return upSeries; }
            set { upSeries = value; }
        }

        public string DownSeries
        {
            get { return downSeries; }
            set { downSeries = value; }
        }

        public string Area
        {
            get { return area; }
            set { area = value; }
        }


        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }
        public ChartType ChartType
        {
            get { return chartType; }
            set { chartType = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public MarkerStyle MarkerStyle
        {
            get { return markerStyle; }
            set { markerStyle = value; }
        }
    }

    public class ChartSchema : xbase.Schema
    {
        private string dataSource;
        private SchemaList<NamedValueSchema> searchParams = new SchemaList<NamedValueSchema>();
        private SchemaList<ChartArea> areaList = new xbase.SchemaList<ChartArea>();
        private string text;
        private bool is3D;
        private string backColor;
        private xbase.SchemaList<SeriesSchema> seriesList = new xbase.SchemaList<SeriesSchema>();
        private string xField;

        //new
        private int width;
        private int height;
        private bool showGridVLine;
        private string gridVLineColor;
        private int gridVLineWidth;
        private bool showGridHLine;
        private string gridHLineColor;
        private int gridHLineWidth;
        private bool legenH;
        private string legenPos;
        private string legenAlign;
        private string titlePos;
        private bool xYReverse;
        private string xIntervalUnit;
        private int xInterval;
        private string xLableFormat;
        private int pageSize;
        private string dataAdminPagePath;

        public string DataAdminPagePath
        {
            get { return dataAdminPagePath; }
            set { dataAdminPagePath = value; }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public string XIntervalUnit
        {
            get { return xIntervalUnit; }
            set { xIntervalUnit = value; }
        }

        public int XInterval
        {
            get { return xInterval; }
            set { xInterval = value; }
        }

        public string XLableFormat
        {
            get { return xLableFormat; }
            set { xLableFormat = value; }
        }


        public bool XYReverse
        {
            get { return xYReverse; }
            set { xYReverse = value; }
        }

        public string TitlePos
        {
            get { return titlePos; }
            set { titlePos = value; }
        }

        public int Width
        {
            get
            {
                if (width > 710)
                    width = 710;
                if (width < 200)
                    width = 200;
                return width;
            }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public bool ShowGridVLine
        {
            get { return showGridVLine; }
            set { showGridVLine = value; }
        }

        public string GridVLineColor
        {
            get { return gridVLineColor; }
            set { gridVLineColor = value; }
        }

        public int GridVLineWidth
        {
            get { return gridVLineWidth; }
            set { gridVLineWidth = value; }
        }

        public bool ShowGridHLine
        {
            get { return showGridHLine; }
            set { showGridHLine = value; }
        }

        public string GridHLineColor
        {
            get { return gridHLineColor; }
            set { gridHLineColor = value; }
        }

        public int GridHLineWidth
        {
            get { return gridHLineWidth; }
            set { gridHLineWidth = value; }
        }

        public bool LegenH
        {
            get { return legenH; }
            set { legenH = value; }
        }

        public string LegenPos
        {
            get { return legenPos; }
            set { legenPos = value; }
        }

        public string LegenAlign
        {
            get { return legenAlign; }
            set { legenAlign = value; }
        }

        public string XField
        {
            get { return xField; }
            set { xField = value; }
        }

        public xbase.SchemaList<SeriesSchema> SeriesList
        {
            get { return seriesList; }
            set { seriesList = value; }
        }


        public string BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public bool Is3D
        {
            get { return is3D; }
            set { is3D = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public SchemaList<NamedValueSchema> SearchParams
        {
            get { return searchParams; }
            set { searchParams = value; }
        }

        public xbase.SchemaList<ChartArea> AreaList
        {
            get
            {
                return areaList;
            }

            set { areaList = value; }
        }
    }

    public class ChartShemaContainer : xbase.SchemaContainer<ChartSchema>
    {

    }

}
