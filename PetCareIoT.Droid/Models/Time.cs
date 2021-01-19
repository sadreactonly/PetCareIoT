
namespace PetCareIoT.Models
{
    public class Time
    {
        public Time(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public override string ToString()
        {
            string hours;
            string minutes;
            string seconds;

            if (Hours < 10)
                hours = $"0{Hours}";
            else
                hours = Hours.ToString();

            if (Minutes < 10)
                minutes = $"0{Minutes}";
            else
                minutes = Minutes.ToString();


            if (Seconds < 10)
                seconds = $"0{Seconds}";
            else
                seconds = Seconds.ToString();

            return $"{hours}:{minutes}:{seconds}";
        }

    }
}