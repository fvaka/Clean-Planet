using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clean_Planet
{
    /// <summary>
    /// Логика взаимодействия для history.xaml
    /// </summary>
    public partial class history : Window
    {
        private readonly AppDBContext db = new AppDBContext();
        private MainWindow _mainWindow;
        public history(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadPartners();
            cmbPartners.SelectedIndex = 1;
        }

        private void LoadPartners()
        {
            try
            {
                using (var db = new AppDBContext())
                {
                    var partners = db.partners
                        .OrderBy(t => t.Наименование_партнера)
                        .Select(p => new
                        {
                            id = p.ID,
                            partner_name = p.Наименование_партнера
                        })
                        .ToList();

                    cmbPartners.DisplayMemberPath = "partner_name";
                    cmbPartners.SelectedValuePath = "id";
                    cmbPartners.ItemsSource = partners;
                    Console.WriteLine($"Загружено {partners.Count} партнеров в ComboBox");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке партнеров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadHistory(int partnerId)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке истории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при возврате к главному окну: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPartners.SelectedValue != null)
            {
                int partnerId = (int)cmbPartners.SelectedValue;
                LoadHistory(partnerId);
            }
        }

        private void CmbPartners_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPartners.SelectedValue != null)
            {
                int partnerId = (int)cmbPartners.SelectedValue;
                LoadHistory(partnerId);
            }
        }
    }
}
