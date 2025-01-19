using ConsoleApp1.Models;

public class ItemQuantityInCart
{
    public Dish Dish { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Dish.Price * Quantity;

    // Parameterless constructor for object initializer
    public ItemQuantityInCart() { }

    // Constructor with parameters
    public ItemQuantityInCart(Dish dish, int quantity)
    {
        if (dish == null)
            throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        Dish = dish;
        Quantity = quantity;
    }

    public void ModifyQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        Quantity = newQuantity;
        Console.WriteLine($"Quantity for dish '{Dish.Name}' updated to {Quantity}.");
    }

    public void AddQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0)
            throw new ArgumentException("Additional quantity must be greater than zero.", nameof(additionalQuantity));

        Quantity += additionalQuantity;
        Console.WriteLine($"Added {additionalQuantity} to dish '{Dish.Name}'. Total quantity: {Quantity}.");
    }

    public void RemoveQuantity(int quantityToRemove)
    {
        if (quantityToRemove <= 0)
            throw new ArgumentException("Quantity to remove must be greater than zero.", nameof(quantityToRemove));
        if (quantityToRemove > Quantity)
            throw new ArgumentException($"Cannot remove more than current quantity ({Quantity}).");

        Quantity -= quantityToRemove;
        Console.WriteLine($"Removed {quantityToRemove} from dish '{Dish.Name}'. Remaining quantity: {Quantity}.");
    }

    public override string ToString()
    {
        return $"Item: {Dish.Name}, Quantity: {Quantity}, Total Price: {TotalPrice:C}";
    }

    public override bool Equals(object obj)
    {
        if (obj is not ItemQuantityInCart other)
            return false;

        return Dish.IdDish == other.Dish.IdDish && Quantity == other.Quantity;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Dish.IdDish, Quantity);
    }
}
