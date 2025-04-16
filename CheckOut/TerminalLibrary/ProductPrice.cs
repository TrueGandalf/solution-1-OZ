namespace TerminalLibrary;

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
