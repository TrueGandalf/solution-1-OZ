using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLibrary;

public class PointOfSaleTerminal()
{
    private readonly Dictionary<(string, int), ProductPrice> _productPrices = new();
    private readonly Dictionary<string, Product> _products = new(); // simple implementation of a lightweight pattern
    private readonly Dictionary<Product, int> _busket = new();

    public void SetPricing(string name, int volume, decimal price)
    {
        var product = FindProduct(name);
        SetProductPrice(name, volume, product, price);

    }

    public void SetPricing(string name, int volume, double price)
    {
        SetPricing(name, volume, (decimal)price);
    }

    private void SetProductPrice(string name, int volume, Product product, decimal price)
    {

        if (_productPrices.TryGetValue((name, volume), out ProductPrice? productPrice))
        {
            productPrice = _productPrices[(name, volume)];
        }

        if (productPrice == null)
        {
            _productPrices[(name, volume)] = new ProductPrice(product, volume, price);
            return;
        }

        productPrice.UpdatePrice(price);
    }

    public void Scan(string name)
    {
        var product = FindProduct(name);

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

    private Product FindProduct(string name)
    {
        if (_products.TryGetValue(name, out Product? product))
        {
            return product;
        }
        return _products[name] = new Product(name);
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
