namespace AssessmentTestlet
{
    public enum ItemTypeEnum
    {
        Pretest = 0,
        Operational = 1
    }

    public class Item
    {
        public string ItemId;

        public ItemTypeEnum ItemType;
    }
}
