using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class AlphabeticUnigramSelectorDesc<T> : UnigramSelectorBase, IUnigramSelector<T>
    {
        public override Guid Id { get; } = Guid.Parse("00BE868F-7D3B-4B2E-80F1-83139C8F8BC7");

        public override string Name { get; } = "Descending alphabetic unigram selector";
        public override string Description { get; } = "Descending alphabetic unigram selector";

        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .OrderByDescending(a => a)
                .FirstOrDefault();
        }
    }
}