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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clean_Planet
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDBContext dBContext = new AppDBContext();
        public MainWindow()
        {
            InitializeComponent();
            LoadPartners();
        }

        public void LoadPartners()
        {
            try
            {
                using (var db = new AppDBContext())
                {
                    stackPanel1.Children.Clear();
                    var partners = db.partners.ToList();
                    foreach (var partner in partners)
                    {
                        try
                        {

                            string partnerType = partner.Тип_партнера ?? "Неизвестный тип";
                            PartnerControl pc = new PartnerControl(this);
                            pc.DataSet(
                                partnerType,
                                partner.Наименование_партнера,
                                partner.Руководитель,
                                partner.Телефон_партнера,
                                partner.Рейтинг.ToString(),
                                partner.ID
                            );
                            stackPanel1.Children.Add(pc);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show($"Ошибка при загрузке партнера {partner}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Произошла ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updateForm = new addPartner(this);
                updateForm.Closed += (s, args) => this.LoadPartners();
                updateForm.Show();
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.Hide();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", "Ошибка");
            }
        }

        private void history_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updateForm = new history(this);
                updateForm.Closed += (s, args) => this.LoadPartners();
                updateForm.Show();
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.Hide();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", "Ошибка");
            }
        }
    }
}
