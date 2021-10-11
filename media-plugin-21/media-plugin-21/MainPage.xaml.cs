using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.RichTextEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace media_plugin_21
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            _settings = new StoreCameraMediaOptions
            {
                MaxWidthHeight = 150,
                PhotoSize = PhotoSize.MaxWidthHeight,
                Name = "thumbnail.jpg",
            };
        }

        /// <summary>
        /// WORKS Programmatically focus the Entry control
        /// </summary>
        private async void OnFocusChangedEntry(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<int> e)
        {
            if (e.NewValue != -1)
            {
                var segctl = (RadSegmentedControl)sender;
                switch (segctl.ItemsSource.Cast<string>().ToArray()[e.NewValue])
                {
                    case "Focus":
                        entry.Focus();
                        break;
                    case "Unfocus":
                        entry.Unfocus();
                        break;
                }
                await Task.Delay(250);
                segctl.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// DFW - Doesn't Factually Work
        /// </summary>
        private async void OnFocusChangedRRTE(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<int> e)
        {            
            if(e.NewValue != -1)
            {
                var segctl = (RadSegmentedControl)sender;
                switch(segctl.ItemsSource.Cast<string>().ToArray()[e.NewValue])
                {
                    case "Focus":
                        richTextEditor.Focus();
                        break;
                    case "Unfocus":
                        richTextEditor.Unfocus();
                        break;
                }
                await Task.Delay(250);
                segctl.SelectedIndex = -1;
            }
        }

        readonly StoreCameraMediaOptions _settings;
        private async void OnPickImage(object sender, PickImageEventArgs e)
        {
            await CrossMedia.Current.Initialize();
            var file = await CrossMedia.Current.TakePhotoAsync(_settings);

            if (file == null) return;           // Cancelled
            var fi = new FileInfo(file.Path);   // Verify that Media Plugin reduces image size

            e.Accept(RichTextDataImageSource.FromFile(file.AlbumPath, RichTextImageType.Jpeg));
        }
    }
    class Bindings : INotifyPropertyChanged
    {
        public Bindings()
        {
            OpenImageEditorCommand = new Command(OnOpenImageEditor);
            // Requires an image to enable toolbar
            Image = ImageSource.FromResource("media_plugin_21.res.placeholder.png", GetType().Assembly);
        }
        bool _IsImageEditorInvoked = false;
        public bool IsImageEditorInvoked
        {
            get => _IsImageEditorInvoked;
            set
            {
                if(_IsImageEditorInvoked != value)
                {
                    _IsImageEditorInvoked = value;
                    OnPropertyChanged();
                }
            }
        }

        ImageSource _Image = null;
        public ImageSource Image
        {
            get => _Image;
            set
            {
                if((_Image == null) || !_Image.Equals(value))
                {
                    _Image = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenImageEditorCommand { get; }
        private void OnOpenImageEditor(object o)
        {
            IsImageEditorInvoked = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
    class CustomToolbarItem : RichTextEditorToolbarItem { }
    class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ? false : !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ContentProperty(nameof(Source))]
    class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            return imageSource;
        }
    }
}
