using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace BestandsArchivering
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectSource_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Selecteer de bronmap";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSourcePath.Text = dialog.SelectedPath;
            }
        }

        private void BtnSelectTarget_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Selecteer de doelmap";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtTargetPath.Text = dialog.SelectedPath;
            }
        }

        private void BtnMoveFiles_Click(object sender, RoutedEventArgs e)
        {
            lstLogs.Items.Clear();

            string sourcePath = txtSourcePath.Text;
            string targetPath = txtTargetPath.Text;
            DateTime? selectedDate = dpCutoffDate.SelectedDate;

            if (!Directory.Exists(sourcePath))
            {
                lstLogs.Items.Add("Bronmap bestaat niet.");
                return;
            }

            if (!Directory.Exists(targetPath))
            {
                lstLogs.Items.Add("Doelmap bestaat niet.");
                return;
            }

            if (selectedDate == null)
            {
                lstLogs.Items.Add("Selecteer een geldige datum.");
                return;
            }

            DateTime cutoffDate = selectedDate.Value;

            foreach (string filePath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                DateTime fileDate = File.GetLastWriteTime(filePath);

                if (fileDate < cutoffDate)
                {
                    string relativePath = Path.GetRelativePath(sourcePath, filePath);
                    string destinationPath = Path.Combine(targetPath, relativePath);
                    string destinationDir = Path.GetDirectoryName(destinationPath);

                    try
                    {
                        if (!Directory.Exists(destinationDir))
                        {
                            Directory.CreateDirectory(destinationDir);
                        }

                        File.Move(filePath, destinationPath);
                        lstLogs.Items.Add($"Verplaatst: {relativePath}");
                    }
                    catch (Exception ex)
                    {
                        lstLogs.Items.Add($"FOUT bij {relativePath}: {ex.Message}");
                    }
                }
            }

            lstLogs.Items.Add("Verplaatsen voltooid.");
        }
    }
}
