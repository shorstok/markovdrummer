using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class MostPopularUnigramSelector<T> : UnigramSelectorBase, IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .GroupBy(a => a).OrderByDescending(a => a.Count())
                .FirstOrDefault()
                .FirstOrDefault();
        }

        public override Guid Id { get; } = Guid.Parse("E7CD99B5-07B6-4F91-B7DA-6433035A3914");
        
        public override string Name { get; } = "Most Popular unigram selector";
        public override string Description { get; } = "Most Popular unigram selector";
    }
}
