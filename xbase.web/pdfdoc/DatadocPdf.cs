using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xbase.bi;
using xbase.bi.schema;
using System.IO;
using xbase.umc;
using xbase;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI.DataVisualization.Charting;
using xbase.web.controls;
using xbase.umc.attributes;

namespace xbase.web.app.pdfdoc
{
    [WboAttr(Id = "DatadocPdf", Title = "数据文档PDF", Version = 1.1, LifeCycle = LifeCycle.Global, IsPublish = true)]
    public class DatadocPdf
    {
        private static  string simhei =Path.GetDirectoryName( Environment.SystemDirectory) +"\\Fonts\\simhei.ttf";//宋体
        private static string simsun = Path.GetDirectoryName(Environment.SystemDirectory) + "\\Fonts\\simsun.ttc,1";//黑体

        private Document pdfDoc;
        private DataDoc doc;
        private PdfWriter writer;
        private Font fontFooterCN=new Font(BaseFont.CreateFont(simsun,  BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED ),10);
        private Font h1_Font = new Font(BaseFont.CreateFont(simhei,  BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED ),20);
        private Font h2_Font = new Font(BaseFont.CreateFont(simhei, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 18);
        private Font h3_Font = new Font(BaseFont.CreateFont(simhei, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 18);
        private Font h4_Font = new Font(BaseFont.CreateFont(simhei, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 18);
        private Font h5_Font = new Font(BaseFont.CreateFont(simhei, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 18);
        private Font p_Font = new Font(BaseFont.CreateFont(simsun, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 12);
        private string tempDir=XSite.SitePhysicalPath+"~temp_pdf\\";
        private string tempUrlPath = "/~temp_pdf/";


        [WboMethodAttr(Title = "获取PDF", Description = "根据指定的文档名和标题全路径Id，获取PDF文档")]
        public string GetSubjectPdf(string docName, string subjectId)
        {
            doc = new DataDoc(docName);
            List<Font> sbjFonts = new List<Font>() { h1_Font, h2_Font, h3_Font, h4_Font, h5_Font };
            

            string fileName = Guid.NewGuid().ToString() + ".pdf";
            string retFile =tempDir+ fileName;
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            pdfDoc = new Document(PageSize.A4, 72, 72, 72, 72);//默认值为36磅
            try
            {
                writer = PdfWriter.GetInstance(this.pdfDoc, new FileStream(retFile, FileMode.Create));

                Image headerLogo = Image.GetInstance(XSite.BinServerPath + @"\HeaderLogo.wmf");
             
                HeaderFooter header = new HeaderFooter(new Phrase(new Chunk(headerLogo, 0, 0)), false);
                header.Alignment = Rectangle.ALIGN_LEFT;
                header.Border = Rectangle.BOTTOM_BORDER;
                header.BorderWidth = 0.1f;
                HeaderFooter footer = new HeaderFooter(new Phrase("第 ", fontFooterCN), new Phrase(" 页", fontFooterCN));
                footer.Alignment = Rectangle.ALIGN_RIGHT;
                footer.Border = Rectangle.TOP_BORDER;
                footer.BorderWidth = 0.1f;
                pdfDoc.Header = header;
                pdfDoc.Footer = footer;

                pdfDoc.Open();
                pdfDoc.AddTitle(doc.Title);
                pdfDoc.AddAuthor("云晨期货有限责任公司");
                pdfDoc.AddCreator("云晨期货有限责任公司");
               
                Chapter chapter1 = null;

                List<Subject> subjects = doc.DrillDown(subjectId);

                for (int i = 0; i < subjects.Count; i++)
                {
                    Subject sbj = subjects[i];
                    Font sbjFont = p_Font;
                    int fontLevel = sbj.Level - 1;

                    if (fontLevel>-1 && fontLevel < sbjFonts.Count)
                        sbjFont = sbjFonts[sbj.Level];

                    Paragraph title = new Paragraph(sbj.Title, sbjFont);
                    string stext = sbj.Text;
                    if (!string.IsNullOrEmpty(stext))
                        stext = stext.Trim().Replace(" ", "");
                    Paragraph text = new Paragraph(stext, p_Font);
                    text.FirstLineIndent = 24;
                    text.Leading = 18;
                    text.SpacingAfter = 12;
                    text.Alignment = 3;

                    pdfDoc.Add(title);
                    pdfDoc.Add(text);


                    if (!string.IsNullOrEmpty(sbj.ChartId))
                    {
                       
                        DataChart chart = new DataChart(sbj.ChartId);
//                        chart.Render();
                        System.Drawing.Bitmap bmp = chart.GetBitmap();
//                        bmp.Save(tempDir+sbj.ChartId+ ".bmp");
                        iTextSharp.text.Image itImage = iTextSharp.text.Image.GetInstance(bmp,System.Drawing.Imaging.ImageFormat.Bmp);
                        itImage.ScalePercent(60);
                        pdfDoc.Add(itImage);
                    }

                 //   Image img=Image.GetInstance(


                    //if (sbj.Level == 1)
                    //{
                    //    chapter1 = new Chapter(title, 1);
                    //    chapter1.SetChapterNumber(0);
                    //    pdfDoc.Add(chapter1);
                    //}

                    //if (chapter1 != null)
                    //{
                    //    Section section1 = chapter1.AddSection(title);
                    //    section1.Add(text);
                    //}
                    //else
                    //{
                    //    pdfDoc.Add(title);
                    //    pdfDoc.Add(text);
                    //}

                }
            }
            finally
            {
                pdfDoc.Close();
            }
            return tempUrlPath+fileName;
        }


    }
}
