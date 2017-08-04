using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiaSharpAtomStructure
{
    public partial class MainPage : ContentPage
    {

        const double cycleTime = 1000;       // in milliseconds
        
        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        float t;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                t = (float)(stopwatch.Elapsed.TotalMilliseconds % cycleTime / cycleTime);
                CanvasView.InvalidateSurface();

                if (!pageIsActive)
                {
                    stopwatch.Stop();
                }
                return pageIsActive;
            });
        }


        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var skImageInfo = e.Info;
            var skSurface = e.Surface;
            var skCanvas = skSurface.Canvas;

            var skCanvasWidth = skImageInfo.Width;
            var skCanvasHeight = skImageInfo.Height;

            skCanvas.Clear();

            skCanvas.Translate((float)skCanvasWidth / 2, (float)skCanvasHeight / 2);
            
            using (SKPaint paintCenter = new SKPaint())
            {
                paintCenter.Style = SKPaintStyle.Fill;
                paintCenter.Color = SKColors.Black;
                paintCenter.IsDither = true;
                skCanvas.DrawCircle(0, 0, 45, paintCenter);
            }

            SKPaint paintStaticElectronOrbit = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 5,
                Color = SKColors.Black,
            };

            Random rand = new Random();
            int electronsCount = 4; //rand.Next(1, 50);
            float orbitAngleDegree = 180 / (float)electronsCount;
            for (double degrees = 0; degrees < (180); degrees += orbitAngleDegree)
            {
                var arcRectWidth = 350;
                var arcRectHeight = 100;

                skCanvas.DrawOval(0, 0, arcRectWidth, arcRectHeight, paintStaticElectronOrbit);


                SKPaint paintElectron = new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    IsAntialias = true,
                    StrokeCap = SKStrokeCap.Round,
                    StrokeWidth = 15,
                };

                SKRect paintMovingElectronOrbit = new SKRect();
                paintMovingElectronOrbit.Size = new SKSize((arcRectWidth * 2), (arcRectHeight * 2));
                paintMovingElectronOrbit.Location = new SKPoint(-(arcRectWidth * 2 / 2), -(arcRectHeight * 2 / 2));

                var _sweepAngle = 0 * t + 360 * (1 - t);

                float electronDrawStartPoint = _sweepAngle;
                float electronDrawSize = 1; // (75 / 100) * 360

                SKPath pathMovingElectronOrbit = new SKPath();
                pathMovingElectronOrbit.AddArc(paintMovingElectronOrbit, electronDrawStartPoint, electronDrawSize);
                skCanvas.DrawPath(pathMovingElectronOrbit, paintElectron);




                if (degrees == 0 && electronsCount % 2 == 0)
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree);
                }
                else
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree + 180);
                }

            }





            //for (int i = 0; i < 5; i++)
            //{
            //    SKPaint skpaint = new SKPaint()
            //    {
            //        Style = SKPaintStyle.Stroke,
            //        Color = SKColors.Red,
            //        StrokeWidth = 10,
            //        IsAntialias = true,
            //        StrokeCap = SKStrokeCap.Round
            //    };

            //    var arcRectWidth = 350;
            //    var arcRectHeight = 100;

            //    SKRect skRectangle = new SKRect();
            //    skRectangle.Size = new SKSize(arcRectWidth, arcRectHeight);
            //    skRectangle.Location = new SKPoint(-(arcRectWidth / 2), -(arcRectHeight / 2));

            //    var _sweepAngle = 0 * t + 360 * (1 - t);

            //    float startAngle = _sweepAngle;
            //    float sweepAngle = 5; // (75 / 100) * 360

            //    SKPath skPath = new SKPath();
            //    skPath.AddArc(skRectangle, startAngle, sweepAngle);
            //    skCanvas.DrawPath(skPath, skpaint);

            //    // paint orbit
            //    skpaint.Color = SKColors.OrangeRed.WithAlpha(128);
            //    skpaint.StrokeWidth = 5;
            //    skPath.AddArc(skRectangle, 0, 360);
            //    skCanvas.DrawPath(skPath, skpaint);

            //    skCanvas.RotateDegrees((float)60);
            //}

            
        }
    }
}