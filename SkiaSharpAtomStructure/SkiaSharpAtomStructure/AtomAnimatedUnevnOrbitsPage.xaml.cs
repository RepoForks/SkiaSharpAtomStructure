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
	public partial class AtomAnimatedUnevnOrbitsPage : ContentPage
    {
        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;

        private int ElectronsCount { get; set; }

        List<MovingElectronObjectWithOrbit> _movingElectronObjects = new List<MovingElectronObjectWithOrbit>();

        public AtomAnimatedUnevnOrbitsPage ()
		{
			InitializeComponent ();

            ElectronsCount = 6;
            LabelElectronsCount.Text = $"Electrons: {ElectronsCount}";
            InitAtom();
        }

        private void InitAtom()
        {
            Random rand = new Random();
            _movingElectronObjects = new List<MovingElectronObjectWithOrbit>();

            for (int i = 0; i < ElectronsCount; i++) //rand.Next(1, 50)
            {
                var arcRectWidth = rand.Next(230, 350); 
                var arcRectHeight = 100;

                _movingElectronObjects.Add(new MovingElectronObjectWithOrbit()
                {
                    OrbitCycleTime = rand.Next(1000, 2000),
                    TimeAtPointInOrbit = 0,
                    OrbitRectHeight = arcRectHeight,
                    OrbitRectWidth = arcRectWidth,
                });
            }
        }

        private void InitAnimation()
        {
            pageIsActive = true;
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                for (int i = 0; i < _movingElectronObjects.Count; i++)
                {
                    _movingElectronObjects[i].TimeAtPointInOrbit = (float)(stopwatch.Elapsed.TotalMilliseconds % _movingElectronObjects[i].OrbitCycleTime / _movingElectronObjects[i].OrbitCycleTime);
                }

                CanvasView.InvalidateSurface();
                if (!pageIsActive)
                {
                    stopwatch.Stop();
                }

                return pageIsActive;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitAnimation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
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

            SKPaint paintStaticElectronOrbit = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 5,
                Color = SKColors.Black,
            };

            var colors = new[] { SKColors.Gray, SKColors.LightGray, SKColors.Silver.WithAlpha(128) };
            using (var shader = SKShader.CreateSweepGradient(
                                    new SKPoint(0, 0), colors, null))
            {
                paintStaticElectronOrbit.Shader = shader;
            }

            Random rand = new Random();
            float orbitAngleDegree = 180 / (float)_movingElectronObjects.Count;
            for (int i = 0; i < _movingElectronObjects.Count; i++)
            {
                
                //SKRect rectStaticElectronOrbit = new SKRect();
                //rectStaticElectronOrbit.Size = new SKSize((_movingElectronObjects[i].OrbitRectWidth * 2), (_movingElectronObjects[i].OrbitRectHeight * 2));
                //rectStaticElectronOrbit.Location = new SKPoint(-(_movingElectronObjects[i].OrbitRectWidth * 2 / 2), -(_movingElectronObjects[i].OrbitRectHeight * 2 / 2));

                //float electronOrbitDrawStartPoint = 0;

                //var _orbitSweepAngle = 0 * _movingElectronObjects[i].TimeAtPointInOrbit + 360 * (1 - _movingElectronObjects[i].TimeAtPointInOrbit);
                //electronOrbitDrawStartPoint = _orbitSweepAngle;

                //float electronOrbitDrawSize = 360; // (75 / 100) * 360

                //SKPath pathElectronOrbit = new SKPath();
                //pathElectronOrbit.AddArc(rectStaticElectronOrbit, electronOrbitDrawStartPoint, electronOrbitDrawSize);
                //skCanvas.DrawPath(pathElectronOrbit, paintStaticElectronOrbit);


                //skCanvas.DrawOval(0, 0, _movingElectronObjects[i].OrbitRectWidth, _movingElectronObjects[i].OrbitRectHeight, paintStaticElectronOrbit);
                
                SKPaint paintElectron = new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    IsAntialias = true,
                    StrokeCap = SKStrokeCap.Round,
                    StrokeWidth = 15,
                };

                SKRect paintMovingElectronOrbit = new SKRect();
                paintMovingElectronOrbit.Size = new SKSize((_movingElectronObjects[i].OrbitRectWidth * 2), (_movingElectronObjects[i].OrbitRectHeight * 2));
                paintMovingElectronOrbit.Location = new SKPoint(-(_movingElectronObjects[i].OrbitRectWidth * 2 / 2), -(_movingElectronObjects[i].OrbitRectHeight * 2 / 2));

                float electronDrawStartPoint = 0;

                var _sweepAngle = 0 * _movingElectronObjects[i].TimeAtPointInOrbit + 360 * (1 - _movingElectronObjects[i].TimeAtPointInOrbit);
                electronDrawStartPoint = _sweepAngle;

                float electronDrawSize = 180; // (75 / 100) * 360

                SKPath pathMovingElectronOrbit = new SKPath();
                pathMovingElectronOrbit.AddArc(paintMovingElectronOrbit, electronDrawStartPoint, electronDrawSize);
                skCanvas.DrawPath(pathMovingElectronOrbit, paintElectron);


                if (i == 0 && _movingElectronObjects.Count % 2 == 0)
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree);
                }
                else
                {
                    skCanvas.RotateDegrees((float)orbitAngleDegree + 180);
                }
            }
        }



        private SKPoint _lastTouchPoint = new SKPoint();
        private async void CanvasView_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Pressed)
            {
                _lastTouchPoint = e.Location;
                e.Handled = true;
            }
            else if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Moved)
            {
                if (_lastTouchPoint.Y < e.Location.Y)
                {
                    // swipe down

                    if (ElectronsCount == 1)
                        return;

                    pageIsActive = false;
                    ElectronsCount--;
                    InitAtom();
                    InitAnimation();
                }
                else if (_lastTouchPoint.Y > e.Location.Y)
                {
                    // swipe up

                    pageIsActive = false;
                    await Task.Delay(TimeSpan.FromMilliseconds(33));

                    ElectronsCount++;
                    InitAtom();
                    InitAnimation();
                }

                _lastTouchPoint = e.Location;

                LabelElectronsCount.Text = $" Electrons: {ElectronsCount}";
            }
        }

        private async void PlusOrMinusButtons_Clicked(object sender, EventArgs e)
        {
            if (((Button)(sender)).Text == "+")
            {
                pageIsActive = false;
                await Task.Delay(TimeSpan.FromMilliseconds(33));

                ElectronsCount++;
                InitAtom();
                InitAnimation();
            }
            else if (((Button)(sender)).Text == "-")
            {
                if (ElectronsCount == 1)
                    return;

                pageIsActive = false;
                ElectronsCount--;
                InitAtom();
                InitAnimation();
            }

            LabelElectronsCount.Text = $" Electrons: {ElectronsCount}";
        }
    }

    public class MovingElectronObjectWithOrbit : MovingElectronObject
    {
        public float OrbitRectWidth{ get; set; }

        public float OrbitRectHeight { get; set; }
    }
}