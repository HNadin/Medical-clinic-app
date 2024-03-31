using Medical_clinic.Data;
using Medical_clinic.Models;
using Medical_clinic.Observer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

// Інтерфейс спостерігача
public interface IObservable
{
    void Attach(IObserver observer, ApplicationContext context);
    void Detach(IObserver observer, ApplicationContext context);
    void Notify(NewService newService, List<string> subscribedUsers);
}


public class MedicalFacility : IObservable
{
    private List<IObserver> _observers = new List<IObserver>();

    // Клас для підписки на сповіщення
    public void Attach(IObserver observer, ApplicationContext context)
    {
        _observers.Add(observer);

        if (observer is Patient patient)
        {
            var existingSubscription = context.Subscriptions.FirstOrDefault(s => s.UserId == patient.UserId);
            if (existingSubscription == null)
            {
                var newSubscription = new Subscription { UserId = patient.UserId, IsSubscribed = true };
                context.Subscriptions.Add(newSubscription);
            }
            else
            {
                existingSubscription.IsSubscribed = true;
            }

            context.SaveChanges();
        }
    }

    // Клас для відписки від сповіщень
    public void Detach(IObserver observer, ApplicationContext context)
    {
        _observers.Remove(observer);

        if (observer is Patient patient)
        {
            var existingSubscription = context.Subscriptions.FirstOrDefault(s => s.UserId == patient.UserId);
            if (existingSubscription != null)
            {
                existingSubscription.IsSubscribed = false;
                context.SaveChanges();
            }
        }
    }

    // Клас для надсилання сповіщень
    public void Notify(NewService newService, List<string> subscribedUsers)
    {
        foreach (var observer in _observers)
        {
            observer.Update(newService);
        }

        foreach (var userEmail in subscribedUsers)
        {
            WriteToFile(userEmail, newService);
        }
    }

    private void WriteToFile(string userEmail, NewService newService)
    {
        // Ось код, який записує інформацію до файлу про отримане сповіщення
        string filePath = $"NotificationLog_{DateTime.Now.ToString("yyyyMMdd")}.txt";
        string logEntry = $"User: {userEmail}, Received Notification: {newService.Name} - {newService.Description}, Timestamp: {DateTime.Now.ToString()}";
        File.AppendAllText(filePath, logEntry + Environment.NewLine);
    }
}
