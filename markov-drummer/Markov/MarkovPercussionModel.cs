﻿using System.Collections.Generic;
using markov_drummer.Markov.Chiscore;
using markov_drummer.Vm.NoteMappers;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Markov
{
    public class MarkovPercussionModel : GenericMarkov<Note[], MarkovNoteToken>
    {
        private readonly TempoMap _tempoMap;
        private readonly NoteMappingBase _noteMapping;

        public MarkovPercussionModel(int level, TempoMap tempoMap, NoteMappingBase noteMapping) : base(level)
        {
            _tempoMap = tempoMap;
            _noteMapping = noteMapping;
        }
        
        public override IEnumerable<MarkovNoteToken> SplitTokens(Note[] phrase)
        {
            for (var i = 0; i < phrase.Length; i++)
            {
                var note = phrase[i];
                var next = i == phrase.Length-1 ? phrase[0]: phrase[i+1];

                yield return new MarkovNoteToken(note,next,_noteMapping);
            }
        }

        public override Note[] RebuildPhrase(IEnumerable<MarkovNoteToken> tokens)
        {
            long position = 0;
            var result = new List<Note>();

            foreach (var markovNoteToken in tokens)
            {
                if(markovNoteToken.IsEmpty)
                    continue;

                var clone = markovNoteToken.Source.Clone();

                clone.Time = position;
                position += markovNoteToken.DurationWithSilence;

                result.Add(clone);
            }

            return result.ToArray();
        }

        public override MarkovNoteToken GetTerminatorUnigram()
        {
            return MarkovNoteToken.Empty;
        }

        public override MarkovNoteToken GetPrepadUnigram()
        {
            return MarkovNoteToken.Empty;
        }
    }
}