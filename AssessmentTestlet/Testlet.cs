namespace AssessmentTestlet
{
    public class Testlet
    {
        public string TestletId;
        private List<Item> Items;

        private const int itemsCount = 10;
        private const int firstItemsCount = 2;
        private const ItemTypeEnum firstItemsType = ItemTypeEnum.Pretest;

        private readonly Random random = new Random();

        private readonly Dictionary<ItemTypeEnum, int> typesCount = 
            new Dictionary<ItemTypeEnum, int> { 
                { ItemTypeEnum.Pretest, 4 },
                { ItemTypeEnum.Operational, 6 }
            };

        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;
            Validate();
        }

        public List<Item> Randomize()
        {
            Validate();

            var firstItems = Items
                .Where(x => x.ItemType == firstItemsType)
                .OrderBy(x => random.NextInt64())
                .Take(firstItemsCount)
                .ToList();

            var restItems = Items
                .Where(x => !firstItems.Any(e => e.ItemId == x.ItemId))
                .OrderBy(x => random.NextInt64())
                .ToList();

            return firstItems.Concat(restItems).ToList();
        }

        private void Validate()
        {
            if(string.IsNullOrEmpty(TestletId) || Items == null)
            {
                throw new ArgumentNullException("TestletId and Items must not be null");
            }

            if (Items.Any(x => string.IsNullOrEmpty(x.ItemId)))
            {
                throw new ArgumentException("Item IDs must not be null or empty");
            }

            if (Items.GroupBy(x => x.ItemId).Count() != itemsCount)
            {
                throw new ArgumentException("Item IDs must be unique");
            }

            if (Items.Count != itemsCount)
            {
                throw new ArgumentException (string.Format("Items list length should be {0}", itemsCount));
            }

            var itemTypesCount = Items.GroupBy(x => x.ItemType).Select(x => new
            {
                type = x.Key,
                count = x.Count()
            }).ToDictionary(x => x.type, x => x.count);

            if (typesCount.Any(x => itemTypesCount[x.Key] != x.Value))
            {
                throw new ArgumentException(
                    string.Format(
                        "The number of elements by type should be: {0}",
                        string.Join(", ", typesCount.Select(x => $"{x.Key.ToString()}: {x.Value}"))
                    )
                );
            }
        }
    }
}
