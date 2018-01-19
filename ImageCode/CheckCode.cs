using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImageCode
{
    public static class CheckCode
    {

        private static Dictionary<string, string> _codes = new Dictionary<string, string>();

        public static void getCode(){
            
            string code = rndStr(4);
            _codes.Add(HttpContext.Current.Session.SessionID, code);
            general(code);
        }

        public static bool checkCode(string code)
        {
            string sId=HttpContext.Current.Session.SessionID;
            if (_codes.ContainsKey(sId))
                return false;
            return _codes[sId].Equals(code, StringComparison.OrdinalIgnoreCase);
        }

        public static void general(String sCc)
        {
            Int32 ccLen = sCc.Length;
            String ccFtFm = "Arial";
            Int32 ccFtSz = 12;
            Int32 ccWidth = ccLen * ccFtSz + 1;
            Int32 ccHeight = ccFtSz + 5;
            using (Bitmap oImg = new Bitmap(ccWidth, ccHeight))
            {
                using (Graphics oGpc = Graphics.FromImage(oImg))
                {
                    HatchBrush hBrush = new HatchBrush(HatchStyle.DashedVertical,
                      Color.Yellow, Color.Silver);
                    oGpc.FillRectangle(hBrush, 0, 0, ccWidth, ccWidth);
                    oGpc.DrawString(sCc, new System.Drawing.Font(ccFtFm, ccFtSz, FontStyle.Bold),
                     new System.Drawing.SolidBrush(Color.Black), 0, 0);
                    //-----------------------边框   
                    Pen blackPen = new Pen(Color.Black, 1);
                    oGpc.DrawLine(blackPen, 0, ccHeight, 0, 0); // 左竖线   
                    oGpc.DrawLine(blackPen, 0, 0, ccWidth, 0); // 顶横线   
                    oGpc.DrawLine(blackPen, ccWidth - 1, 0, ccWidth - 1, 20); // 右竖线   
                    oGpc.DrawLine(blackPen, 0, ccHeight - 1, ccWidth, ccHeight - 1); // 底横线   
                    writeImg(oImg);
                }
            }
        } // end public static void general   

        public static String rndStr(Int32 len)
        {
            String sTemp = "";
            String sForRnd = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            String[] aRnd = sForRnd.Split(',');
            Random oRnd = new Random();
            Int32 iArLen = aRnd.Length;
            for (Int32 i = 0; i < len; i++)
            {
                sTemp += aRnd[oRnd.Next(0, iArLen)];
            }
            return sTemp;
        } // end public static String rndStr   
        //-----------------------------------end public static method   
        //-----------------------------------begin private static method   
        private static void writeImg(Bitmap oImg)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                oImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Png";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
        } // end private static void writeImg   
    }
}
