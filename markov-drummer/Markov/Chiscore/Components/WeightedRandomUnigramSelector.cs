using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class WeightedRandomUnigramSelector<T> : UnigramSelectorBase, IUnigramSelector<T>
    {
        private readonly Random _generator = new Random();

        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams.OrderBy(a => _generator.Next()).FirstOrDefault();
        }

        public override Guid Id { get; } = Guid.Parse("98B72FCE-70FE-4941-BE6F-8CCFDA373E28");
        
        public override string Name { get; } = "Weighted Random unigram selector (default)";
        public override string Description { get; } = "Weighted Random unigram selector";
    }
}
