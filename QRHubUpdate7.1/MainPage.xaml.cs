using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace QRHubUpdate7._1
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhoneNumberChooserTask phoneTask;
        string name;
        string number;
        BitmapImage bitmap = new BitmapImage();
        BitmapImage bitmap2 = new BitmapImage();
        ProgressBar bar = new ProgressBar();
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            bar.IsIndeterminate = true;
            this.ContentPanel.Children.Add(bar);
            string temp;
            try
            {
                temp = textBox1.Text;
            }
            catch (InvalidOperationException ex) 
            {
                temp = textBox1.Text;
            }
            WebClient webClientImgDownloader = new WebClient();
            webClientImgDownloader.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientImgDownloader_OpenReadCompleted);
            webClientImgDownloader.OpenReadAsync(new Uri("https://chart.googleapis.com/chart?chs=500x500&cht=qr&chl=" + temp, UriKind.Absolute));
        }
        void webClientImgDownloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                bitmap.SetSource(e.Result);
                image1.Source = bitmap;
                this.ContentPanel.Children.Remove(bar);
            }
            catch (WebException e2)
            {
                MessageBox.Show("No internet connection detected");
                this.ContentPanel.Children.Remove(bar);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            String tempJPEG = "TempJPEG";

            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (myStore.FileExists(tempJPEG))
            {
                myStore.DeleteFile(tempJPEG);
            }

            IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);
            try
            {
                WriteableBitmap wb = new WriteableBitmap(bitmap);
                wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
            }
            catch (NullReferenceException e4)
            {
                MessageBox.Show("No QR image to save. Create QR first.");
            }


            myFileStream.Close();

            // Create a new stream from isolated storage, and save the JPEG file to the media library on Windows Phone.
            myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);

            // Save the image to the camera roll or saved pictures album.
            MediaLibrary library = new MediaLibrary();
            try
            {
                Picture pic = library.SavePicture("SavedPicture.jpg", myFileStream);
                MessageBox.Show("Image saved to Saved Pictures album");
            }
            catch (ArgumentException e5)
            {
                MessageBox.Show("Please create QR then save it");
            }

            myFileStream.Close();   
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            phoneTask = new PhoneNumberChooserTask();
            phoneTask.Completed += new EventHandler<PhoneNumberResult>(phoneTask_Completed);
            phoneTask.Show();
        }

        void phoneTask_Completed(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                name = e.DisplayName;
                number = e.PhoneNumber;
            }
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {

            bar.IsIndeterminate = true;
            this.ContentPanel2.Children.Add(bar);
            WebClient webClientImgDownloader = new WebClient();
            if (name == null || number == null)
            {
                MessageBox.Show("Choose contact first then QR-ify");
                this.ContentPanel2.Children.Remove(bar);
            }
            else
            {
                webClientImgDownloader.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientImgDownloader2_OpenReadCompleted);
                webClientImgDownloader.OpenReadAsync(new Uri("https://chart.googleapis.com/chart?chs=500x500&cht=qr&chl=" + name + " " + number, UriKind.Absolute));
            }

        }

        void webClientImgDownloader2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                bitmap2.SetSource(e.Result);
                image12.Source = bitmap2;
                this.ContentPanel2.Children.Remove(bar);
            }
            catch (WebException e3)
            {
                MessageBox.Show("No internet connection detected");
                this.ContentPanel2.Children.Remove(bar);
            }
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            String tempJPEG = "TempJPEG";

            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (myStore.FileExists(tempJPEG))
            {
                myStore.DeleteFile(tempJPEG);
            }

            IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);
            try
            {
                WriteableBitmap wb = new WriteableBitmap(bitmap2);
                wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
            }
            catch (NullReferenceException e4)
            {
                MessageBox.Show("No QR image to save. Create QR first.");
            }


            myFileStream.Close();

            // Create a new stream from isolated storage, and save the JPEG file to the media library on Windows Phone.
            myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);

            // Save the image to the camera roll or saved pictures album.
            MediaLibrary library = new MediaLibrary();
            try
            {
                Picture pic = library.SavePicture("SavedPicture.jpg", myFileStream);
                MessageBox.Show("Image saved to Saved Pictures album");
            }
            catch (ArgumentException e5)
            {
                MessageBox.Show("Please create QR then save it");
            }

            myFileStream.Close();
        }
    }
}