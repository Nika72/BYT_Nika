using ConsoleApp1.Models;

namespace ConsoleApp1.Interfaces;

public interface IVale
{
    void ParkCar(Customer customer);
    void RetrieveCar(Customer customer);
    void Stand();
}
