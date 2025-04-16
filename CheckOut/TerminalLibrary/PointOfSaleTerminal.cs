using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLibrary;

public class PointOfSaleTerminal()
{
    private readonly Dictionary<Product, int> _busket = new();
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

        if (!_busket.ContainsKey(product))
        {
            _busket.Add(product, 1);
        }
        else
        {
            _busket[product]++;
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

        foreach (var product in _busket.Keys)
        {
            total += product.GetTotal(_busket[product]);
        }

        return (double)total;
    }
}
