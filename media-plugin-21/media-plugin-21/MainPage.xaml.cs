using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.RichTextEditor;
using Xamarin.Forms;

namespace media_plugin_21
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            _settings = new StoreCameraMediaOptions
            {
                MaxWidthHeight = 100,
                PhotoSize = PhotoSize.Custom,
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

            if (file == null)
                return;
            var fi = new FileInfo(file.Path);
            e.Accept(RichTextImageSource.FromStream(file.GetStream(), RichTextImageType.Jpeg));
        }
    }
    class Bindings : INotifyPropertyChanged
    {
        public Bindings()
        {
            OpenImageEditorCommand = new Command(OnOpenImageEditor);
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
}
