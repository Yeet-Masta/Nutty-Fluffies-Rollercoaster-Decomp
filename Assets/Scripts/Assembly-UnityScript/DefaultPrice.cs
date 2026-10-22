using System;

[Serializable]
public class DefaultPrice
{
	public string id;

	public int price;

	public DefaultPrice(string id, int price)
	{
		this.id = id;
		this.price = price;
	}
}
