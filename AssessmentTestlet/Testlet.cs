namespace AssessmentTestlet
{
    public class Testlet
    {
        public string TestletId;
        private List<Item> Items;

        const int maxRandom = 99999;
        readonly Random random;

        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;

            random = new Random();
        }

        public List<Item> Randomize()
        {
            var firstTwo = Items
                .Where(x => x.ItemType == ItemTypeEnum.Pretest)
                .OrderBy(x => random.Next(maxRandom))
                .Take(2)
                .ToList();

            var rest = Items
                .Where(x => !firstTwo.Any(e => e.ItemId == x.ItemId))
                .OrderBy(x => random.Next(maxRandom))
                .ToList();

            return firstTwo.Concat(rest).ToList();
        }
    }
}
