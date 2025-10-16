using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace WindowsApplication;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void HashFileButton_Click(object sender, RoutedEventArgs e)
    {
        // Open file dialog to select a file
        var openFileDialog = new OpenFileDialog
        {
            Title = "Select a file to hash",
            Filter = "All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        string inputFilePath = openFileDialog.FileName;
        string fileName = Path.GetFileName(inputFilePath);
        
        try
        {
            // Hash the file
            string hash = await ComputeFileHashAsync(inputFilePath);

            // Prompt for save location
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save hash file",
                FileName = $"{fileName}.hashed",
                Filter = "Hash files (*.hashed)|*.hashed|All files (*.*)|*.*",
                DefaultExt = ".hashed"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            // Save the hash to file
            await File.WriteAllTextAsync(saveFileDialog.FileName, hash);

            MessageBox.Show($"Hash saved successfully!\n\nSHA-256: {hash}", 
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", 
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private static async Task<string> ComputeFileHashAsync(string filePath)
    {
        using var sha256 = SHA256.Create();
        await using var stream = File.OpenRead(filePath);
        byte[] hashBytes = await sha256.ComputeHashAsync(stream);
        
        // Convert to hexadecimal string
        var sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }
        
        return sb.ToString();
    }
}