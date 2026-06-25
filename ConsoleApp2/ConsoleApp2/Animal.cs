namespace Test;

interface IAnimal
{
    void MakeSound();
    void Eat();
}


class Dog : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Woof!");
        //throw new NotImplementedException();
    }

    public void Eat()
    {
        Console.WriteLine("Dog is eating.");
    }
}
