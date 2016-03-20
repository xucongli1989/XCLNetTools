/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Drawing;
using System.Web;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 验证码相关
    /// </summary>
    public class VerificationCode
    {
        /// <summary>
        /// 生成验证码的随机数
        /// </summary>
        /// <returns>随机数</returns>
        public static string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="checkCode">字符代码</param>
        public static void CreateCheckCodeImage(string checkCode)
        {
            if (string.IsNullOrEmpty(checkCode))
            {
                return;
            }
            using (System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22))
            using (Graphics g = Graphics.FromImage(image))
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Gif";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
        }
    }
}