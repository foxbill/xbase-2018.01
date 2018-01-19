using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Text;
using System.IO;
using System.Web.UI;
using xbase.bi;
using xbase.bi.schema;
using xbase.data;
using System.Text.RegularExpressions;
using xbase.umc;
using xbase.security;
using xbase.umc.attributes;

namespace xbase.web.controls
{
    [WboAttr(Id = "DataChart", Title = "数据图表", IsPublish = true, LifeCycle = LifeCycle.Session, ContainerType = typeof(ChartShemaContainer))] 
    public class DataChart : IVisualWbo,ISessionWbo
    {
        public const int MutiYOffSetUnit = 8;

        private Chart msChart = new Chart();
        private XChart xChart;
        private ISession session;

        public DataChart(string chartId)
        {
            InitialMsChart();
            xChart = new XChart(chartId);
            SetChartData(xChart);
        }

        public Bitmap GetBitmap()
        {
            int w = (int)msChart.Width.Value;
            int h = (int)msChart.Height.Value;

            Bitmap ret = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(ret);
            msChart.ImageType = ChartImageType.Bmp;
            msChart.Paint(g, new Rectangle(0, 0, w, h));
            return ret;
        }

        private void InitialMsChart()
        {
            msChart.Visible = true;
            msChart.ImageType = ChartImageType.Png;
            //            msChart.ImageLocation = "/TempImages";
//            msChart.ImageLocation = "~/TempImages/ChartPic_#(300,3).png";//SEQ
    //        msChart.ImageLocation = "C:/ChartPic";//SEQ
           

            msChart.ImageStorageMode = ImageStorageMode.UseHttpHandler;
          
//            msChart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            msChart.RenderType = RenderType.ImageTag;
         
            msChart.Palette = ChartColorPalette.BrightPastel;

            //            chart.BorderDashStyle="Solid"


            msChart.BackGradientStyle = GradientStyle.LeftRight;// "VerticalCenter"
            msChart.BackGradientStyle = GradientStyle.VerticalCenter;
            msChart.BackColor = Color.FromArgb(0xD3, 0xDF, 0xF0);
            msChart.BackSecondaryColor = Color.White;

            msChart.BorderlineDashStyle = ChartDashStyle.NotSet;
            msChart.BorderWidth = 2;
            msChart.BorderlineColor = Color.Black;
            msChart.BorderColor = Color.FromArgb(26, 59, 105);

            msChart.BorderSkin.BackColor = Color.Gray;
            msChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            msChart.BorderSkin.BorderColor = Color.White;
            msChart.BorderSkin.PageColor = Color.White;
        }

        private void SetChartData_(XChart xChart)
        {
            Title title = msChart.Titles.Add("aaaa");
            title.Text = "测试图表";

            msChart.Series.Clear();

            Legend lg = msChart.Legends.FindByName("Default");
            if (lg == null)
            {
                lg = new Legend("Default");
                msChart.Legends.Add(lg);
            }

            try
            {
                lg.Docking = Docking.Top;
            }
            catch { }

            try
            {
                lg.Alignment = StringAlignment.Center;
            }
            catch { }

            lg.LegendStyle = LegendStyle.Column;

            lg.LegendStyle = LegendStyle.Row;



            //lg.Alignment = StringAlignment.Near;
            //lg.Position.X = 0;
            //lg.Position.Y = 0;
            lg.Title = "xxxxx";
            lg.BackColor = Color.Transparent;

            if (msChart.Titles.Count < 1)
                msChart.Titles.Add("Chart");

            msChart.Width = 300;
            msChart.Height = 300;

            System.Web.UI.DataVisualization.Charting.ChartArea area = msChart.ChartAreas.Add("Default");

            //            System.Web.UI.DataVisualization.Charting.ChartArea area = chart.ChartAreas[0];



            area.Area3DStyle.Enable3D = true;
            //  area.AxisY.IsLogarithmic = true;
            //  area.AxisX.IsLogarithmic = true;

            //            area.AxisX.Interval = 0;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.IsMarginVisible = false;
            area.BackColor = Color.Pink;
            // area.AxisX.

            //            area.AxisX.Interval = 1;
            Series a = msChart.Series.Add("a");
            a.Legend = "Default";
            a.ChartArea = "Default";
            a.Points.AddXY(1, 23);

            a.Points.AddXY(2, 84);
            a.Points.AddXY(3, 45);
            a.Points.AddXY(4, 245);
            a.Points.AddXY(5, 34);
            a.Points.AddXY(6, 34);
            a.Points.AddXY(7, 33);
            //            a.PostBackValue = "#AXISLABEL";
            a.PostBackValue = "xxxxxxx";
            a.ToolTip = "#LEGENDTEXT \n" +
                             "X轴：#VALX\n" +
                             "Y轴：#VAL";
        }

