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

            

            SKPaint skpaintWhiteStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 10,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };

            SKRect skRectangle = new SKRect();
            skRectangle.Size = new SKSize(300, 500);
            skRectangle.Location = new SKPoint(-150, -250);

            var _sweepAngle = 0 * t + 360 * (1 - t);

            float startAngle = _sweepAngle;
            float sweepAngle = 5; // (75 / 100) * 360

            SKPath skPath = new SKPath();
            skPath.AddArc(skRectangle, startAngle, sweepAngle);
            skCanvas.DrawPath(skPath, skpaintWhiteStroke);

            // paint orbit
            skpaintWhiteStroke.Color = SKColors.OrangeRed.WithAlpha(128);
            skpaintWhiteStroke.StrokeWidth = 5;
            skPath.AddArc(skRectangle, 0, 360);
            skCanvas.DrawPath(skPath, skpaintWhiteStroke);
            
            //SKPaint paint = new SKPaint
            //{
            //    Style = SKPaintStyle.Stroke
            //};

            //SKPoint center = new SKPoint(0,0);
            //float baseRadius = Math.Min(skCanvasWidth, skCanvasHeight) / 12;

            //for (int circle = 0; circle < 5; circle++)
            //{
            //    float radius = baseRadius * (circle + t);

            //    paint.StrokeWidth = baseRadius / 2 * (circle == 0 ? t : 1);
            //    paint.Color = new SKColor(0, 0, 255,
            //        (byte)(255 * (circle == 4 ? (1 - t) : 1)));

            //    skCanvas.DrawCircle(center.X, center.Y, radius, paint);
            //}

        }
    }
}