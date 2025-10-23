using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clean_Planet
{
    /// <summary>
    /// Логика взаимодействия для addPartner.xaml
    /// </summary>
    public partial class addPartner : Window
    {
        private AppDBContext dbContext = new AppDBContext();
        private readonly MainWindow _mainWindow;

        public addPartner(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadPartnerTypes();
            ClearFormFields();
        }

        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var main = new MainWindow();
                main.Closed += (s, args) => System.Windows.Application.Current.Shutdown();
                this.Close();
                main.Show();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void LoadPartnerTypes()
        {
            using (var db = new AppDBContext())
            {
                try
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
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Ошибка при загрузке типов партнеров: {ex.Message}");
                }
            }
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameTB.Text) ||
                    typeCMB.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(addressTB.Text) ||
                    string.IsNullOrWhiteSpace(phoneTB.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Заполните все обязательные поля!", "Ошибка");
                    return;
                }

                if (!int.TryParse(ratingTB.Text, out int рейтинг) || рейтинг < 0)
                {
                    System.Windows.Forms.MessageBox.Show("Рейтинг должен быть целым неотрицательным числом!", "Ошибка");
                    return;
                }

                var newPartner = new Partners_import
                {
                    Наименование_партнера = nameTB.Text,
                    Тип_партнера = (string)typeCMB.SelectedItem,
                    Рейтинг = рейтинг,
                    Юридический_адрес_партнера = addressTB.Text,
                    Руководитель = FIODTb.Text,
                    Телефон_партнера = phoneTB.Text,
                    Электронная_почта_партнера = emailTb.Text,
                    ИНН = Convert.ToDouble(InnTB.Text)
                };

                using (var db = new AppDBContext())
                {
                    db.partners.Add(newPartner);
                    db.SaveChanges();
                }

                System.Windows.Forms.MessageBox.Show("Партнер успешно добавлен!", "Успех");

                _mainWindow.LoadPartners();
                _mainWindow.Show();
                Close();
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Некорректный формат данных в числовых полях!", "Ошибка");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка");
            }
        }
        private void ClearFormFields()
        {
            nameTB.Text = string.Empty;
            typeCMB.SelectedIndex = -1;
            ratingTB.Text = string.Empty;
            addressTB.Text = string.Empty;
            FIODTb.Text = string.Empty;
            phoneTB.Text = string.Empty;
            emailTb.Text = string.Empty;
            InnTB.Text = string.Empty;
        }

    }
}
