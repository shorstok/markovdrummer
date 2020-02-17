using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using markov_drummer.Properties;
using markov_drummer.Services;
using markov_drummer.Vm.NoteMappers;

namespace markov_drummer.Vm
{
    public partial class MainVm : INotifyPropertyChanged, INotifyDataErrorInfo 
    {
        private NoteMappingBase _selectedNoteMapping;        

        public MidiProcessorVm Processor { get; }

        public string SourceFolderPath
        {
            get => UiSettings.Current.SourcePath;
            set
            {
                if (value == UiSettings.Current.SourcePath) return;
                UiSettings.Current.SourcePath = value;
                Processor.ForceReloadSources = true;
                OnPropertyChanged();
            }
        }
                
        public string TargetFolderPath
        {
            get => UiSettings.Current.TargetPath;
            set
            {
                if (value == UiSettings.Current.TargetPath) return;
                UiSettings.Current.TargetPath = value;
                OnPropertyChanged();
            }
        }
        
        public int MarkovOrder
        {
            get => UiSettings.Current.MarkovOrder;
            set
            {
                if (value == UiSettings.Current.MarkovOrder) return;
                UiSettings.Current.MarkovOrder = value;
                Processor.ForceRegenerateChain = true;
                OnPropertyChanged();
            }
        }
        
        public bool TreatNotesInMetre
        {
            get => UiSettings.Current.TreatNotesInMetre;
            set
            {
                if (value == UiSettings.Current.TreatNotesInMetre) return;
                UiSettings.Current.TreatNotesInMetre = value;
                Processor.ForceReloadSources = true;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public NoteMappingBase[] AvailableNoteMappings { get; } 
        
        public NoteMappingBase SelectedNoteMapping
        {
            get => _selectedNoteMapping;
            set
            {
                if (Equals(value, _selectedNoteMapping)) return;
                _selectedNoteMapping = value;
                UiSettings.Current.SelectedMappingId  = _selectedNoteMapping?.Id ?? Guid.Empty;
                Processor.ForceRegenerateChain = true;
                OnPropertyChanged();
            }
        }
        
        public ICommand LocateSourceCommand { get; }
        public ICommand LocateTargetCommand { get; }
        
        public ICommand Start { get; }

        public MainVm()
        {
            AvailableNoteMappings = AvailableMappersProvider.GetAllMappings().ToArray();
            Processor = new MidiProcessorVm(this);
            SelectedNoteMapping = AvailableNoteMappings.FirstOrDefault(nm => nm.Id == UiSettings.Current.SelectedMappingId) ??
                                  AvailableNoteMappings.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(TargetFolderPath))
                TargetFolderPath = Directory.GetCurrentDirectory();

            Start = new DelegateCommand(t=> !string.IsNullOrWhiteSpace(SourceFolderPath), async o => await RunGenerator());
            
            LocateSourceCommand = new DelegateCommand(t=> true, SelectSourceFolder);
            LocateTargetCommand = new DelegateCommand(t=> true, SelectTargetFolder);
        }

        private void SelectTargetFolder(object obj)
        {
            var seed = TargetFolderPath;

            if (string.IsNullOrWhiteSpace(seed) || !Directory.Exists(seed))
                seed = null;

            var resultingPath = FolderSelectorService.SelectFolder("Pick folder to place results", seed);

            if (resultingPath != null)
                TargetFolderPath = resultingPath;
        }

        private void SelectSourceFolder(object obj)
        {
            var seed = SourceFolderPath;

            if (string.IsNullOrWhiteSpace(seed) || !Directory.Exists(seed))
                seed = null;

            var resultingPath = FolderSelectorService.SelectFolder("Pick folder with source MIDI files", seed);

            if (resultingPath != null)
            {
                SourceFolderPath = resultingPath;

                if(!Directory.GetFiles(SourceFolderPath,"*.mid").Any())
                    SetErrorFor(nameof(SourceFolderPath),"No MIDI files in this folder!");
            }
        }

        private async Task RunGenerator()
        {            
            if(!Validate())
                return;

            await Processor.WorkMarkov();
        }

        private bool Validate()
        {
            ClearAllErrors();

            if(string.IsNullOrWhiteSpace(TargetFolderPath) || !Directory.Exists(TargetFolderPath))
                SetErrorFor(nameof(TargetFolderPath),"Please, specify valid target folder");

            if(string.IsNullOrWhiteSpace(SourceFolderPath) || !Directory.Exists(SourceFolderPath))
                SetErrorFor(nameof(SourceFolderPath),"Please, specify valid source folder");
            else if(!Directory.GetFiles(SourceFolderPath,"*.mid").Any())
                SetErrorFor(nameof(SourceFolderPath),"No MIDI files in this folder!");

            if(null == SelectedNoteMapping)
                SetErrorFor(nameof(SelectedNoteMapping),"Please, select note mapping");

            return !HasErrors;
        }

        public void Shutdown()
        {
            UiSettings.Current.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
