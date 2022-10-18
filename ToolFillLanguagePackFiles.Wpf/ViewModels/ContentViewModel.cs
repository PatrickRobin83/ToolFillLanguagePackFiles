/*
*----------------------------------------------------------------------------------
*          Filename:	ContentViewModel.cs
*          Date:        2022.10.17
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;


namespace ToolFillLanguagePackFiles.Wpf.ViewModels;
[ObservableObject]
public partial class ContentViewModel
{
    #region Fields



    #endregion

    #region Properties
    [ObservableProperty]
    private bool _progressVisibility;

    [ObservableProperty]
    private string? _readPath;

    [ObservableProperty]
    private string? _writePath;

    [ObservableProperty] 
    private List<string> _listOfFilesToRead = new List<string>();

    [ObservableProperty] private int _numberOfFiles;
    [ObservableProperty] int _counter = 0;
    [ObservableProperty] private float _currentProgressValue;
    private float _result;


    public delegate void FilesEditedChanged(object sender, int allFiles, int countValue);

    public event FilesEditedChanged? OnFilesEditedChanged;

    #endregion

    #region Constructor

    public ContentViewModel()
    {
        OnFilesEditedChanged += OnFilesEdited;
    }
    #endregion

    #region Methods

    private void OnFilesEdited(object sender, int allFilesCount, int count)
    {
        Counter = count;
        NumberOfFiles = allFilesCount;
        _result = (100 / NumberOfFiles) * Counter;
        CurrentProgressValue = _result;
        if (CurrentProgressValue >= 100)
        {
            CurrentProgressValue = 100;
            ProgressVisibility = false;
        }

    }

    private void ReadFilesFromDirectory()
    {
        if (ReadPath != null)
            foreach (string file in Directory.GetFiles(ReadPath))
            {
                ListOfFilesToRead.Add(file);
            }
    }
    #endregion

    #region Commands

    [RelayCommand]
    private void OpenLanguagePackReadFolder()
    {
        ListOfFilesToRead.Clear();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Title = "Pfad";
        folderBrowserDialog.AllowMultiSelect = false;
        folderBrowserDialog.InitialFolder = @"C:\";
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            ReadPath = folderBrowserDialog.SelectedFolder;
        }

        if (!String.IsNullOrEmpty(ReadPath))
        {
            ReadFilesFromDirectory();
        }
    }

    [RelayCommand]
    private void OpenLanguagePackWriteFolder()
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Title = "Pfad";
        folderBrowserDialog.AllowMultiSelect = false;
        folderBrowserDialog.InitialFolder = @"C:\";
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            WritePath = folderBrowserDialog.SelectedFolder;
        }
    }

    [RelayCommand]
    private async void WriteFiles()
    {
        ProgressVisibility = true;
        Counter = 0;
        foreach (string file in ListOfFilesToRead)
        {
            FileInfo fi = new FileInfo(file);
            string filename = fi.Name;
            string dataName;
            string value;
            List<string> linesToAppend = new List<string>();
            List<string> resultLines = new List<string>(File.ReadAllLines(file));
            List<string> headerLines = resultLines.GetRange(0, 119);

            resultLines.RemoveRange(0,119);
            filename = filename.Substring(0, filename.IndexOf('.'));
            filename = $"{filename}.Test.resx";
            File.WriteAllLines($@"{WritePath}\{filename}", headerLines);
            foreach (string line in resultLines)
            {
                if (line.Contains("<data name=\""))
                {
                    linesToAppend.Clear();
                    dataName = line.Substring(14);
                    dataName = dataName.Substring(0, dataName.IndexOf('"'));
                    value = $"<value>Test_{dataName}_Test</value>";
                    linesToAppend.Add(line);
                    linesToAppend.Add(value);
                    linesToAppend.Add("</data>");
                    File.AppendAllLines($@"{WritePath}\{filename}", linesToAppend);
                }
            }
            File.AppendAllText($@"{WritePath}\{filename}", "</root>");
            await Task.Delay(500);
            Counter++;
            if (OnFilesEditedChanged != null)
            {
                OnFilesEditedChanged(this, ListOfFilesToRead.Count, Counter);
            }
        } 
    }
    #endregion
}