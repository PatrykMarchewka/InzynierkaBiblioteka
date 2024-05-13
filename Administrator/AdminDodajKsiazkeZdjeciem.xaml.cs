using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using ZXing;
using ZXing.Windows.Compatibility;

namespace InżynierkaBiblioteka
{
    /// <summary>
    /// Interaction logic for AdminDodajKsiazkeZdjeciem.xaml
    /// </summary>
    public partial class AdminDodajKsiazkeZdjeciem : Page
    {
        MediaCapture mediaCapture;
        BarcodeReader barcodeReader = new BarcodeReader();
        static BitmapImage bitmapImage;
        private static bool skan = false;
        public AdminDodajKsiazkeZdjeciem()
        {
            InitializeComponent();
            skan = false;
            barcodeReader.Options.TryHarder = true;
            barcodeReader.Options.TryInverted = true;



            //var writer = new BarcodeWriter();
            //writer.Format = BarcodeFormat.EAN_13;
            //var bp = writer.Write("978020137962");
            //bp.Save(@"C:\Users\Patryk\OneDrive\Pulpit\BEEPEE.png");




            //Bitmap bitmapa = new Bitmap(@"C:\Users\Patryk\OneDrive\Pulpit\sk2.png");

            //barcodeReader.Options.TryInverted = true;
            //Result result = barcodeReader.Decode(bitmapa);
            //MessageBox.Show(result.Text);

            InitializeCameraAsync();

        }

        async void InitializeCameraAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            
            //mediaCapture = new MediaCapture();
            //await mediaCapture.InitializeAsync();
            while (!skan)
            {
                Bitmap bitmapa = await CapturePhotoAsync();
                imObraz.Source = bitmapImage;
                var result = barcodeReader.Decode(bitmapa);
                if (result != null)
                {
                    MessageBox.Show(result.Text);
                    skan = true;
                }
                else
                {
                    Console.WriteLine("Nieznaleziono dla zdjecia");
                }
            }
        }



        //async Task CaptureAndDecodeBarcode()
        //{
        //    var myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        //    StorageFile file = await myPictures.SaveFolder.CreateFileAsync("photo.jpg", CreationCollisionOption.GenerateUniqueName);
        //    System.Drawing.Bitmap bitmap;

        //    var captureStream = new InMemoryRandomAccessStream();
        //    await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);

        //    using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //    {
        //        var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(captureStream);
        //        var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);

        //        var properties = new BitmapPropertySet {
        //    { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.Normal, PropertyType.UInt16) }
        //};
        //        await encoder.BitmapProperties.SetPropertiesAsync(properties);

        //        await encoder.FlushAsync();
        //    }

        //    captureStream.Dispose();

        //    // Convert the StorageFile to a System.Drawing.Bitmap
        //    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
        //    {
        //        BitmapImage bitmapImage = new BitmapImage();
        //        await bitmapImage.SetSourceAsync(fileStream);

        //        // Convert the BitmapImage to a System.Drawing.Bitmap
        //        using (MemoryStream outStream = new MemoryStream())
        //        {
        //            BitmapEncoder enc = BitmapEncoder.Create(BitmapEncoder.PngEncoderId);
        //            enc.SetSoftwareBitmap(SoftwareBitmap.CreateCopyFromBuffer(bitmapImage.PixelBuffer, BitmapPixelFormat.Bgra8, bitmapImage.PixelWidth, bitmapImage.PixelHeight, BitmapAlphaMode.Premultiplied));
        //            await enc.FlushAsync();

        //            outStream.Seek(0, SeekOrigin.Begin);
        //            bitmap = new System.Drawing.Bitmap(outStream);
        //        }
        //    }

        //    var result = barcodeReader.Decode(bitmap);
        //    if (result != null)
        //    {
        //        MessageBox.Show(result.Text);
        //    }
        //    else
        //    {

        //    }
        //}




        public static async Task<Bitmap> CapturePhotoAsync()
        {
            // Create MediaCapture object
            MediaCapture mediaCapture = new MediaCapture();

            // Initialize MediaCapture
            await mediaCapture.InitializeAsync();


            //Proba autofokusa
            if (mediaCapture.VideoDeviceController.FocusControl.Supported)
            {
                var focusSettings = new FocusSettings { Mode = FocusMode.Auto };
                mediaCapture.VideoDeviceController.FocusControl.Configure(focusSettings);

                await mediaCapture.VideoDeviceController.FocusControl.FocusAsync();
            }



            // Set the format for capturing photos
            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreatePng();

            // Create a storage file for the photo
            var file = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("temp.png", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Capture the photo
            await mediaCapture.CapturePhotoToStorageFileAsync(imgFormat, file);

            // Convert the captured photo to BitmapImage
            using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream.AsStream();
                bitmapImage.EndInit();


                // Convert BitmapImage to Bitmap
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    System.Windows.Media.Imaging.BitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapImage));
                    encoder.Save(memoryStream);

                    Bitmap bitmap = new Bitmap(memoryStream);
                    mediaCapture.Dispose();
                    return bitmap;
                }
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            skan = true;
            MainWindow.GlownaRamka.GoBack();
        }





        //Dla czytnika laserowego
        //private static void CzytanieZCzytnika(object sender, KeyEventArgs e)
        //{
        //    if (e.Key >= Key.D0 && e.Key <= Key.D9)
        //    {
        //        Kod += (e.Key - Key.D0).ToString();
        //    }
        //    else if (e.Key == Key.Enter)
        //    {
        //        //Tutaj przejsc do strony Ksiazki
        //        //
        //        Kod = String.Empty;
        //    }
        //}


    }
}
