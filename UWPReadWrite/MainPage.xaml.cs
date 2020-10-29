using Newtonsoft.Json;
using SharedLibrary;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPReadWrite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        string filepath = @"C:\Users\Matti\AppData\Local\Packages\fe89e871-a9c1-4f9a-a603-cc3e693cc971_5xnsqkv93bhft\LocalState\";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<string> CsvRows = new ObservableCollection<string>();
        private async void ReadCsv_Click(object sender, RoutedEventArgs e)
        {

            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".csv");

            StorageFile file = await picker.PickSingleFileAsync();

            CsvRows.Clear();

            using (CsvParse.CsvFileReader csvReader = new CsvParse.CsvFileReader(await file.OpenStreamForReadAsync()))
            {
                CsvParse.CsvRow row = new CsvParse.CsvRow();
                while (csvReader.ReadRow(row))
                {
                    string newRow = null;

                    for (int i = 0; i < row.Count; i++)
                    {
                        newRow += row[i] + "";
                    }
                    string print = newRow.Replace(";", " ");
                    CsvRows.Add(print);
                }

                LVCSV.ItemsSource = CsvRows;
            }
        }

        private async void ReadJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".json");
                StorageFile file = await picker.PickSingleFileAsync();


                if (file != null)
                {
                    string text = await FileIO.ReadTextAsync(file);
                    List<Person> DeserializedProducts = JsonConvert.DeserializeObject<List<Person>>(text);
                    LVJson.ItemsSource = DeserializedProducts;
                }
            }
            catch { }
        }

        private async void ReadXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".xml");
                StorageFile file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    var stream = await file.OpenAsync(FileAccessMode.Read);
                    using (StreamReader reader = new StreamReader(stream.AsStream()))
                    {
                        using XmlTextReader xml = new XmlTextReader(reader);
                        xml.Read();

                        while (xml.Read())
                        {
                            XmlNodeType ntype = xml.NodeType;

                            if (ntype == XmlNodeType.Element)
                            {
                                if (xml.Name == "person")
                                {
                                    TBXml.Text = xml.GetAttribute("fullinfo");
                                }
                            }
                        }

                    }
                }
            }
            catch { }
        }

        private async void Readtxt_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker txt = new FileOpenPicker();
            txt.ViewMode = PickerViewMode.Thumbnail;
            txt.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            txt.FileTypeFilter.Add(".txt");

            StorageFile file = await txt.PickSingleFileAsync();

            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                using (StreamReader reader = new StreamReader(stream.AsStream()))
                {
                    TBTxt.Text = reader.ReadToEnd();
                }

            }
        }

        private void Addtxt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadTextBoxes();
                AddTextServices.AddTextToTxt(Path.Combine(filepath, "persons.txt"),  ReadTextBoxes());
            }
            catch { }
            finally
            {
                ClearAllTextFields();
            }
        }

        private void AddJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Person json = new Person
                {
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    Age = tbAge.Text,
                    City = tbCity.Text
                };
                AddTextServices.AddTextToJson(Path.Combine(filepath, "persons.json"), json);
            }
            catch { }
            finally
            {
                ClearAllTextFields();
            }
        }

        private void AddCsv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                AddTextServices.AddTextToCsv(Path.Combine(filepath, "persons.csv"), ReadTextBoxes());
            }
            catch { }
            finally
            {
                ClearAllTextFields();
            }
        }

        private void AddXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddTextServices.WriteToFileXMLTest(Path.Combine(filepath, "persons.xml"), tbFirstName.Text, tbLastName.Text, tbAge.Text, tbCity.Text);
            }
            catch { }
            finally
            {
                ClearAllTextFields();
            }

        }

        
        private void ClearAllTextFields()
        {
            tbFirstName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            tbAge.Text = string.Empty;
            tbCity.Text = string.Empty;
        }

        private string ReadTextBoxes ()
        {
            string package = $"{tbFirstName.Text} {tbLastName.Text} {tbAge.Text} {tbCity.Text}";
            return package;
        }

        private async Task <StorageFile>PickaFileToRead()                           //Används ej
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".csv");
            StorageFile file = await picker.PickSingleFileAsync();
            return file;
        }

       








    }


}
