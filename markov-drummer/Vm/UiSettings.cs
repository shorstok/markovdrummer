using System;
using System.Runtime.Serialization;
using markov_drummer.Config;

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

        [DataMember]
        [DefaultGlobalSettingValue(true)]
        public bool OpenTargetFolderOnSuccess { get; set; }
    }
}