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
        private int ElectronsCount { get; set; }

        public AtomOrbitalPathsPage()
        {
            InitializeComponent();

            ElectronsCount = 6;
            LabelElectronsCount.Text = $"Electrons: {ElectronsCount}";
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
                skCanvas.DrawCircle(0, 0, skCanvasWidth / 17f, paintCenter); // 45
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
            
            float orbitAngleDegree = 180 / (float)ElectronsCount;
            for (double degrees = 0; degrees < (180); degrees += orbitAngleDegree)
            {
                var arcRectWidth = skCanvasWidth / 2.2f; //350
                var arcRectHeight = skCanvasHeight / 11.3f; //100

                skCanvas.DrawOval(0, 0, arcRectWidth, arcRectHeight, paintOrbit);

                using (SKPaint paintElectron = new SKPaint())
                {
                    paintElectron.Style = SKPaintStyle.Fill;
                    paintElectron.Color = SKColors.Black;
                    paintElectron.IsAntialias = true;
                    skCanvas.DrawCircle(arcRectWidth, 0, 10, paintElectron);
                }

                if (degrees == 0 && ElectronsCount % 2 == 0)
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree);
                }
                else
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree + 180);
                }
            }
        }

        private void PlusOrMinusButtons_Clicked(object sender, EventArgs e)
        {
            if (((Button)(sender)).Text == "+")
            {
                ElectronsCount++;
            }
            else if (((Button)(sender)).Text == "-")
            {
                if (ElectronsCount == 1)
                    return;
                
                ElectronsCount--;
            }

            LabelElectronsCount.Text = $" Electrons: {ElectronsCount}";

            CanvasView.InvalidateSurface();
        }
    }
}