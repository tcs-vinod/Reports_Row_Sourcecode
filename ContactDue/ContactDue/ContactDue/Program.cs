namespace ContactDue
{
	internal class Program
	{
		private static void Main()
		{
			Notify DoIt = new Notify();
			DoIt.SendEmail();
		}
	}
}
