/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System;
using System.Linq;
using System.IO;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 图片相关
    /// </summary>
    public static class ImgLib
    {
        /// <summary>
        /// 指定坐标和宽高裁剪图片
        /// </summary>
        /// <param name="img">原图路径</param>
        /// <param name="width">指定的宽度</param>
        /// <param name="height">指定的高度</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>System.Drawing.Image,再调用save就行了,注意：调用完后需要Dispose</returns>
        public static System.Drawing.Image Crop(string img, int width, int height, int x, int y)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(img))
            {
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                bmp.SetResolution(80, 60);
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    gfx.SmoothingMode = SmoothingMode.AntiAlias;
                    gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gfx.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
                }
                return bmp;
            }
        }

        ///<summary>
        ///生成缩略图
        ///</summary>
        ///<param name="originalImagePath">源图路径（物理路径）</param>
        ///<param name="thumbnailPath">缩略图路径（物理路径）</param>
        ///<param name="width">缩略图宽度</param>
        ///<param name="height">缩略图高度</param>
        ///<param name="mode">生成缩略图的方式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, XCLNetTools.Enum.CommonEnum.ThumbImageModeEnum mode = XCLNetTools.Enum.CommonEnum.ThumbImageModeEnum.EqualRatioWH)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            if (null == originalImage)
            {
                return;
            }
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width or height is invalid!");
            }

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case XCLNetTools.Enum.CommonEnum.ThumbImageModeEnum.W:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;

                case XCLNetTools.Enum.CommonEnum.ThumbImageModeEnum.H:
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;

                case XCLNetTools.Enum.CommonEnum.ThumbImageModeEnum.EqualRatioWH:
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;

                case XCLNetTools.Enum.CommonEnum.ThumbImageModeEnum.WH:
                default:
                    break;
            }
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 根据图片exif调整方向
        /// </summary>
        public static Image RotateImage(Image img)
        {
            var exif = img.PropertyItems;
            byte orien = 0;
            var item = exif.Where(m => m.Id == 274).ToArray();
            if (item.Length > 0)
            {
                orien = item[0].Value[0];
            }
            switch (orien)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip
                    break;

                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top
                    break;

                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip
                    break;

                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;

                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top
                    break;

                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;

                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom
                    break;

                default:
                    break;
            }
            return img;
        }

        /// <summary>
        /// 获取本地图片
        /// </summary>
        public static Image GetImage(string path)
        {
            using (Stream stm = File.Open(path, FileMode.Open))
            {
                var img = RotateImage(Image.FromStream(stm));
                return img;
            }
        }
    }
}