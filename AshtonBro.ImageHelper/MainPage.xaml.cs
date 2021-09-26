using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Graphics.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.Activation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AshtonBro.ImageHelper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap _writeableBitmap;
        private ShareOperation _shareOperation;
        private RandomAccessStreamReference _bitmap;
        private IReadOnlyList<IStorageItem> _items;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_writeableBitmap != null)
            {
                var picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeChoices.Add("Image", new List<string>() { ".png" });
                picker.DefaultFileExtension = ".png";
                picker.SuggestedFileName = "photo";

                var saveFile = await picker.PickSaveFileAsync();

                try
                {
                    if (saveFile != null)
                    {
                        IRandomAccessStream output = await saveFile.OpenAsync(FileAccessMode.ReadWrite);

                        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, output);
                        encoder.SetPixelData(
                            BitmapPixelFormat.Rgba8,
                            BitmapAlphaMode.Straight,
                            (uint)_writeableBitmap.PixelWidth,
                            (uint)_writeableBitmap.PixelHeight,
                            96.0, 96.0,
                            _writeableBitmap.PixelBuffer.ToArray());

                        await encoder.FlushAsync();
                        await output.GetOutputStreamAt(0).FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    var s = ex.ToString();
                }
            }
        }

        private async void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();

            var result = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (result != null)
            {
                await LoadBitmap(await result.OpenAsync(FileAccessMode.Read));
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var args = e.Parameter as ShareTargetActivatedEventArgs;

            if (args != null)
            {
                _shareOperation = args.ShareOperation;

                if (_shareOperation.Data.Contains(StandardDataFormats.Bitmap))
                {
                    _bitmap = await _shareOperation.Data.GetBitmapAsync();

                    await ProcessBitmap();
                }
                else if (_shareOperation.Data.Contains(StandardDataFormats.StorageItems))
                {
                    _items = await _shareOperation.Data.GetStorageItemsAsync();

                    await ProcessStorageItems();
                }
                else
                {
                    _shareOperation.ReportError("Image Helper was unable to find a valid bitmap.");
                }
            }
        }

        private async Task LoadBitmap(IRandomAccessStream stream)
        {
            _writeableBitmap = new WriteableBitmap(1, 1);
            _writeableBitmap.SetSource(stream);
            _writeableBitmap.Invalidate();

            await Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => ImageTarget.Source = _writeableBitmap);
        }

        private async Task ProcessBitmap()
        {
            if (_bitmap != null)
            {
                await LoadBitmap(await _bitmap.OpenReadAsync());
            } 
        }

        private async Task ProcessStorageItems()
        {
            foreach (var item in _items)
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    var file = item as StorageFile;

                    if (file.ContentType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
                    {
                        await LoadBitmap(await file.OpenReadAsync());
                        break;
                    }
                }
            }
        }

    }
}
