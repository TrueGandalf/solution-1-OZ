namespace TerminalLibrary;

public class ProductListingSingleton
{
    private static ProductListingSingleton _instance;
    // product name, product
    private readonly Dictionary<string, Product> _products = new();
    private static readonly object _lock = new object();

    public Product FindProduct(string name)
    {
        if (_products.TryGetValue(name, out Product? product))
        {
            return product;
        }
        return _products[name] = new Product(name);
    }

    private ProductListingSingleton() { }

    public static ProductListingSingleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ProductListingSingleton();
                }
                return _instance;
            }
        }
    }
}
