namespace HolidayMakerGroup6;
//public enum Location
//{
//	Rivergate,
//	Tangalooma,
//	Azurebliss,
//	Greenside,
//	Crystalcove
//}
public class Location
{
	public string Name;
	public double DistanceToBeach;
	public double DistanceToCentre;

	public Location(string name, double distanceToBeach, double distanceToCentre)
	{
		Name = name;
		DistanceToBeach = distanceToBeach;
		DistanceToCentre = distanceToCentre;
	}
}