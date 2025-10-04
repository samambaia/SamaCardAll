namespace FrontMaui.Helpers;

public static class ServiceHelper
{
    public static T GetService<T>() where T : notnull
    {
        var services = Microsoft.Maui.Controls.Application.Current?
            .Handler?
            .MauiContext?
            .Services;

        if (services is null)
            throw new InvalidOperationException("Unable to resolve services. The MauiContext is not available yet.");

        var service = services.GetService(typeof(T));
        if (service is null)
            throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");

        return (T)service;
    }
}