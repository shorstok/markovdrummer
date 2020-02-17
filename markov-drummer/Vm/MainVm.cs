using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using markov_drummer.Config;
using markov_drummer.Properties;
using markov_drummer.Vm.NoteMappers;

namespace markov_drummer.Vm
{
    [DataContract]
    public class UiSettings : GlobalSettings<UiSettings>
    {
        [DataMember]
        public string SourcePath { get;set; }

        [DataMember]
        [DefaultGlobalSettingValue(2)]
        public int MarkovOrder { get; set; }

        [DataMember]
        [DefaultGlobalSettingValue(true)]
        public bool TreatNotesInMetre { get; set; }

        [DataMember]
        public Guid SelectedMappingId { get; set; }

        [DataMember]
        public string TargetPath { get; set; }
    }

    
    public class MainVm : INotifyPropertyChanged, INotifyDataErrorInfo 
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
        public NoteMappingBase[] AvailableNoteMappings { get; } = {
            new NoteValueMapping(),
            new EzDrummerNoteValueMapping(),
            new VelSensitiveEzDrummerNoteValueMapping()
        };

        

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

        public MainVm()
        {
            Processor = new MidiProcessorVm(this);
            SelectedNoteMapping = AvailableNoteMappings.FirstOrDefault(nm => nm.Id == UiSettings.Current.SelectedMappingId) ??
                                  AvailableNoteMappings.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(TargetFolderPath))
                TargetFolderPath = Directory.GetCurrentDirectory();
        }
        
        public IEnumerable GetErrors(string propertyName)
        {
            yield break;
        }

        public bool HasErrors { get; } = false;

        

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
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
