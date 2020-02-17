using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class AlphabeticUnigramSelector<T> : UnigramSelectorBase, IUnigramSelector<T>
    {
        public override Guid Id { get; } = Guid.Parse("3EC5415A-6526-4BFA-8DEC-4AA91BB35B71");

        public override string Name { get; } = "Alphabetic unigram selector";
        public override string Description { get; } = "Alphabetic unigram selector";

        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .OrderBy(a => a)
                .FirstOrDefault();
        }
    }
}