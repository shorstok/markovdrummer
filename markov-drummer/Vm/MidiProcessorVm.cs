﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using markov_drummer.Annotations;
using markov_drummer.Markov;
using markov_drummer.Markov.Chiscore.Components;
using markov_drummer.Vm.NoteMappers;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm
{
    public partial class MidiProcessorVm : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly MainVm _owner;
        private string _status;

        private static readonly TempoMap ResultingTempoMap = TempoMap.Create(Tempo.FromBeatsPerMinute(120));
        private List<Note> _sourceNotes;
        private double _progressValue;
        private bool _forceReloadSources;
        private bool _forceRegenerateChain;
        private MarkovPercussionModel _model;

        public UnigramSelectorBase ActiveUnigramSelector { get; set; }

        public bool ForceRegenerateChain
        {
            get => _forceRegenerateChain;
            set
            {
                if (value == _forceRegenerateChain) return;
                _forceRegenerateChain = value;

                if (value)
                    Status = null;

                OnPropertyChanged();
            }
        }

        public bool ForceReloadSources
        {
            get => _forceReloadSources;
            set
            {
                if (value == _forceReloadSources) return;
                _forceReloadSources = value;

                if (value)
                    Status = null;

                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged(nameof(IsGenerationInProgress));
                OnPropertyChanged();
            }
        }

        public bool IsGenerationInProgress => !string.IsNullOrWhiteSpace(Status);


        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                if (value.Equals(_progressValue)) return;
                _progressValue = value;
                OnPropertyChanged();
            }
        }

        public MidiProcessorVm(MainVm owner)
        {
            _owner = owner;
            
        }

        private  List<Note> LoadSources()
        {
            _model = null;
            var result = new List<Note>();

            var sourceFiles = Directory.GetFiles(_owner.SourceFolderPath,"*.mid");
            for (var i = 0; i < sourceFiles.Length; i++)
            {
                ProgressValue = (double) i / sourceFiles.Length;

                var sourceFile = sourceFiles[i];
                var midiFile = MidiFile.Read(sourceFile,
                    new ReadingSettings
                    {
                        NotEnoughBytesPolicy = NotEnoughBytesPolicy.Ignore,
                        InvalidChunkSizePolicy = InvalidChunkSizePolicy.Ignore
                    });

                var origTempoMap = midiFile.GetTempoMap();

                foreach (var note in midiFile.GetNotes())
                {
                    if (_owner.TreatNotesInMetre)
                    {
                        var convTime = TimeConverter.ConvertTo<MusicalTimeSpan>(note.Time, origTempoMap);
                        var convLen = LengthConverter.ConvertTo<MusicalTimeSpan>(note.Length, note.Time, origTempoMap);

                        note.Time = TimeConverter.ConvertFrom(convTime, ResultingTempoMap);
                        note.Length = LengthConverter.ConvertFrom(convLen, convTime, ResultingTempoMap);
                    }
                    else
                    {
                        var convTime = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, origTempoMap);
                        var convLen = LengthConverter.ConvertTo<MetricTimeSpan>(note.Length, note.Time, origTempoMap);

                        note.Time = TimeConverter.ConvertFrom(convTime, ResultingTempoMap);
                        note.Length = LengthConverter.ConvertFrom(convLen, convTime, ResultingTempoMap);
                    }

                    result.Add(note);
                }
            }

            return result;
        }

        public async Task<string> WorkMarkov()
        {
            try
            {
                var midiFile = new MidiFile();

                var trackChunk = new TrackChunk();

                Status = "Loading source files...";

                if (_sourceNotes == null || ForceReloadSources)
                {
                    _sourceNotes = await Task.Run(() => LoadSources());
                    ForceReloadSources = false;
                }

                if (!_sourceNotes.Any())
                    throw new Exception("No source found! Please specify folder with midi files");

                using (var notesManager = trackChunk.ManageNotes())
                {
                    if (_model == null || ForceRegenerateChain)
                    {
                        _model = new MarkovPercussionModel(_owner.MarkovOrder, ResultingTempoMap,
                            _owner.SelectedNoteMapping)
                        {
                            UnigramSelector = ActiveUnigramSelector as IUnigramSelector<MarkovNoteToken> ?? new WeightedRandomUnigramSelector<MarkovNoteToken>(),
                            //EnsureUniqueWalk = true
                        };

                        Status = "Training markov chain...";
                        await Task.Run(() => _model.Learn(_sourceNotes.ToArray()));

                        ForceRegenerateChain = false;
                    }

                    Status = "Generating...";
                    // Walk the model
                    var result = await Task.Run(()=>_model.Walk().FirstOrDefault());
                
                    notesManager.Notes.Add(result);

                    
                }
           
                midiFile.Chunks.Add(trackChunk);                           

                var target = Path.Combine(_owner.TargetFolderPath,$"result-{DateTime.Now:yyyy-dd-M--HH-mm-ss}.mid");

                File.Delete(target);
                midiFile.Write(target);
                Status = $"Result OK, in {target}";

                return target;
            }
            catch (Exception e)
            {
                Status = "Failure: " + e.Message;
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {         
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