        [WboMethodAttr(PermissionTypes=PermissionTypes.DoAll)]
        public string Render(string elementName)
        {


            //  chart.BackHatchStyle = ChartHatchStyle.Horizontal;



            //    Dictionary<string, ListData<string>> keyValues = table.FieldValues;
            //    Dictionary<string, string> captions = table.FieldCaptions;

            //    chart.ChartAreas.Clear();
            //   chart.ChartAreas[0].
            //  chart.ChartAreas[0].Area3DStyle=ChartArea3DStyle.
            // chart.CurrentImageLocation = "";
            StringBuilder sb = new StringBuilder("");
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
         //   msChart.SaveImage("~/TempImages/ChartPic_#SEQ(300,3).jpg");
            msChart.RenderControl(htw);
            return sb.ToString();
          //  return msChart.CurrentImageLocation;
        }

        private void SetChartData(XChart xChart)
        {

            ChartSchema schema = xChart.Schema;

            DataSource table = null;

            if (!String.IsNullOrEmpty(schema.DataSource))
            {
                table = new DataSource(schema.DataSource);
            //    table.RefreshData();
            //    table.PageSize = 100;
            }

            //    Dictionary<string, ListData<string>> keyValues = table.FieldValues;
            //    Dictionary<string, string> captions = table.FieldCaptions;

            //    msChart.ChartAreas.Clear();
            //   msChart.ChartAreas[0].
            //  msChart.ChartAreas[0].Area3DStyle=ChartArea3DStyle.
            msChart.Series.Clear();

            Legend lg = msChart.Legends.FindByName("Default");
            if (lg == null)
            {
                lg = new Legend("Default");
                msChart.Legends.Add(lg);
            }

            try
            {
                lg.Docking = (Docking)int.Parse(schema.LegenPos);
            }
            catch { }

            try
            {
                lg.Alignment = (StringAlignment)int.Parse(schema.LegenAlign);
            }
            catch { }

            lg.LegendStyle = LegendStyle.Column;
            if (schema.LegenH)
                lg.LegendStyle = LegendStyle.Row;



            //lg.Alignment = StringAlignment.Near;
            //lg.Position.X = 0;
            //lg.Position.Y = 0;


            if (msChart.Titles.Count < 1)
                msChart.Titles.Add("Chart");
            Title title = msChart.Titles[0];
            title.Text = schema.Title;
            try
            {
                title.Alignment = (ContentAlignment)int.Parse(schema.TitlePos);
            }
            catch
            {
                title.Alignment = ContentAlignment.TopCenter;
            }

            //            if (schema.Width > 200)
            msChart.Width = schema.Width;
            //   if (schema.Width > 710)
            //       msChart.Width = 710;

            if (schema.Height > 100)
                msChart.Height = schema.Height;


            //            System.Web.UI.DataVisualization.Charting.ChartArea area = msChart.ChartAreas[0];
            msChart.ChartAreas.Clear();
            System.Web.UI.DataVisualization.Charting.ChartArea area = msChart.ChartAreas.Add("Default");

            if (!string.IsNullOrEmpty(schema.BackColor))
                msChart.BackColor = ToColor(schema.BackColor);

            if (!string.IsNullOrEmpty(xChart.AreaBackColor))
                area.BackColor = ToColor(xChart.AreaBackColor);


            area.Area3DStyle.Enable3D = schema.Is3D;
            //  area.AxisY.IsLogarithmic = true;
            //  area.AxisX.IsLogarithmic = true;

            //            area.AxisX.Interval = 0;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.IsMarginVisible = false;
            // area.AxisX.

            area.AxisX.Interval = schema.XInterval;

            try
            {
                area.AxisX.IntervalType = (DateTimeIntervalType)int.Parse(schema.XIntervalUnit);
            }
            catch
            {
                area.AxisX.IntervalType = DateTimeIntervalType.Auto;
            }

            area.AxisX.IsLabelAutoFit = true;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont | LabelAutoFitStyles.IncreaseFont |
                                           LabelAutoFitStyles.LabelsAngleStep90 | LabelAutoFitStyles.LabelsAngleStep45 |
                                           LabelAutoFitStyles.LabelsAngleStep30;

            area.AxisX.LabelStyle.Enabled = true;
            area.AxisX.LabelStyle.Format = schema.XLableFormat;

            //  lg.



            //            area.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;

            //            area.AxisX.IntervalOffsetType =DateTimeIntervalType.Minutes;

            if (schema.AreaList[0].MinY == float.NaN || schema.AreaList[0].MaxY == float.NaN || schema.AreaList[0].MaxY == 0 || schema.AreaList[0].MinY == 0)
            {
                area.AxisY.Minimum = Double.NaN;
                area.AxisY.Maximum = Double.NaN;
            }
            else
            {
                // area.AxisY.IsLogarithmic = true;
                area.AxisY.Minimum = schema.AreaList[0].MinY;
                area.AxisY.Maximum = schema.AreaList[0].MaxY;
            }


            string[] xValues = null;
            if (table != null && !String.IsNullOrEmpty(schema.XField))
            {
                xValues = table.GetFieldData(schema.XField);
            }


            area.AxisX.MajorGrid.Enabled = schema.ShowGridHLine;
            area.AxisX.MajorGrid.LineColor = ToColor(schema.GridHLineColor);
            area.AxisX.MajorGrid.LineWidth = schema.GridHLineWidth;

            area.AxisY.MajorGrid.Enabled = schema.ShowGridVLine;
            area.AxisY.MajorGrid.LineColor = ToColor(schema.GridVLineColor);
            area.AxisY.MajorGrid.LineWidth = schema.GridVLineWidth;

            int mutYOffset = MutiYOffSetUnit;

            if (xChart.XYReverse)
                ShowReverseSeries(table, area);
            else
                foreach (SeriesSchema se in schema.SeriesList)
                {
                    Series series = msChart.Series.Add(se.Id);
                    series.ChartType = (SeriesChartType)se.ChartType;
                    series.Color = ToColor(se.Color);
                    series.MarkerStyle = (System.Web.UI.DataVisualization.Charting.MarkerStyle)se.MarkerStyle;
                    //  series.AxisLabel=
                    series.ChartArea = area.Name;
                    series.BorderWidth = se.LineWidth;
                    series.Legend = "Default";
                    series.YAxisType = AxisType.Primary;
                    area.AxisY.IsStartedFromZero = se.IsFromY0;
                    // series.
                    if (se.UsingY2)
                    {
                        series.YAxisType = AxisType.Secondary;
                        area.AxisY2.LineColor = series.Color;
                        area.AxisY2.LabelStyle.ForeColor = series.Color;
                        area.AxisY2.IsStartedFromZero = se.IsFromY0;
                    }
                    if (table != null)
                    {
                        try
                        {
                            series.LegendText = table.FieldCaptions[se.Id];
                        }
                        catch
                        {
                            series.LegendText = se.Id;
                        }


                        string[] values = new string[0];
                        string[] valuesUp = new string[0];
                        string[] valuesDown = new string[0];

                        int count = 0;
                        try { values = table.GetFieldData(se.Id); }
                        catch { }
                        if (values != null)
                            count = values.Length;

                        try { valuesUp = table.GetFieldData(se.UpSeries); }
                        catch { }
                        if (valuesUp != null && valuesUp.Length > count) count = valuesUp.Length;

                        try { valuesDown = table.GetFieldData(se.DownSeries); }
                        catch { }
                        if (valuesDown != null && valuesDown.Length > count) count = valuesDown.Length;



                        for (int i = 0; i < count; i++)
                        {
                            double v = 0;
                            try
                            {
                                v = double.Parse(values[i]);
                            }
                            catch { }

                            double vUp = 0;
                            try
                            {
                                vUp = double.Parse(valuesUp[i]);
                            }
                            catch { }

                            double vDown = 0;
                            try
                            {
                                vDown = double.Parse(valuesDown[i]);
                            }
                            catch { }

                            object x = i;
                            try { x = xValues[i]; }
                            catch { }

                            if (area.AxisX.IntervalType != DateTimeIntervalType.Auto &&
                                       area.AxisX.IntervalType != DateTimeIntervalType.NotSet &&
                                          area.AxisX.IntervalType != DateTimeIntervalType.Number)
                            {
                                try { x = DateTime.Parse(xValues[i]); }
                                catch { }
                            }
                            else
                            {
                                try { x = Double.Parse(xValues[i]); }
                                catch { }
                            }



                            switch (series.ChartType)
                            {
                                case SeriesChartType.SplineRange:
                                case SeriesChartType.Range:
                                case SeriesChartType.RangeColumn:
                                case SeriesChartType.RangeBar:
                                    //  series.Points.AddY(vUp, vDown);
                                    series.Points.AddXY(x, vUp, vDown);
                                    break;
                                default:
                                    series.Points.AddXY(x, v);
                                    break;
                            }

                            //     if (xValues != null)
                            //         series.Points[i].AxisLabel = xValues[i];
                        }

                        //       Legend lg = new Legend(se.Id);
                        // lg.s
                        //       msChart.Legends.Add(lg);
                        //     lg.Title = table.FieldCaptions[se.Id];
                        //msChart.Legends[se.Id].Title = table.FieldCaptions[se.Id];
                    }
                    if (se.CreateAxisY && !se.UsingY2)
                    {
                        this.CreateYAxis(msChart, area, series, mutYOffset, MutiYOffSetUnit);
                        mutYOffset += MutiYOffSetUnit;
                    }

                    series.PostBackValue = "#AXISLABEL";
                    series.ToolTip = "#LEGENDTEXT \n" +
                                     "X轴：#VALX\n" +
                                     "Y轴：#VAL";

                }
            //msChart.Style=

            //    foreach (BI.Schema.ChartArea areaSch in schema.AreaList)
            //    {
            //        System.Web.UI.DataVisualization.Charting.ChartArea chartArea = msChart.ChartAreas.Add(areaSch.Id);
            //    }

        }//Method

