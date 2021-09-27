using System.Collections.Generic;
using AshtonBro.Layout.Examples;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AshtonBro.Layout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly List<Example> Examples = new List<Example>
        {
            new Example
                {
                    Title = "Canvas",
                    Demo = new CanvasExample()
                },
            new Example
                {
                    Title = "Grid",
                    Demo = new GridExample()
                },
            new Example
                {
                    Title = "StackPanel",
                    Demo = new StackPanelExample()
                },
            new Example
                {
                    Title = "ItemsControl (WrapGrid)",
                    Demo = new WrapGridExample()
                },
            new Example
                {
                    Title = "ItemsControl (VariableSizedWrapGrid)",
                    Demo = new VariableSizedWrapGridExample()
                },
            new Example
                {
                    Title = "ScrollViewer",
                    Demo = new ScrollViewerExample()
                },
            new Example
                {
                    Title = "ViewBox",
                    Demo = new ViewBoxExample()
                }
        };

        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}