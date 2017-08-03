using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpAtomStructure
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AtomOrbitalPathsPage : ContentPage
    {
        public AtomOrbitalPathsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // loop through random atom structures
            Device.StartTimer(TimeSpan.FromSeconds(2),
                () =>
                {
                    CanvasView.InvalidateSurface();

                    return true;
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

            SKPaint paintOrbit = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 5,
            };
            var colors = new[] { SKColors.Gray, SKColors.LightGray, SKColors.Silver.WithAlpha(128) };
            using (var shader = SKShader.CreateSweepGradient(
                                    new SKPoint(0, 0), colors, null))
            {
                paintOrbit.Shader = shader;
            }

            Random rand = new Random();
            int electronsCount = rand.Next(1, 50);
            float orbitAngleDegree = 180 / (float)electronsCount;
            for (double degrees = 0; degrees < (180); degrees += orbitAngleDegree)
            {
                var arcRectWidth = 350;
                var arcRectHeight = 100;

                skCanvas.DrawOval(0, 0, arcRectWidth, arcRectHeight, paintOrbit);

                using (SKPaint paintElectron = new SKPaint())
                {
                    paintElectron.Style = SKPaintStyle.Fill;
                    paintElectron.Color = SKColors.Black;
                    paintElectron.IsAntialias = true;
                    skCanvas.DrawCircle(arcRectWidth, 0, 10, paintElectron);
                }

                if (degrees == 0 && electronsCount % 2 == 0)
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree);
                }
                else
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree + 180);
                }
            }
        }
    }
}