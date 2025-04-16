namespace TerminalLibrary;

public class PointOfSaleTerminal()
{
    public Dictionary<Product, int> Busket { get; private set; } = new();
    private readonly PriceListingSingleton _priceListing = PriceListingSingleton.Instance;
    private readonly ProductListingSingleton _productListing = ProductListingSingleton.Instance;

    public void SetPricing(string name, int volume, decimal price)
    {
        var product = _productListing.FindProduct(name);
        _priceListing.SetProductPrice(name, volume, product, price);
    }

    public void SetPricing(string name, int volume, double price)
    {
        SetPricing(name, volume, (decimal)price);
    }

    public void Scan(string name)
    {
        var product = _productListing.FindProduct(name);

        if (!Busket.ContainsKey(product))
        {
            Busket.Add(product, 1);
        }
        else
        {
            Busket[product]++;
        }
    }

    public void ScanSplitted(string names)
    {
        foreach (char a in names)
        {
            Scan(new string([a]));
        }
    }

    public double CalculateTotal()
    {
        decimal total = 0;

        foreach (var product in Busket.Keys)
        {
            total += product.GetTotal(Busket[product]);
        }

        return (double)total;
    }
}
