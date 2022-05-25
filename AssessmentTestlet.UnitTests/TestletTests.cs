using System.Diagnostics.CodeAnalysis;

namespace AssessmentTestlet.Tests
{
    public class TestletTests
    {
        [Fact()]
        public void RandomizeTest()
        {
            // Arrange
            const int itemsCount = 10;
            const int pretestItemsCount = 4;
            const int firstItemsCount = 2;
            const ItemTypeEnum firstItemsType = ItemTypeEnum.Pretest;
            var testletId = Faker.Identification.UkNhsNumber();

            var random = new Random();

            var items = Enumerable
                .Range(1, itemsCount)
                .Select((x, i) => new
                {
                    order = random.NextInt64(),
                    item = new Item
                    {
                        ItemId = Faker.Identification.UkNhsNumber(),
                        ItemType = i < pretestItemsCount ? ItemTypeEnum.Pretest : ItemTypeEnum.Operational
                    }
                })
                .OrderBy(x => x.order)
                .Select(x => x.item)
                .ToList();

            // Act
            var result = new Testlet(testletId, items).Randomize();

            // Assert
            Assert.NotNull(result);

            Assert.NotEmpty(result);

            Assert.Equal(itemsCount, result.Count);

            Assert.Equal(pretestItemsCount, result.Count(x => x.ItemType == ItemTypeEnum.Pretest));

            Assert.NotEqual(items, result, new ItemComparer());

            for(var i = 0; i < firstItemsCount; i++)
            {
                Assert.Equal(firstItemsType, result[i].ItemType);
            }

            // Null arument
            Assert.Throws<ArgumentNullException>(() => new Testlet(string.Empty, null));

            // Wrong items count
            var wrongCountItems = Enumerable.Range(0, itemsCount).Select(x => new Item()).ToList();
            Assert.Throws<ArgumentException>(() => new Testlet(testletId, wrongCountItems));

            // IDs are not unique
            items[0].ItemId = items[1].ItemId;
            Assert.Throws<ArgumentException>(() => new Testlet(testletId, items));

            // Empty ID
            items[0].ItemId = String.Empty;
            Assert.Throws<ArgumentException>(() => new Testlet(testletId, items));

            // Wrong count of items by types
            items[0].ItemId = Faker.Identification.UkNhsNumber();
            items[pretestItemsCount].ItemType = firstItemsType;
            Assert.Throws<ArgumentException>(() => new Testlet(testletId, items));
        }

        public class ItemComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item? x, Item? y) => x.ItemId == y.ItemId;

            public int GetHashCode([DisallowNull] Item obj) => obj.GetHashCode();
        }

    }
}