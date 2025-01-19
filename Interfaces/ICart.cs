using ConsoleApp1.Models;

namespace ConsoleApp1
{
    public interface ICart
    {
        int CartID { get; }
        void GetCart();
        void AddItemToCart(Dish dish, int quantity);
        void ModifyItemCount(Dish dish, int newQuantity);
    }
}