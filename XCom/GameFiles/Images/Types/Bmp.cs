//#define hq2xWorks

using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Reflection;
using XCom.Interfaces;
using DSShared;

#if hq2xWorks
using hq2x;
#endif

namespace XCom
{
    public class Bmp
    {
        public static event LoadingDelegate LoadingEvent;

        public static void FireLoadingEvent(int curr, int total)
        {
            if (LoadingEvent != null)
                LoadingEvent(curr, total);
        }

        /// <summary>
        /// Saves an image collection as a bmp sprite sheet
        /// </summary>
        /// <param name="file">output file name</param>
        /// <param name="collection">image collection</param>
        /// <param name="pal">Palette to color the images with</param>
        /// <param name="across">number of columns to use for images</param>
        public static void SaveBMP(string file, XCImageCollection collection, Palette pal, int across, int space)
        {
            if (collection.Count == 1)
                across = 1;

            int mod = 1;
            if (collection.Count % across == 0)
                mod = 0;

            Bitmap b = DSShared.Bmp.MakeBitmap(across * (collection.IXCFile.ImageSize.Width + space) - space, (collection.Count / across + mod) * (collection.IXCFile.ImageSize.Height + space) - space, pal.Colors);

            for (int i = 0; i < collection.Count; i++)
            {
                int x = i % across * (collection.IXCFile.ImageSize.Width + space);
                int y = i / across * (collection.IXCFile.ImageSize.Height + space);
                DSShared.Bmp.Draw(collection[i].Image, b, x, y);
            }
            DSShared.Bmp.Save(file, b);
        }

        /// <summary>
        /// Loads a previously saved sprite sheet as a generic collection to be saved later
        /// </summary>
        /// <param name="b">bitmap containing sprites</param>
        /// <returns></returns>
        public static XCImageCollection Load(Bitmap b, Palette pal, int imgWid, int imgHei, int space)
        {
            XCImageCollection list = new XCImageCollection();

            int cols = (b.Width + space) / (imgWid + space);
            int rows = (b.Height + space) / (imgHei + space);

            int num = 0;

            //Console.WriteLine("Image: {0},{1} -> {2},{3}",b.Width,b.Height,cols,rows);
            for (int i = 0; i < cols * rows; i++)
            {
                int x = (i % cols) * (imgWid + space);
                int y = (i / cols) * (imgHei + space);
                //Console.WriteLine("{0}: {1},{2} -> {3}",num,x,y,PckImage.Width);
                list.Add(LoadTile(b, num++, pal, x, y, imgWid, imgHei));
                FireLoadingEvent(num, rows * cols);
            }

            list.Pal = pal;

            return list;
        }

        public static XCImage LoadTile(Bitmap src, int imgNum, Palette p, int startX, int startY, int imgWid, int imgHei)
        {
            //image data in 8-bit form
            byte[] data = new byte[imgWid * imgHei];

            Rectangle srcRect = new Rectangle(startX, startY, imgWid, imgHei);
            BitmapData srcData = src.LockBits(srcRect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            IntPtr srcPixels = srcData.Scan0;

            unsafe
            {
                byte* sBits;
                if (srcData.Stride > 0)
                    sBits = (byte*)srcPixels.ToPointer();
                else
                    sBits = (byte*)srcPixels.ToPointer() + srcData.Stride * (src.Height - 1);

                uint sStride = (uint)Math.Abs(srcData.Stride);

                for (uint row = 0, i = 0; row < imgHei; row++)
                    for (uint col = 0; col < imgWid; col++)
                        data[i++] = *(sBits + row * sStride + col);
            }

            src.UnlockBits(srcData);

            return new XCImage(data, imgWid, imgHei, p, imgNum);
        }

        /*		public static XCImageCollection Load(string file, Type collectionType)
                {
                    Bitmap b = new Bitmap(file);

                    MethodInfo mi = collectionType.GetMethod("FromBmp");
                    if(mi==null)
                        return null;
                    else
                        return (XCImageCollection)mi.Invoke(null,new object[]{b});
                }

                public static XCImage LoadSingle(Bitmap src,int num,Palette pal,Type collectionType)
                {			
        //			return PckFile.FromBmpSingle(src,num,pal);

                    MethodInfo mi = collectionType.GetMethod("FromBmpSingle");
                    if(mi==null)
                        return null;
                    else
                        return (XCImage)mi.Invoke(null,new object[]{src,num,pal});
                }*/
    }
}
