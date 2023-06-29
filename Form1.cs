using System.Text;

namespace CheckFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lstResults.SelectionMode = SelectionMode.MultiExtended;
            lstResults.KeyDown += ListBox1_KeyDown;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                txtFolder.Text = selectedPath;
                Console.WriteLine("Đường dẫn được chọn: " + selectedPath);
            }
        }

        private void ListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopySelectedItemsToClipboard(lstResults);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string folderPath = txtFolder.Text; // Thay đổi đường dẫn thư mục cần tìm kiếm
            string keywords = txtSearch.Text; // Thay đổi các từ khóa cần tìm kiếm, cách nhau bởi dấu cách

            List<string> matchingFiles = SearchFiles(folderPath, keywords);

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                MessageBox.Show("Select folder!");
            }

            if (string.IsNullOrWhiteSpace(keywords))
            {
                MessageBox.Show("Input keyword!");
            }

            string[] searchKeywords = keywords.Split(' ');

            if (matchingFiles.Count > 0)
            {
                lstResults.Items.Clear();
                Console.WriteLine("Matching files:");
                foreach (string file in matchingFiles)
                {
                    Console.WriteLine(file);
                    Console.WriteLine("Lines with matching keywords:");

                    string[] lines = File.ReadAllLines(file);

                    lstResults.Items.Add(file);
                    string headerDetail = $"Detail: ";
                    lstResults.Items.Add(headerDetail);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        foreach (string keyword in searchKeywords)
                        {
                            if (lines[i].Contains(keyword))
                            {
                                Console.WriteLine("Line {0}: {1}", i + 1, lines[i]);
                                string lineDetail = $"Line {i + 1}: {lines[i]}";
                                lstResults.Items.Add(lineDetail);

                                break;
                            }
                        }
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                lstResults.Items.Clear();
                MessageBox.Show("No matching files found.");
            }

            Console.ReadLine();
        }

        static List<string> SearchFiles(string folderPath, string keywords)
        {
            List<string> matchingFiles = new List<string>();

            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            string[] searchKeywords = keywords.Split(' ');

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                bool isMatch = false;

                string[] lines = File.ReadAllLines(file);

                for (int i = 0; i < lines.Length; i++)
                {
                    foreach (string keyword in searchKeywords)
                    {
                        if (lines[i].Contains(keyword))
                        {
                            isMatch = true;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        matchingFiles.Add(file);
                        break;
                    }
                }
            }

            return matchingFiles;
        }

        private void CopySelectedItemsToClipboard(ListBox listBox)
        {
            if (listBox.SelectedItems.Count > 0)
            {
                // Tạo một chuỗi để lưu trữ dữ liệu được chọn
                StringBuilder sb = new StringBuilder();

                // Lặp qua các mục được chọn và thêm chúng vào chuỗi
                foreach (var selectedItem in listBox.SelectedItems)
                {
                    sb.AppendLine(selectedItem.ToString());
                }

                // Sao chép chuỗi vào Clipboard
                Clipboard.SetText(sb.ToString());
            }
        }

        //private void copyButton_Click(object sender, EventArgs e)
        //{
        //    CopySelectedItemsToClipboard(lstResults);
        //}

    }
}