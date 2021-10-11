using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace media_plugin_21
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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

        private void OnPickImage(object sender, Telerik.XamarinForms.RichTextEditor.PickImageEventArgs e)
        {
        }
    }
}
