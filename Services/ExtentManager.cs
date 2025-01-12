using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public static class ExtentManager
    {
        public static void LoadAllExtents()
        {
            try
            {
                SerializableObject<Reservation>.LoadExtent();
                SerializableObject<Member>.LoadExtent();
                SerializableObject<Dish>.LoadExtent();
                SerializableObject<Order>.LoadExtent();
                SerializableObject<Table>.LoadExtent();
                SerializableObject<Chef>.LoadExtent();
                SerializableObject<ExecutiveChef>.LoadExtent();
                SerializableObject<SousChef>.LoadExtent();
                SerializableObject<Waiter>.LoadExtent();
                SerializableObject<Manager>.LoadExtent();
                SerializableObject<Payment>.LoadExtent();
                Console.WriteLine("All Extents Loaded Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Extents: {ex.Message}");
            }
        }

        public static void SaveAllExtents()
        {
            try
            {
                SerializableObject<Reservation>.SaveExtent();
                SerializableObject<Member>.SaveExtent();
                SerializableObject<Dish>.SaveExtent();
                SerializableObject<Order>.SaveExtent();
                SerializableObject<Table>.SaveExtent();
                SerializableObject<Chef>.SaveExtent();
                SerializableObject<ExecutiveChef>.SaveExtent();
                SerializableObject<SousChef>.SaveExtent();
                SerializableObject<Waiter>.SaveExtent();
                SerializableObject<Manager>.SaveExtent();
                SerializableObject<Payment>.SaveExtent();
                Console.WriteLine("All Extents Saved Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Saving Extents: {ex.Message}");
            }
        }
    }
}