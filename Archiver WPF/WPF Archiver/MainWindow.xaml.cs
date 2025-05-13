using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Forms = System.Windows.Forms;

namespace BestandsArchivering
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dpCutoffDate.SelectedDate = DateTime.Today.AddYears(-2); // Standaardwaarde
        }

        private void BtnSelectSource_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    txtSourcePath.Text = dialog.SelectedPath;
                    lstLogs.Items.Add($"Bronmap geselecteerd: {dialog.SelectedPath}");
                }
            }
        }

        private void BtnSelectTarget_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    txtTargetPath.Text = dialog.SelectedPath;
                    lstLogs.Items.Add($"Doelmap geselecteerd: {dialog.SelectedPath}");
                }
            }
        }

        private void BtnMoveFiles_Click(object sender, RoutedEventArgs e)
        {
            if (dpCutoffDate.SelectedDate == null)
            {
                lstLogs.Items.Add("Selecteer een geldige datum!");
                return;
            }

            lstLogs.Items.Add("=== ARCHIVERING START ===");
            txtStatus.Text = "Bezig met verplaatsen...";
            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;

            string sourcePath = txtSourcePath.Text;
            string targetPath = txtTargetPath.Text;
            DateTime cutoffDate = dpCutoffDate.SelectedDate.Value;

            if (!ValidatePaths(sourcePath, targetPath)) return;

            try
            {
                string[] allFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
                ProcessFiles(allFiles, sourcePath, targetPath, cutoffDate);
            }
            catch (Exception ex)
            {
                lstLogs.Items.Add($"Ernstige fout: {ex.Message}");
                txtStatus.Text = "Ernstige fout opgetreden";
            }
            finally
            {
                progressBar.Visibility = Visibility.Collapsed;
                lstLogs.Items.Add("=== ARCHIVERING VOLTOOID ===");
                lstLogs.ScrollIntoView(lstLogs.Items[lstLogs.Items.Count - 1]);
            }
        }

        private bool ValidatePaths(string sourcePath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !Directory.Exists(sourcePath))
            {
                lstLogs.Items.Add("Fout: Selecteer een geldige bronmap!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(targetPath))
            {
                lstLogs.Items.Add("Fout: Selecteer een doelmap!");
                return false;
            }

            try
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                    lstLogs.Items.Add($"Doelmap aangemaakt: {targetPath}");
                }
                return true;
            }
            catch (Exception ex)
            {
                lstLogs.Items.Add($"Fout bij aanmaken doelmap: {ex.Message}");
                return false;
            }
        }

        private void ProcessFiles(string[] allFiles, string sourcePath, string targetPath, DateTime cutoffDate)
        {
            int totalFiles = allFiles.Length;
            int filesMoved = 0, filesSkipped = 0, errors = 0;

            for (int i = 0; i < totalFiles; i++)
            {
                string filePath = allFiles[i];
                try
                {
                    DateTime fileDate = File.GetLastWriteTime(filePath);

                    if (fileDate < cutoffDate)
                    {
                        string relativePath = Path.GetRelativePath(sourcePath, filePath);
                        string destinationPath = Path.Combine(targetPath, relativePath);
                        string destinationDir = Path.GetDirectoryName(destinationPath);

                        if (!Directory.Exists(destinationDir))
                        {
                            Directory.CreateDirectory(destinationDir);
                            lstLogs.Items.Add($"Map aangemaakt: {Path.GetDirectoryName(relativePath)}");
                        }

                        if (File.Exists(destinationPath))
                        {
                            lstLogs.Items.Add($"[OVERGESLAGEN] {relativePath} (bestaat al)");
                            filesSkipped++;
                            continue;
                        }

                        File.Move(filePath, destinationPath);
                        lstLogs.Items.Add($"[VERPLAATST] {relativePath}");
                        filesMoved++;
                    }
                }
                catch (Exception ex)
                {
                    lstLogs.Items.Add($"[FOUT] {Path.GetFileName(filePath)}: {ex.Message}");
                    errors++;
                }

                progressBar.Value = (i + 1) * 100 / totalFiles;
            }

            txtStatus.Text = $"Klaar - Verplaatst: {filesMoved}, Overgeslagen: {filesSkipped}, Fouten: {errors}";
            lstLogs.Items.Add($"Resultaat: {filesMoved} verplaatst, {filesSkipped} overgeslagen, {errors} fouten");
        }
    }
}