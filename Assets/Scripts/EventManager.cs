
namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine.Events;

    
    /// The event manager class. It is used to handle events.
    
    public static class EventManager
    {
        
        /// The event dictionary used for events.
        
        private static readonly Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

        
        /// The subscribe function. Subscribes to an event for later use.
        /// <para>
        /// </para>
        /// <remarks>
        /// <paramref name="eventName"></paramref> -The event name to subscribe to.
        /// </remarks>
        /// <para>
        /// </para>
        /// <remarks>
        /// <paramref name="listener"></paramref> -The listener that will be the event function.
        /// </remarks>
        
        /// <param name="eventName">
        /// The event Name.
        /// </param>
        /// <param name="listener">
        /// The listener.
        /// </param>
        public static void Subscribe(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;

            // If the event is already in the dictionary
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                // Then add another function to it
                thisEvent.AddListener(listener);
            }
            // If the dictionary does not contain the event
            else
            {// Create a new event
                thisEvent = new UnityEvent();

                // Add the function to it
                thisEvent.AddListener(listener);

                // Add the event name and event to the dictionary
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        
        /// The unsubscribe function. Unsubscribes to an event.
        /// <para>
        /// </para>
        /// <remarks>
        /// <paramref name="eventName"></paramref> -The event name to unsubscribe from.
        /// </remarks>
        /// <para>
        /// </para>
        /// <remarks>
        /// <paramref name="listener"></paramref> -The listener that will be the event function.
        /// </remarks>
        
        /// <param name="eventName">
        /// The event Name.
        /// </param>
        /// <param name="listener">
        /// The listener.
        /// </param>
        public static void UnSubscribe(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;

            // If the dictionary contains the event
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                // Remove the function from it
                thisEvent.RemoveListener(listener);
            }
        }

        
        /// The Publish function. Executes all functions in the event.
        /// <para>
        /// </para>
        /// <remarks>
        /// <paramref name="eventName"></paramref> -The event name to be called and execute its function(s).
        /// </remarks>
        
        /// <param name="eventName">
        /// The event Name.
        /// </param>
        public static void Publish(string eventName)
        {
            UnityEvent thisEvent;

            // If the dictionary contains the event
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                // Invoke the functions listening in the event
                thisEvent.Invoke();
            }
        }
    }
}
