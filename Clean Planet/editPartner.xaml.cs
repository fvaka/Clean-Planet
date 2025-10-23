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
    /// Логика взаимодействия для editPartner.xaml
    /// </summary>
    public partial class editPartner : Window
    {
        private readonly int _partnerID;
        private readonly MainWindow _mainWindow;

        public editPartner(int partnerID, MainWindow mainWindow)
        {
            InitializeComponent();
            _partnerID = partnerID;
            _mainWindow = mainWindow;
            LoadPartnerTypes();
            LoadPartnerData();
        }

        private void LoadPartnerTypes()
        {
            try
            {
                using (var db = new AppDBContext())
                {
                    var partnerTypes = db.partners
                        .Where(p => p.Тип_партнера != null)
                        .Select(p => p.Тип_партнера)
                        .Distinct()
                        .OrderBy(t => t)
                        .ToList();

                    typeCMB.ItemsSource = partnerTypes;
                    typeCMB.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов партнеров: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartnerData()
        {
            try
            {
                using (var db = new AppDBContext())
                {
                    var partner = db.partners
                        .FirstOrDefault(p => p.ID == _partnerID);

                    if (partner == null)
                    {
                        MessageBox.Show("Партнер не найден!", "Ошибка",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                        return;
                    }

                    nameTB.Text = partner.Наименование_партнера;
                    typeCMB.SelectedItem = partner.Тип_партнера;
                    ratingTB.Text = partner.Рейтинг.ToString();
                    addressTB.Text = partner.Юридический_адрес_партнера;
                    FIODTb.Text = partner.Руководитель;
                    phoneTB.Text = partner.Телефон_партнера;
                    emailTb.Text = partner.Электронная_почта_партнера;
                    InnTB.Text = partner.ИНН.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных партнера: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameTB.Text) ||
                    typeCMB.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(addressTB.Text) ||
                    string.IsNullOrWhiteSpace(phoneTB.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(ratingTB.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out double рейтинг) || рейтинг < 0)
                {
                    MessageBox.Show("Рейтинг должен быть неотрицательным числом!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new AppDBContext())
                {
                    var partner = db.partners
                        .FirstOrDefault(p => p.ID == _partnerID);

                    if (partner == null)
                    {
                        MessageBox.Show("Партнер не найден!", "Ошибка",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    partner.Наименование_партнера = nameTB.Text;
                    partner.Тип_партнера = (string)typeCMB.SelectedItem;
                    partner.Рейтинг = рейтинг;
                    partner.Юридический_адрес_партнера = addressTB.Text;
                    partner.Руководитель = FIODTb.Text;
                    partner.Телефон_партнера = phoneTB.Text;
                    partner.Электронная_почта_партнера = emailTb.Text;
                    partner.ИНН = Convert.ToDouble(InnTB.Text);

                    db.SaveChanges();
                }

                MessageBox.Show("Партнер успешно обновлен!", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                _mainWindow.LoadPartners();
                _mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Show();
            Close();
        }
    }
}
