namespace TerminalLibrary;

public class PriceListingSingleton
{
    private static PriceListingSingleton _instance;
    private readonly Dictionary<(string name, int volume), ProductPrice> _productPrices = new();
    private static readonly object _lock = new object();

    public void SetProductPrice(string name, int volume, Product product, decimal price)
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

    private PriceListingSingleton() { }

    public static PriceListingSingleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new PriceListingSingleton();
                }
                return _instance;
            }
        }
    }
}
