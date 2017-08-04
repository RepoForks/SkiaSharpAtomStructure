using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpAtomStructure
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AtomAnimatedPage : ContentPage
    {
        const double cycleTime = 2000;       // in milliseconds

        List<double> cycleTimes;

        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        float t;
        
        List<float> ts;


        bool isElectronsInit = false;

        public AtomAnimatedPage()
        {
            InitializeComponent();

            Random rand = new Random();
            cycleTimes = new List<double>()
            {
                rand.Next(1000,2000),
                rand.Next(1000,2000),
                rand.Next(1000,2000),
                rand.Next(1000,2000),
                rand.Next(1000,2000),
                rand.Next(1000,2000),
                rand.Next(1000,2000),
                rand.Next(1000,2000),
            };

            ts = new List<float>
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                t = (float)(stopwatch.Elapsed.TotalMilliseconds % cycleTime / cycleTime);

                for (int i = 0; i < ts.Count; i++)
                {
                    ts[i] = (float)(stopwatch.Elapsed.TotalMilliseconds % cycleTimes[i] / cycleTimes[i]);
                }

                CanvasView.InvalidateSurface();
                isElectronsInit = true;
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
            int electronsCount = 8; //rand.Next(1, 50);
            float orbitAngleDegree = 180 / (float)electronsCount;
            int x = 0;
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

                float electronDrawStartPoint = 0;
                
                var _sweepAngle = 0 * ts[x] + 360 * (1 - ts[x]);
                electronDrawStartPoint = _sweepAngle;

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

                x++;
            }
        }
    }
}