using Microsoft.AspNetCore.SignalR;

public class DrawHub : Hub
{
	private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
	public async Task SendDrawing(object data)
	{
		await Clients.Others.SendAsync("ReceiveDrawing", data);
	}

	public async Task ClearBoard()
	{
		await Clients.All.SendAsync("ClearBoard");
	}

	public async Task WaitForDrawing()
	{
		var tcs = new TaskCompletionSource<object>();
		_connections.Add(Context.ConnectionId, Context.ConnectionId, tcs);

		await tcs.Task;
	}

	public override Task OnDisconnectedAsync(Exception exception)
	{
		_connections.Remove(Context.ConnectionId);
		return base.OnDisconnectedAsync(exception);
	}

	public async Task TriggerDrawing(object data)
	{
		await Clients.All.SendAsync("ReceiveDrawing", data);
		foreach (var connectionId in _connections.GetConnections())
		{
			_connections.GetTcs(connectionId).SetResult(null);
		}
	}
}
