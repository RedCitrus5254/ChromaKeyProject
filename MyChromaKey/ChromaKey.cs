using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyChromaKey
{
    class ChromaKey : IChromaKey
    {
        private ISubscriber subscriber;
        public void Subscribe(ISubscriber subscriber)
        {
            this.subscriber = subscriber;
        }

        private void NotifySubscriber(Bitmap image)
        {
            subscriber.UpdateImage(image);
        }

        public void CropImage(Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] msv = new byte[bmpData.Stride * bmpData.Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, msv, 0, msv.Length);
            bitmap.UnlockBits(bmpData);

            byte[] mask = GetGrayImage(msv); // делаем 8 бит на пиксель
            mask = GetSobelMask(mask, bmpData.Width, bmpData.Height); //применяем оператор Собеля и устраняем шумы

            mask = Dilate(mask, bmpData.Width, bmpData.Height); //далее избавляемся от шумов посредством эрозии и расширения

            mask = Erosion(mask, bmpData.Width, bmpData.Height);

            mask = Erosion(mask, bmpData.Width, bmpData.Height);
            mask = Dilate(mask, bmpData.Width, bmpData.Height);


            mask = Erosion(mask, bmpData.Width, bmpData.Height);
            mask = Dilate(mask, bmpData.Width, bmpData.Height);


            mask = Erosion(mask, bmpData.Width, bmpData.Height);

            Posterize(mask); //постеризация для удобства, так как остаются только чёрные и белые пиксели
            FloodFill(msv, mask, bmpData.Width, bmpData.Height); //заливка исходного изображения вокруг маски
            Bitmap bitmapGray = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData bmpDataGray = bitmapGray.LockBits(new Rectangle(0, 0, bitmapGray.Width, bitmapGray.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(msv, 0, bmpDataGray.Scan0, bmpDataGray.Stride * bmpDataGray.Height);
            bitmapGray.UnlockBits(bmpDataGray);
            NotifySubscriber(bitmapGray); //оповещаем presenter и передаём ему новое изображение

        }

        private byte[] GetGrayImage(byte[] rgbaImage)
        {

            byte[] grayImage = new byte[rgbaImage.Length / 4];
            for(int i = 0, j=0; i < rgbaImage.Length; i+=4, j++)
            {
                grayImage[j] = Convert.ToByte((rgbaImage[i] + rgbaImage[i + 1] + rgbaImage[i + 2]) / 3);
            }
            return grayImage;
        }

        private byte[] GetSobelMask(byte[] grayImageMass, int width, int height)
        {
            byte[] mask = new byte[grayImageMass.Length];

            for(int i = 0; i < width; i++)
            {
                mask[i] = 0;
            }

            for(int i = 1; i < height - 1; i++)
            {
                mask[i * width] = 0;

                int iMinusOnePos = (i - 1) * width;
                int iPos = i * width;
                int iPlusOnePos = (i + 1) * width;

                for(int j = 1; j < width - 1; j++)
                {
                    int first = grayImageMass[iMinusOnePos + j - 1] + 2 * grayImageMass[iMinusOnePos + j] + grayImageMass[iMinusOnePos + j + 1] - grayImageMass[iPlusOnePos + j - 1] - 2 * grayImageMass[iPlusOnePos + j] - grayImageMass[iPlusOnePos + j + 1];
                    int second = grayImageMass[iMinusOnePos + j + 1] + 2 * grayImageMass[iPos + j + 1] + grayImageMass[iPlusOnePos + j + 1] - grayImageMass[iMinusOnePos + j - 1] - 2 * grayImageMass[iPos + j - 1] - grayImageMass[iPlusOnePos + j - 1];
                    mask[i * width + j] = Convert.ToByte((Math.Abs(first) + Math.Abs(second)) / 16); //делим на 8 (8*255 -- максимально возможное значение цвета; делим на 2 для уменьшения количества шумов)
                    //if (mask[i * width + j] < 5)
                    //{
                    //    mask[i * width + j] = 0;
                    //}
                }
                mask[i * width - 1] = 0;
            }

            for (int i = width * (height - 1); i < width * height; i++) 
            {
                mask[i] = 0;
            }

            return mask;
        }

        private byte[] Dilate(byte[] mass, int width, int height)
        {
            int elementSize = Convert.ToInt32(Math.Sqrt(width * height) / 300);
            if (elementSize < 3)
                elementSize = 3;

            if (elementSize > 20)
                elementSize = 20;

            byte[] dilateMass = new byte[mass.Length];

            for(int i = 0; i < elementSize*width; i++)//заполняем необрабатываемые элементы числами их исходного массива
            {
                dilateMass[i] = mass[i];
            }
            
            for(int i = elementSize; i < height - elementSize - 1; i++)
            {
                for(int n = i * width; n < i * width + elementSize / 2; n++) //заполняем необрабатываемые элементы числами их исходного массива
                {
                    dilateMass[n] = mass[n];
                }


                for(int j = elementSize; j < width - elementSize - 1; j++)
                {

                    byte max = 0;
                    for(int k = i - elementSize / 2; k < i + elementSize / 2; k++)
                    {
                        for(int h = j - elementSize / 2; h < j + elementSize / 2; h++)
                        {
                            if (mass[k * width + h] > max)
                            {
                                max = mass[k * width + h];

                            }
                        }
                    }
                    dilateMass[i * width + j] = max;
                }

                for(int n = (i + 1) * width - elementSize / 2; n < (i + 1) * width; n++)//заполняем необрабатываемые элементы числами их исходного массива
                {
                    dilateMass[n] = mass[n];
                }
            }

            for(int i = width * (height - elementSize); i < width * height; i++)//заполняем необрабатываемые элементы числами их исходного массива
            {
                dilateMass[i] = mass[i];
            }

            return dilateMass;
        }

        public byte[] Erosion(byte[] mass, int width, int height)
        {
            int elementSize = Convert.ToInt32(Math.Sqrt(width * height) / 300);
            if (elementSize < 3)
                elementSize = 3;

            if (elementSize > 20)
                elementSize = 20;

            byte[] erosionMass = new byte[mass.Length];

            for (int i = 0; i < elementSize * width; i++)//заполняем необрабатываемые элементы числами их исходного массива
            {
                erosionMass[i] = mass[i];
            }

            for (int i = elementSize; i < height - elementSize - 1; i++)
            {
                for (int n = i * width; n < i * width + elementSize / 2; n++) //заполняем необрабатываемые элементы числами их исходного массива
                {
                    erosionMass[n] = mass[n];
                }


                for (int j = elementSize; j < width - elementSize - 1; j++)
                {

                    byte min = 255;
                    for (int k = i - elementSize / 2; k < i + elementSize / 2; k++)
                    {
                        for (int h = j - elementSize / 2; h < j + elementSize / 2; h++)
                        {
                            if (mass[k * width + h] < min)
                            {
                                min = mass[k * width + h];
                            }
                        }
                    }
                    erosionMass[i * width + j] = min;
                }

                for (int n = (i + 1) * width - elementSize / 2; n < (i + 1) * width; n++)//заполняем необрабатываемые элементы числами их исходного массива
                {
                    erosionMass[n] = mass[n];
                }
            }

            for (int i = width * (height - elementSize); i < width * height; i++)//заполняем необрабатываемые элементы числами их исходного массива
            {
                erosionMass[i] = mass[i];
            }

            return erosionMass;
        }

        private void Posterize(byte[] mass)
        {
            for(int i = 0; i < mass.Length; i++)
            {
                if (mass[i] >= 1)
                {
                    mass[i] = 255;
                }
                else
                {
                    mass[i] = 0;
                }
            }
        }

        private void FloodFill(byte[] inputMass, byte[] mask, int width, int height)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(0, 0));
            byte maskBorderColor = 255;
            byte newColor = 0;
            while (stack.Count > 0)
            {
                Point curPoint = stack.Pop();

                int xLeft = curPoint.X;
                int xRight = curPoint.X;


                //поиск размера линии слева от текущего пикселя
                while(xLeft - 1 >= 0 && inputMass[curPoint.Y * width * 4 + (xLeft - 1) * 4 + 3] != newColor && mask[curPoint.Y * width + xLeft - 1] != maskBorderColor)
                {
                    xLeft--;
                }

                //поиск размера линии справа от текущего пикселя
                while (xRight + 1 < width && inputMass[curPoint.Y * width * 4 + (xRight + 1) * 4 + 3] != newColor &&  mask[curPoint.Y * width + xRight + 1] != maskBorderColor)
                {
                    xRight++;
                }

                if((curPoint.Y - 1 >= 0))  //проверка пикселя под текущим
                {
                    for (int i = xLeft; i < xRight; i++)
                    {
                        if ((mask[(curPoint.Y - 1) * width + i] != maskBorderColor) && (inputMass[(curPoint.Y - 1) * width * 4 + i * 4 + 3] != newColor))
                        {
                            stack.Push(new Point(i, curPoint.Y - 1));
                        }
                    }
                }

                if(curPoint.Y + 1 < height) //проверка пикселя под текущим
                {
                    for (int i = xLeft; i < xRight; i++)
                    {
                        if ((mask[(curPoint.Y + 1) * width + i] != maskBorderColor) && (inputMass[(curPoint.Y + 1) * width * 4 + i * 4 + 3] != newColor))
                        {
                            stack.Push(new Point(i, curPoint.Y + 1));
                        }
                    }
                }

                for(int i = xLeft; i < xRight; i++) //заливка линии
                {
                    inputMass[curPoint.Y * width * 4 + i * 4 + 3] = newColor;
                }


            }

        }

        private void CropByMask(byte[] inputImage, byte[] mask)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (inputImage[i * 4 + 3] == 0)
                {
                    inputImage[i * 4] = 255;
                }
            }
        }




    }
}
