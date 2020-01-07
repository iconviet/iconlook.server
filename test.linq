<Query Kind="Program">
  <NuGetReference>Humanizer</NuGetReference>
  <Namespace>Humanizer</Namespace>
  <Namespace>Humanizer.Localisation</Namespace>
  <Namespace>Humanizer.Inflections</Namespace>
</Query>

void Main()
{
	var calculator = new BlockCalculator(13281892);
	calculator.GetNextTermDuration().Humanize(3).Dump();
}

public class BlockCalculator
{
	public long _height;
	
	public static long LastTermBlock = 13251043;

	public BlockCalculator(long height)
	{
		_height = height;
	}
	
	public long GetNextTermBlock()
	{
		return LastTermBlock + 43200;
	}
	
	public TimeSpan GetNextTermDuration()
	{
			return TimeSpan.FromSeconds((GetNextTermBlock() - _height)  * 2);
	}
}