        private void ShowReverseSeries(DataSource table, System.Web.UI.DataVisualization.Charting.ChartArea area)
        {
            //   area.AxisX.LabelStyle.Enabled = false;
            area.AxisX.LabelStyle.Interval = 0;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;

            string[] xValues = null;
            ChartSchema schema = xChart.Schema;

            if (table != null && !String.IsNullOrEmpty(schema.XField))
            {
                xValues = table.GetFieldData(schema.XField);
            }
            if (xValues == null) return;
            if (schema.SeriesList.Count < 1) return;
            SeriesSchema se0 = schema.SeriesList[0];
            area.AxisY.IsStartedFromZero = se0.IsFromY0;

            Dictionary<string, string[]> YDatas = new Dictionary<string, string[]>();

            foreach (SeriesSchema se in schema.SeriesList)
            {
                string[] values = new string[0];
                try
                {
                    values = table.GetFieldData(se.Id);
                }
                catch { }
                YDatas.Add(se.Id, values);
            }


            for (int i = 0; i < xValues.Length; i++)
            {
                string name = xValues[i];
                if (area.AxisX.IntervalType != DateTimeIntervalType.Auto &&
                           area.AxisX.IntervalType != DateTimeIntervalType.NotSet &&
                              area.AxisX.IntervalType != DateTimeIntervalType.Number)
                {
                    try { name = DateTime.Parse(name).ToString(area.AxisX.LabelStyle.Format); }
                    catch { }
                }
                if (string.IsNullOrEmpty(name) || name.Equals("null"))
                    name = xValues[i];
                Series series = msChart.Series.Add(name);
                series.Legend = "Default";

                series.ChartType = (SeriesChartType)se0.ChartType;
                series.Color = ToColor(se0.Color);
                series.MarkerStyle = (System.Web.UI.DataVisualization.Charting.MarkerStyle)se0.MarkerStyle;
                //  series.AxisLabel=
                //series.ChartArea = se.Area;
                series.BorderWidth = se0.LineWidth;
                series.Points.Clear();
                foreach (string k in YDatas.Keys)
                {
                    double v = 0;
                    string x = table.FieldCaptions[k];
                    if (x == null || x == "")
                        x = k;

                    try { v = double.Parse(YDatas[k][i]); }
                    catch { }

                    int j = series.Points.AddY(v);
                    series.Points[j].AxisLabel = x;
                }

            }

        }//Method




