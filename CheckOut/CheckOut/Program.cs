// See https://aka.ms/new-console-template for more information
using System.Collections.Specialized;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");


PointOfSaleTerminal terminal = new PointOfSaleTerminal();
terminal.SetPricing("A", volume: 1, price: 1.25);
terminal.SetPricing("A", volume: 3, price: 3.00);
terminal.SetPricing("B", volume: 1, price: 4.25);
terminal.SetPricing("C", volume: 1, price: 1.00);
terminal.SetPricing("C", volume: 6, price: 5.00);
terminal.SetPricing("D", volume: 1, price: 0.75);
//terminal.Scan("A");
//terminal.Scan("C");


//terminal.ScanSplitted("AAAABCDAAA");
//terminal.ScanSplitted("CCCCCCC");
terminal.ScanSplitted("ABCD");



//terminal.ScanSplitted("AAAAAAA");

//... etc.
double result = terminal.CalculateTotal();

Console.WriteLine("result:");
Console.WriteLine(result);
Console.ReadLine();


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

public class Product
{
    public Product(string name)
    {
        Name = name;
    }
    public string Name { get; private set; }

    // volume, price
    private readonly SortedDictionary<int, ProductPrice> _productPrices = new();
    public void AddPricing(ProductPrice productPrice)
    {
        if (!_productPrices.ContainsKey(productPrice.Volume) && _productPrices.Count >= 2)
            throw new ArgumentOutOfRangeException("You can't add more than 2 prices for a product");

        _productPrices[productPrice.Volume] = productPrice;
    }
    public void RemovePricing(int volume)
    {
        _productPrices.Remove(volume);
    }

    // if we will have a lot of bulk prices we can use binary search and use SortedDictionary
    // now we have only two prices for each product
    private ProductPrice GetBestPrice(int inputVolume)
    {
        if (_productPrices.Count == 0 && !_productPrices.ContainsKey(1))
        {
            // log this cases properly
            return null;
        }
        var bulkVolume = _productPrices.Keys.Max();
        var bulkPrice = _productPrices[bulkVolume];
        var singlePrice = _productPrices[1];

        if (inputVolume <= bulkVolume)
            return singlePrice;

        return bulkPrice;
    }

    public decimal GetTotal(int inputVolume)
    {
        var bestPrice = GetBestPrice(inputVolume);

        if (bestPrice == null)
        {
            // log this case properly
            return 0;
        }

        var remaingVolume = inputVolume;

        decimal total = 0;

        while (remaingVolume > 0)
        {
            inputVolume = remaingVolume;
            bestPrice = GetBestPrice(inputVolume);
            remaingVolume = inputVolume % bestPrice.Volume;

            var chunksQtt = inputVolume / bestPrice.Volume;

            total += chunksQtt  * bestPrice.Price;
        }

        return total;
    }

}

public class ProductPrice
{
    public ProductPrice(Product product, int volume, decimal price)
    {
        Price = price;
        Volume = volume;
        Product = product;

        Product.AddPricing(this);
    }

    public decimal Price { get; private set; }
    public Product Product { get; }
    public int Volume { get; }

    public void UpdatePrice(decimal price)
    {
        if (Price != price)
            Price = price;

        Product.AddPricing(this);
    }
}
