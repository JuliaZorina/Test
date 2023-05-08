using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string folderName;
        private List<String> wordsForText;

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            // если папка выбрана и нажата клавиша `OK` - значит можно получить путь к папке
            if (result == DialogResult.OK)
            {
                // запишем в нашу переменную путь к папке
                folderName = folderBrowserDialog1.SelectedPath;
                folderNameTB.Text = folderName;
            }
        }

        //нужно создать n файлов, filesNumberNumericUpDown.text, в цикле создаем файлы с именем i.txt
        //где i - счетчик
        public void GenerateFiles()
        {
            int filesNumber = Convert.ToInt32(filesNumberNumericUpDown.Value);
            for(int i = 0; i < filesNumber; i++)
            {
                string filePath = string.Format("{0}/{1}.txt", folderName, i);

                using (StreamWriter fileStream = File.Exists(filePath) ? File.CreateText(filePath) : File.CreateText(filePath))
                {
                    
                    fileStream.WriteLine(GenerateText());
                }
            }
        }
        //из wordsForText нужно брать рандомные слова и рандомно разделять их пробелами и знаками препинания
        public string GenerateText()
        {
            var separators = new List<String>
            {
                " ", "," ,"." ,"!" ,"?" ,"-", ";", ":"
            };
            int minWords = Convert.ToInt32(minWordsNumericUpDown.Value);
            int maxWords = Convert.ToInt32(maxWordsNumericUpDown.Value);
            var random = new Random();
            int wordsNumber = random.Next(minWords, maxWords);
            string text = null;
            for(int i = 0; i < wordsNumber; i++)
            {
                int wordIndex = random.Next(0,wordsForText.Count);
                text += wordsForText[wordIndex];
                int separatorIndex = random.Next(0, separators.Count);
                text += separators[separatorIndex];
            }

            return text;
        }

        public List<String> ReadFile()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;//path to exe file
            var path = Path.Combine(exePath, "Text\\RUS.txt");
            List<String> slova = new List<string>();
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                while (!sr.EndOfStream)
                    slova.Add(sr.ReadLine());
            }

            return slova;
        }
        //нужно один раз считать файл и выбрать из него слова
        //считывание делаем, если все поля заполнены и нет ошибок и если файл еще не считан,
        //т.е. если переменная со словами не пустая
        private void acceptButton_Click(object sender, EventArgs e)
        {
            //проверить корректность заполненности полей
            //поле пути не может быть пустым или ест=ли путь введен неверно вывести уведомление об этом
            //генерировать должны хотя бы 1 файл
            //минимальное количество слов не должно превышать максимальное количество слов
            

            if (string.IsNullOrEmpty(folderNameTB.Text))
            {
                MessageBox.Show("Укажите папку для генерации файлов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(Convert.ToInt32(minWordsNumericUpDown.Value) > Convert.ToInt32(maxWordsNumericUpDown.Value))
            {
                MessageBox.Show("Минимальное количество слов не может превышать максимальное количество слов в тексте",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Convert.ToInt32(filesNumberNumericUpDown.Value) <=0)
            {
                MessageBox.Show("Нужно сгенерировать хотя бы 1 файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    if (wordsForText == null)
                    {
                        wordsForText = ReadFile();
                    }
                    GenerateFiles();
                    DialogResult result = MessageBox.Show("Файлы успешно созданы", "Успех!", MessageBoxButtons.OK);
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.ToString(),"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            
        }
    }
}
