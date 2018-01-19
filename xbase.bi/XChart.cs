using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.bi.schema;
using System.Drawing;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.bi
{
    [WboAttr(Id = "XChart", Title = "分析图表", Version = 1.1, LifeCycle = LifeCycle.Session, ContainerType = typeof(ChartShemaContainer)
   , IsPublish = true)]
    public class XChart
    {
        private ChartSchema schema;

        public XChart(string name)
        {
            if (!ChartShemaContainer.Instance().Contains(name))
            {
                this.schema = new ChartSchema();
                this.schema.Id = name;
            }
            else
                this.schema = ChartShemaContainer.Instance().GetItem(name);
        }

        public string DataSource
        {
            get
            {
                return schema.DataSource;
            }
            set
            {
                schema.DataSource = value;
            }
        }

        public string Title
        {
            set { schema.Title = value; }
            get { return schema.Title; }
        }

        public string Text
        {
            set { schema.Text = value; }
            get { return schema.Text; }

        }

        public bool Chart3D
        {
            set
            {
                schema.Is3D = value;
            }
            get
            {
                return schema.Is3D;
            }
        }

        public string BackColor
        {
            set
            {
                schema.BackColor = value;
            }
            get
            {
                return schema.BackColor;
            }
        }

        public string XField
        {
            set
            {
                schema.XField = value;
            }
            get
            {
                return schema.XField;
            }

        }

        public string AreaBackColor
        {
            set
            {
                DefaultArea.BackColor = value;
            }
            get
            {
                return DefaultArea.BackColor;
            }
        }

        private ChartArea DefaultArea
        {
            get
            {
                ChartArea ret = schema.AreaList.FindItem(ChartConst.Default);
                if (ret == null)
                    ret = schema.AreaList.NewItem(ChartConst.Default);
                return ret;
            }
        }
        public float MinY
        {
            set
            {
                this.DefaultArea.MinY = value;
            }
            get
            {
                return this.DefaultArea.MinY;
            }

        }

        public float MaxY
        {
            set
            {
                this.DefaultArea.MaxY = value;
            }
            get
            {
                return this.DefaultArea.MaxY;
            }
        }
        public bool XYReverse
        {
            set
            {
                schema.XYReverse = value;
            }
            get
            {
                return schema.XYReverse;
            }
        }

        public ChartSchema Schema
        {
            get { return schema; }
        }

        private SeriesSchema GetSeriesSchema(string series)
        {
            SeriesSchema se = schema.SeriesList.FindItem(series);
            if (se == null)
            {
                se = schema.SeriesList.NewItem(series);
            }
            return se;
        }

        public void SetSeriesChartType(string series, string seriesChartType)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.ChartType = (ChartType)Enum.Parse(typeof(ChartType), seriesChartType);
        }


        public void SetSeriesLineWidth(string series, string seriesLineWidth)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.LineWidth = int.Parse(seriesLineWidth);

        }

        public void SetSeriesMarkType(string series, string seriesMarkType)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.MarkerStyle = (MarkerStyle)Enum.Parse(typeof(MarkerStyle), seriesMarkType);
        }

        public void SetSeriesArea(string series, string chartArea)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.Area = chartArea;
        }


        public void SetSeriesColor(string series, string color)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.Color = color;
        }

        public void RemoveSeries(string series)
        {
            SeriesSchema se = schema.SeriesList.FindItem(series);
            if (se != null)
                schema.SeriesList.Remove(se);
        }

        public void Save()
        {
            ChartShemaContainer.Instance().UpdateItem(schema.Id, schema);
        }

        public void SetSearchParams(string searchParams)
        {
            //char[] a=new char[1](';');

            xbase.SchemaList<xbase.NamedValueSchema> nvList = new xbase.SchemaList<xbase.NamedValueSchema>();

            if (string.IsNullOrEmpty(searchParams)) return;

            string[] nvStrs = searchParams.Split(new char[] { ';' });

            for (int i = 0; i < nvStrs.Length; i++)
            {
                string nvStr = nvStrs[i];
                string[] nvAry = nvStr.Split(new char[] { '=' });

                if (nvAry.Length > 1)
                {
                    xbase.NamedValueSchema nv = new xbase.NamedValueSchema();
                    nv.Id = nvAry[0];
                    nv.Value = nvAry[1];
                    nvList.Add(nv);
                }
            }

            schema.SearchParams = nvList;
        }

        public void SetChartWidth(int p)
        {
            schema.Width = p;
        }

        public void SetChartHeight(int p)
        {
            schema.Height = p;
        }

        public void SetVLineDisplay(bool p)
        {
            schema.ShowGridVLine = p;
        }

        public void SetVLineColor(string color)
        {
            schema.GridVLineColor = color;
        }

        public void SetVLineWidth(int p)
        {
            schema.GridVLineWidth = p;
        }

        public void SetHLineDisplay(bool p)
        {
            schema.ShowGridHLine = p;
        }

        public void SetHLineColor(string color)
        {
            schema.GridHLineColor = color;
        }

        public void SetHLineWidth(int p)
        {
            schema.GridHLineWidth = p;
        }

        public void SetLegenH(bool p)
        {
            schema.LegenH = p;
        }

        public void SetLegenPos(string legenPos)
        {
            schema.LegenPos = legenPos;
        }

        public void SetLegenAlign(string legenAlign)
        {
            schema.LegenAlign = legenAlign;
        }

        public void SetSeriesUpSeries(string series, string upSeries)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.UpSeries = upSeries;
        }

        public void SetSeriesDownSeries(string series, string downSeries)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.DownSeries = downSeries;
        }

        public void SetTitlePos(string chartTitlePos)
        {
            schema.TitlePos = chartTitlePos;
        }

        public void SetSeriesCreateAxisY(string series, string seriesCreateAxisY)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.CreateAxisY = bool.Parse(seriesCreateAxisY);
        }

        public void SetSeriesUsingY2(string series, string seriesUsingY2)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.UsingY2 = bool.Parse(seriesUsingY2);
        }

        public void SetSeriesIsFromY0(string series, string isFromY0)
        {
            SeriesSchema se = GetSeriesSchema(series);
            se.IsFromY0 = bool.Parse(isFromY0);
        }
    }
}
