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
        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        
        private int ElectronsCount { get; set; }
        
        List<MovingElectronObject> _movingElectronObjects = new List<MovingElectronObject>();

        public AtomAnimatedPage()
        {
            InitializeComponent();

            ElectronsCount = 6;
            LabelElectronsCount.Text = $" Electrons: {ElectronsCount}";
            InitAtom();
        }

        private void InitAtom()
        {
            Random rand = new Random();
            _movingElectronObjects = new List<MovingElectronObject>();

            for (int i = 0; i < ElectronsCount; i++) //rand.Next(1, 50)
            {
                _movingElectronObjects.Add(new MovingElectronObject()
                {
                    OrbitCycleTime = rand.Next(1000, 2000),
                    TimeAtPointInOrbit = 0,
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

            Random rand = new Random();
            float orbitAngleDegree = 180 / (float)_movingElectronObjects.Count;
            for (int i = 0; i < _movingElectronObjects.Count; i++)
            {
                var arcRectWidth = skCanvasWidth / 2.2f; //350
                var arcRectHeight = skCanvasHeight / 11.3f; //100

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

                var _sweepAngle = 0 * _movingElectronObjects[i].TimeAtPointInOrbit + 360 * (1 - _movingElectronObjects[i].TimeAtPointInOrbit);
                electronDrawStartPoint = _sweepAngle;

                float electronDrawSize = 1; // (75 / 100) * 360

                SKPath pathMovingElectronOrbit = new SKPath();
                pathMovingElectronOrbit.AddArc(paintMovingElectronOrbit, electronDrawStartPoint, electronDrawSize);
                skCanvas.DrawPath(pathMovingElectronOrbit, paintElectron);


                if (i == 0 || _movingElectronObjects.Count % 2 == 0)
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

    public class MovingElectronObject
    {
        /// <summary>
        /// Electron Orbit's 1 cycle's time in Milliseconds
        /// </summary>
        public double OrbitCycleTime { get; set; }

        /// <summary>
        /// Time At given Point in Orbit
        /// </summary>
        public float TimeAtPointInOrbit { get; set; }
    }
}