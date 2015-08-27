using System;
using System.Management;

namespace JustForTestConsole
{
    internal class UsbHandler
    {
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            var instance = (ManagementBaseObject) e.NewEvent["TargetInstance"];
            foreach (PropertyData property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            var instance = (ManagementBaseObject) e.NewEvent["TargetInstance"];
            foreach (PropertyData property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }

        private void DoWork()
        {
            var insertQuery =
                new WqlEventQuery(
                    "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            var insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += DeviceInsertedEvent;
            insertWatcher.Start();

            var removeQuery =
                new WqlEventQuery(
                    "SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            var removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += DeviceRemovedEvent;
            removeWatcher.Start();
        }
    }
}
