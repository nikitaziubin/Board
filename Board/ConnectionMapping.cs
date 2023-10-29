using System.Collections.Generic;
using System.Threading.Tasks;

public class ConnectionMapping<T>
{
	private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();
	private readonly Dictionary<string, TaskCompletionSource<object>> _tcsConnections = new Dictionary<string, TaskCompletionSource<object>>();

	public void Add(T key, string connectionId, TaskCompletionSource<object> tcs)
	{
		lock (_connections)
		{
			if (!_connections.TryGetValue(key, out var connections))
			{
				connections = new HashSet<string>();
				_connections.Add(key, connections);
			}

			connections.Add(connectionId);
		}

		lock (_tcsConnections)
		{
			_tcsConnections[connectionId] = tcs;
		}
	}

	public void Remove(string connectionId)
	{
		lock (_connections)
		{
			foreach (var key in _connections.Keys)
			{
				if (_connections[key].Contains(connectionId))
				{
					_connections[key].Remove(connectionId);
					break;
				}
			}
		}

		lock (_tcsConnections)
		{
			_tcsConnections.Remove(connectionId);
		}
	}

	public IEnumerable<string> GetConnections(T key)
	{
		if (_connections.TryGetValue(key, out var connections))
		{
			return connections;
		}

		return new List<string>();
	}

	public IEnumerable<string> GetConnections()
	{
		List<string> allConnections = new List<string>();
		foreach (var key in _connections.Keys)
		{
			allConnections.AddRange(_connections[key]);
		}
		return allConnections;
	}

	public TaskCompletionSource<object> GetTcs(string connectionId)
	{
		if (_tcsConnections.TryGetValue(connectionId, out var tcs))
		{
			return tcs;
		}

		return null;
	}
}
