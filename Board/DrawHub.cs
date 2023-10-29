using Microsoft.AspNetCore.SignalR;

public class DrawHub : Hub
{
	public async Task SendDrawing(object data)
	{
		await Clients.Others.SendAsync("ReceiveDrawing", data);
	}
	public async Task ClearBoard()
	{
		await Clients.All.SendAsync("ClearBoard");
	}
	

}
