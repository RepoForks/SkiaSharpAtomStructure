using SkiaSharp;
using SkiaSharp.Views.Forms;
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
    public partial class Test : ContentPage
    {
        float _sweepAngle = 0;
        float _sweepRadius = 0;

        Stopwatch stopwatch = new Stopwatch();
        float scale;

        public Test()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AnimationLoop();
        }

        async Task AnimationLoop()
        {
            stopwatch.Start();

            while (true)
            {
                double cycleTime = 5f;
                double t = stopwatch.Elapsed.TotalSeconds % cycleTime / cycleTime;
                scale = (1 + (float)Math.Sin(2 * Math.PI * t)) / 2;
                CanvasView.InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            }

            stopwatch.Stop();
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
                StrokeWidth = 40,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };

            SKRect skRectangle = new SKRect();
            skRectangle.Size = new SKSize(300, 300);
            skRectangle.Location = new SKPoint(-150, -150);

            _sweepAngle = 0 * scale + 360 * (1 - scale);

            float startAngle = -90;
            float sweepAngle = _sweepAngle; // (75 / 100) * 360

            SKPath skPath = new SKPath();
            skPath.AddArc(skRectangle, startAngle, sweepAngle);
            skCanvas.DrawPath(skPath, skpaintWhiteStroke);

            // filler animation
            skpaintWhiteStroke.Style = SKPaintStyle.Fill;
            skpaintWhiteStroke.Color = SKColors.DarkOrange;

            _sweepRadius = 0 * scale + 130 * (1 - scale);

            skCanvas.DrawCircle(0, 0, _sweepRadius, skpaintWhiteStroke);
        }
    }
}