public interface IBuyable
{
    bool CanBuy();
    void OnBuyResult(bool success);
    string SKU { get; }
    int USDPrice { get; }
}
