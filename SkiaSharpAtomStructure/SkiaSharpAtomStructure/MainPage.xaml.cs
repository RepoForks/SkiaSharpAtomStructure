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
        public MainPage()
        {
            InitializeComponent();
        }

        private void AtomSilhouettePageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AtomSilhouettePage());
        }

        private void AtomOrbitalPathsPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AtomOrbitalPathsPage());
        }

        private void AtomAnimatedPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AtomAnimatedPage());
        }
    }
}