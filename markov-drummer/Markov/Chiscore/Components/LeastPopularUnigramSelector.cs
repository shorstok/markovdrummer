using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class LeastPopularUnigramSelector<T> : UnigramSelectorBase, IUnigramSelector<T>
    {
        public override Guid Id { get; } = Guid.Parse("CD86B843-C848-4134-80D1-CE3C41AF1339");

        public override string Name { get; } = "Least popular unigram selector";
        public override string Description { get; } = "Least popular unigram selector";

        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .GroupBy(a => a).OrderByDescending(a => a.Count())
                .FirstOrDefault()
                .FirstOrDefault();
        }
    }
}