        public void CreateYAxis(System.Web.UI.DataVisualization.Charting.Chart chart, System.Web.UI.DataVisualization.Charting.ChartArea area, Series series, float axisOffset, float labelsSize)
        {
            area.Position = new ElementPosition(axisOffset, 20, 100 - axisOffset, 80);
            area.InnerPlotPosition = new ElementPosition(labelsSize, 0, 100 - axisOffset - labelsSize, 80);

            // Create new chart area for original series

            System.Web.UI.DataVisualization.Charting.ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;

            series.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            System.Web.UI.DataVisualization.Charting.ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            foreach (DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            // IsDisable grid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            areaAxis.AxisY.LineColor = series.Color;
            areaAxis.AxisY.LabelStyle.ForeColor = series.Color;
            // Adjust area position


            areaAxis.Position.X -= axisOffset;
            areaAxis.InnerPlotPosition.X += labelsSize;
        } //Method

        private Color ToColor(string color)
        {

            int red, green, blue = 0;
            char[] rgb;
            if (string.IsNullOrEmpty(color))
                return Color.Empty;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }


        #region ISessionWbo 成员

        public ISession Session
        {
            get
            {
                return this.session;
            }
            set
            {
                this.session=value;
            }
        }

        #endregion


        public string Render()
        {
            throw new NotImplementedException();
        }
    }
}
