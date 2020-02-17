using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class EzDrummerNoteValueMapping : NoteMappingBase
    {
        public override Guid Id { get; } = Guid.Parse("229A96FE-94D5-4931-8BD5-B74E1E8FC55B");

        public override string Name { get; } = "Toontrack (SD / EZD)";
        public override string Description { get; } = "Tokenize all similar notes as same using Superior Drummer / EZDrummer keymapping (e.g. treat C#4 ... E4 as 'closed hat' etc)";
        
        private static readonly Dictionary<int, int> ReductionMapping = new Dictionary<int, int>();
        
        //These are used for tokens only, original note preserved on Markov walk reconstruction
        private readonly Tuple<int, int>[] _reductionRanges = new[]
        {
            //Closed hats
            Tuple.Create(65,61),
            
            //Cymbals & some rides
            Tuple.Create(58,52),
            Tuple.Create(49,50),
            
            Tuple.Create(40,38),

            Tuple.Create(26,23),

        };

        public EzDrummerNoteValueMapping()
        {
            foreach (var reductionRange in _reductionRanges)
            {
                for (int i = reductionRange.Item2; i < reductionRange.Item1; i++)
                {
                    ReductionMapping[i] = reductionRange.Item1;
                }
            }
        }

        public override long GetNoteHash(Note current, Note next)
        {
            if (ReductionMapping.TryGetValue(current.NoteNumber, out var replacement))
                return replacement;

            return current.NoteNumber;
        }
    }
}