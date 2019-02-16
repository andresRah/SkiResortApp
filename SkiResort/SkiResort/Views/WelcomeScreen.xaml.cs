namespace SkiResort.Views
{
    using System;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomeScreen : ContentPage
    {
        /// <summary>
        /// The cont.
        /// </summary>
        public int cont;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SkiResort.Views.WelcomeScreen"/> class.
        /// </summary>
        public WelcomeScreen()
        {
            InitializeComponent();
            cont = 0;
        }

        /// <summary>
        /// Ons the chart tap gesture tap.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        async void OnChartTapGestureTap(object sender, EventArgs args)
        {
            uint transitionTime = 145;
            double displacement = image.Height;

            var effect = Easing.BounceIn;
            var effect2 = Easing.BounceOut;
            var sideEffect = image.Y;

            switch (cont) 
            {
                case 0:
                    image.Source = ImageSource.FromFile("screenTwoE.jpeg");
                    effect = Easing.CubicIn;
                    effect2 = Easing.CubicInOut;
                    cont = 1;
                    break;
                case 1:
                    image.Source = ImageSource.FromFile("screenThreeE.jpeg");
                    effect = Easing.SinIn;
                    effect2 = Easing.SinInOut;
                    cont = 2;
                    break;
                case 2:
                    image.Source = ImageSource.FromFile("screenFourE.jpeg");
                    effect = Easing.Linear;
                    effect2 = Easing.Linear;
                    cont = 3;
                    break;
                case 3:
                    image.Source = ImageSource.FromFile("screenOneE.jpeg");
                    effect = Easing.Linear;
                    effect2 = Easing.Linear;
                    cont = 0;
                    break;
            }

            await image.TranslateTo(displacement, 0, 0);
            await Task.WhenAll(
                image.FadeTo(1, transitionTime, effect),
                image.TranslateTo(0, image.X, transitionTime, effect2));
        }

        /// <summary>
        /// Handles the clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TestSkiPage());
        }
    }

}
