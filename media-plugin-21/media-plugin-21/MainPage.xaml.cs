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
}
