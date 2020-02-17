using System;
using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class UnweightedRandomUnigramSelector<T> : UnigramSelectorBase, IUnigramSelector<T>
    {
        private readonly Random _generator = new Random();

        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams.GroupBy(a => a)
                .Select(a => a.FirstOrDefault())
                .OrderBy(a => _generator.Next())
                .FirstOrDefault();
        }

        
        public override Guid Id { get; } = Guid.Parse("98B72FCE-70FE-4941-BE6F-8CCFDA373E28");
        
        public override string Name { get; } = "Unweighted Random unigram selector";
        public override string Description { get; } = "Unweighted Random unigram selector";
    }
}
