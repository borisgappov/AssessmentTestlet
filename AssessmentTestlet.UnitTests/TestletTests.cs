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
            const int pretestCount = 4;
            const int randomMax = 99999;

            var random = new Random();

            var items = Enumerable
                .Range(1, itemsCount)
                .Select((x, i) => new
                {
                    order = random.Next(randomMax),
                    item = new Item
                    {
                        ItemId = Faker.Identification.UkNhsNumber(),
                        ItemType = i < pretestCount ? ItemTypeEnum.Pretest : ItemTypeEnum.Operational
                    }
                })
                .OrderBy(x => x.order)
                .Select(x => x.item)
                .ToList();

            // Act
            var result = new Testlet(Faker.Identification.UkNhsNumber(), items).Randomize();

            // Assert
            Assert.NotNull(result);

            Assert.NotEmpty(result);

            Assert.Equal(itemsCount, result.Count);

            Assert.Equal(pretestCount, result.Count(x => x.ItemType == ItemTypeEnum.Pretest));

            Assert.NotEqual(items, result, new ItemComparer());

            Assert.Equal(ItemTypeEnum.Pretest, result[0].ItemType);
            Assert.Equal(ItemTypeEnum.Pretest, result[1].ItemType);
        }

        public class ItemComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item? x, Item? y) => x.ItemId == y.ItemId;

            public int GetHashCode([DisallowNull] Item obj) => obj.GetHashCode();
        }

    }
}