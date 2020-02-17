using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class WeightedRandomUnigramSelector<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
