using System;
using markov_drummer.Markov;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class OnlyDurationMapping : NoteMappingBase
    {
        public override Guid Id { get; } = Guid.Parse("180CA82C-EE42-4B8B-A04A-FA037FB4EF00");

        public override string Name { get; } = "Note length only";
        public override string Description { get; } = "Use note length only as token";

        private long _quantizeDuration = 0;

        public override long GetNoteHash(MarkovNoteToken sourceToken, Note current, Note next)
        {
            if (0 == _quantizeDuration)
                _quantizeDuration = TimeConverter.ConvertFrom(new MusicalTimeSpan(1, 32), sourceToken.TargetTempoMap);

            return sourceToken.DurationWithSilence / _quantizeDuration;
        }
    }
}