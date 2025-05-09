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
            txtStatus.Text = "Bezig met verplaatsen...";

            string sourcePath = txtSourcePath.Text;
            string targetPath = txtTargetPath.Text;
            DateTime cutoffDate = DateTime.Today.AddYears(-2); // Vaste cutoff van 2 jaar

            if (!Directory.Exists(sourcePath))
            {
                lstLogs.Items.Add("Bronmap bestaat niet.");
                txtStatus.Text = "Fout - bronmap bestaat niet";
                return;
            }

            if (!Directory.Exists(targetPath))
            {
                try
                {
                    Directory.CreateDirectory(targetPath);
                    lstLogs.Items.Add($"Doelmap aangemaakt: {targetPath}");
                }
                catch (Exception ex)
                {
                    lstLogs.Items.Add($"FOUT: Kon doelmap niet aanmaken: {ex.Message}");
                    txtStatus.Text = "Fout - kon doelmap niet aanmaken";
                    return;
                }
            }

            int filesMoved = 0;
            int filesSkipped = 0;
            int errors = 0;

            try
            {
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

                            if (File.Exists(destinationPath))
                            {
                                lstLogs.Items.Add($"Bestand bestaat al: {relativePath} - overgeslagen");
                                filesSkipped++;
                                continue;
                            }

                            File.Move(filePath, destinationPath);
                            lstLogs.Items.Add($"Verplaatst: {relativePath}");
                            filesMoved++;
                        }
                        catch (Exception ex)
                        {
                            lstLogs.Items.Add($"FOUT bij {relativePath}: {ex.Message}");
                            errors++;
                        }
                    }
                }

                lstLogs.Items.Add($"Klaar! Verplaatst: {filesMoved}, Overgeslagen: {filesSkipped}, Fouten: {errors}");
                txtStatus.Text = $"Klaar - Verplaatst: {filesMoved}, Overgeslagen: {filesSkipped}, Fouten: {errors}";
            }
            catch (Exception ex)
            {
                lstLogs.Items.Add($"Ernstige fout: {ex.Message}");
                txtStatus.Text = "Ernstige fout opgetreden";
            }
        }
    }
}