using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace markov_drummer.Services
{
    public static class FolderSelectorService
    {
        public static string SelectFolder(string title, string initialDirectory = null)
        {
            var dlg = new CommonOpenFileDialog
            {
                Title = title,
                IsFolderPicker = true,
                InitialDirectory = initialDirectory ?? Directory.GetCurrentDirectory(),
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = initialDirectory ?? Directory.GetCurrentDirectory(),
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };
            
            return dlg.ShowDialog() == CommonFileDialogResult.Ok ? dlg.FileName : null;
        }
    }
}
