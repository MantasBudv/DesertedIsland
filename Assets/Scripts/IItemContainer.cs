public interface IItemContainer 
{
    bool ContainsItem(string item);
    int ItemCount(Item item);
    void RemoveItem(string name);
    bool AddItem(Item item);
    bool IsFull();
}
