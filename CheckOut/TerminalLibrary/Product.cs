namespace TerminalLibrary;

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
            // log this case properly
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

            total += chunksQtt * bestPrice.Price;
        }

        return total;
    }
}
