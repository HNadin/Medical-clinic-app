namespace Medical_clinic.Observer
{

    // Інтерфейс спостерігача
    public interface IObserver
    {
        void Update(NewService newService);
    }

    // Клас для нових медичних послуг
    public class NewService
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    // Клас для пацієнта
    public class Patient : IObserver
    {
        public string UserId { get; set; } // Додано властивість UserId

        public string Name { get; set; }

        public void Update(NewService newService)
        {
            Console.WriteLine($"Dear {Name}, a new medical service is available: {newService.Name} - {newService.Description}");
        }
    }
}