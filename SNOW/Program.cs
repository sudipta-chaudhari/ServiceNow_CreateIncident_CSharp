using System;
using SNOW.Logger;

namespace SNOW
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 1, y = 0, z;
            string incNo = string.Empty;

            try
            {
                z = x / y;
            }
            catch (Exception ex)
            {
                incNo = SNOWLogger.CreateIncidentServiceNow(ex.Message, "My App's exception : " + ex.StackTrace);

                Console.WriteLine("SNOW Incident Number : " + incNo);
            }
        }
    }
}
