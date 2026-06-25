using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using WpfZooManager.Models;

namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = _viewModel;
        }

        private void ZooList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("ZooList_SelectionChanged event triggered."+ ZooList.SelectedValue);
            if (ZooList.SelectedItem != null)
            {
                Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
                _viewModel.UpdateAnimalsList((int)selectedZoo.Id);
                AssociatedAnimalList.ItemsSource = _viewModel.AssociatedAnimalList;
                ZooLocationTextBox.Text = (string)selectedZoo.Location;
               // MessageBox.Show("ZooList_SelectionChanged event triggered. Selected Zoo ID: " + selectedZoo.Id+ ", Name: " + selectedZoo.Location);
            }
            else
            {
                // Clear the right box if nothing is selected
                AssociatedAnimalList.ItemsSource = null;
            }
        }

        private void AnimalListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnimalListBox.SelectedItem != null)
            {
                ZooLocationTextBox.Text = ((Animal)AnimalListBox.SelectedItem).Name;
            }

        }

        private void delete_zoo(object sender, RoutedEventArgs e)
        {

            Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
            _viewModel.DeleteFromDatabase<Zoo>(selectedZoo, _viewModel.ZooList);
            ZooList.ItemsSource = null;
            ZooList.ItemsSource = _viewModel.ZooList;
        }

        private void remove_animal(object sender, RoutedEventArgs e)
        {
            Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
            var AnimalID = AssociatedAnimalList.SelectedValue;

            if (selectedZoo != null && AnimalID != null)
            {
                
                int ZooID = (int)selectedZoo.Id;
                ZooAnimalMap? mappingToDelete = _viewModel.ZooAnimalMapList.FirstOrDefault(z => z.ZooId == ZooID && z.AnimalId == (int)AnimalID);
                if (mappingToDelete != null)
                {
                    _viewModel.DeleteFromDatabase<ZooAnimalMap>(mappingToDelete, _viewModel.ZooAnimalMapList);
                    _viewModel.UpdateAnimalsList(selectedZoo.Id);
                    AssociatedAnimalList.ItemsSource = _viewModel.AssociatedAnimalList;
                }
                else
                {
                    MessageBox.Show("No mapping found to delete.");
                }
            }
            else { MessageBox.Show("Please select both a zoo and an associated animal."); }

         }
        private void add_zoo(object sender, RoutedEventArgs e)
        {
            string ZooLocation = (string)ZooLocationTextBox.Text;
            if (ZooLocation != null)
            {

                Zoo newZoo = new Zoo { Location = ZooLocation };
                _viewModel.AddToDatabase<Zoo>(newZoo, _viewModel.ZooList);
                ZooList.ItemsSource = null;
                ZooList.ItemsSource = _viewModel.ZooList;
                ZooLocationTextBox.Text = null;
            }
            else
            {
                MessageBox.Show("Please enter a valid zoo location.");
            }
        }
        private void add_animal(object sender, RoutedEventArgs e)

        { 
            string AnimalName = (string)ZooLocationTextBox.Text;
            if (AnimalName != null)
            {

                Animal newAnimal = new Animal { Name = AnimalName };
                _viewModel.AddToDatabase<Animal>(newAnimal, _viewModel.AnimalList);
                AnimalListBox.ItemsSource = null;
                AnimalListBox.ItemsSource = _viewModel.AnimalList;
                ZooLocationTextBox.Text = null;
            }
            else
            {
                MessageBox.Show("Please enter a valid animal name.");
            }
        }       
           

        private void update_zoo(object sender, RoutedEventArgs e)
        {

            Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
            if (selectedZoo == null)
            {
                MessageBox.Show("Please select a zoo from the list to update.");
                return;
            }
            string newLocationName = (string)ZooLocationTextBox.Text;
            string oldLocationName = selectedZoo.Location;
            if (newLocationName != oldLocationName)
            {
                Zoo? ZooItem= _viewModel.ZooList.FirstOrDefault(z => z.Id == selectedZoo.Id);
                if (ZooItem != null)
                {
                    ZooItem.Location = newLocationName;
                    _viewModel.UpdateDatabase<Zoo>(ZooItem);
                    ZooList.ItemsSource = null;
                    ZooList.ItemsSource = _viewModel.ZooList;

                    // Clear input box
                    ZooLocationTextBox.Text = null;
                    MessageBox.Show("Zoo updated successfully!");
                }
                else
                {
                    MessageBox.Show("Selected zoo not found in the list.");
                }
            }
            else
            {
                MessageBox.Show("The new location name is the same as the old one. No update needed.");
            }

        }

        private void update_animal(object sender, RoutedEventArgs e)
        {
            Animal selectedAnimal = (Animal)AnimalListBox.SelectedItem;
            if (selectedAnimal == null)
            {
                MessageBox.Show("Please select an animal from the list to update.");
                return;
            }
            string newAnimalName = (string)ZooLocationTextBox.Text;
            string oldAnimalName = selectedAnimal.Name;
            if (newAnimalName != oldAnimalName)
            {
                Animal? AnimalItem = _viewModel.AnimalList.FirstOrDefault(a => a.Id == selectedAnimal.Id);
                if (AnimalItem != null)
                {
                    AnimalItem.Name = newAnimalName;
                    _viewModel.UpdateDatabase<Animal>(AnimalItem);
                    AnimalListBox.ItemsSource = null;
                    AnimalListBox.ItemsSource = _viewModel.AnimalList;
                    Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
                    ZooLocationTextBox.Text = null;
                    if (selectedZoo != null)
                    {
                        _viewModel.UpdateAnimalsList(selectedZoo.Id);
                        AssociatedAnimalList.ItemsSource = _viewModel.AssociatedAnimalList;
                    }
                    
                    MessageBox.Show("Animal updated successfully!");
                }
                else
                {
                    MessageBox.Show("Selected animal not found in the list.");
                }
            }
            else
            {
                MessageBox.Show("The new animal name is the same as the old one. No update needed.");
            }

        }
        private void add_animal_to_zoo(object sender, RoutedEventArgs e)
        {
            Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
            var AnimalID = AnimalListBox.SelectedValue;

            if (selectedZoo != null && AnimalID != null)
            {
                int ZooID = (int)selectedZoo.Id;
                ZooAnimalMap zooAnimalEntry = new ZooAnimalMap { AnimalId = (int)AnimalID, ZooId = ZooID };
                _viewModel.AddToDatabase<ZooAnimalMap>(zooAnimalEntry, _viewModel.ZooAnimalMapList);
                _viewModel.UpdateAnimalsList(selectedZoo.Id);
                AssociatedAnimalList.ItemsSource = _viewModel.AssociatedAnimalList;
            }

            else
            {
                MessageBox.Show("Please select both a zoo and an animal.");
            }

        }
        private void delete_animal(object sender, RoutedEventArgs e)
        {

            Animal selectedAnimal = (Animal)AnimalListBox.SelectedItem;
            Zoo selectedZoo = (Zoo)ZooList.SelectedItem;
            _viewModel.DeleteFromDatabase<Animal>(selectedAnimal, _viewModel.AnimalList);
            AnimalListBox.ItemsSource = null;
            AnimalListBox.ItemsSource = _viewModel.AnimalList;
            if (selectedZoo != null)
            {
                _viewModel.UpdateAnimalsList(selectedZoo.Id);
                AssociatedAnimalList.ItemsSource = _viewModel.AssociatedAnimalList;
            }

        }

        
    }
}