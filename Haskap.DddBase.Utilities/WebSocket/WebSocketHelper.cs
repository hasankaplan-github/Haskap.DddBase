using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Utilities.WebSocket;
public class WebSocketHelper : IDisposable
{
    private const int BufferLengthInBytes = 50;
    private readonly ILogger<WebSocketHelper> _logger;
    private bool _disposedValue;
    private readonly SocketsHttpHandler _socketsHttpHandler;
    private ClientWebSocket _clientWebSocket;

    public WebSocketState State => _clientWebSocket.State;

    public WebSocketHelper(ILogger<WebSocketHelper> logger)
    {
        _logger = logger;
        _socketsHttpHandler = new();
        _clientWebSocket = new();
        _clientWebSocket.Options.HttpVersion = HttpVersion.Version20;
        //_clientWebSocket.Options.HttpVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                CloseConnectionAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).GetAwaiter().GetResult();

                _clientWebSocket?.Dispose();

                _socketsHttpHandler?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~WebSocketHelper()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                return;
            }

            await _clientWebSocket.ConnectAsync(uri, new HttpMessageInvoker(_socketsHttpHandler), cancellationToken);
            _logger.LogInformation("...Connected to the server...");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in Connecting: {exMessage}", ex.Message);
        }
    }

    public async Task ReconnectAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        try
        {
            await CloseConnectionAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken);
            _clientWebSocket.Dispose();
            _clientWebSocket = new();

            await _clientWebSocket.ConnectAsync(uri, new HttpMessageInvoker(_socketsHttpHandler), cancellationToken);
            _logger.LogInformation("...Reconnected to the server...");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in Reconnecting: {exMessage}", ex.Message);
        }
    }

    public async Task CloseConnectionAsync(WebSocketCloseStatus webSocketCloseStatus, string? statusDescription, CancellationToken cancellationToken = default)
    {
        if (_clientWebSocket.State != WebSocketState.Closed && 
            _clientWebSocket.State != WebSocketState.None &&
            _clientWebSocket.State != WebSocketState.Aborted)
        {
            await _clientWebSocket.CloseOutputAsync(webSocketCloseStatus, statusDescription, cancellationToken);
            _logger.LogInformation("...Web Socket Connection closed with description: {description}...", statusDescription);
        }
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        await _clientWebSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, cancellationToken);
    }

    public async Task ReceiveMessageAsync(Func<string?, Task> receivedFunc, CancellationToken cancellationToken = default)
    {
        try
        {
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                byte[] buffer = new byte[BufferLengthInBytes];
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseConnectionAsync(WebSocketCloseStatus.NormalClosure, result.CloseStatusDescription, cancellationToken);

                    break;
                }

                var jsonMessage = await GetJsonMessageAsync(result, buffer, cancellationToken);

                await receivedFunc.Invoke(jsonMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in receiving: {exMessage}", ex.Message);
        }
    }

    private async Task<string?> GetJsonMessageAsync(WebSocketReceiveResult result, byte[] buffer, CancellationToken cancellationToken)
    {
        if (result.MessageType == WebSocketMessageType.Text)
        {
            var offset = 0;

            while (!result.EndOfMessage)
            {
                var newBuffer = new byte[buffer.Length + BufferLengthInBytes];
                Array.Copy(buffer, 0, newBuffer, 0, buffer.Length);
                offset += BufferLengthInBytes;
                buffer = newBuffer;
                result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, BufferLengthInBytes), cancellationToken);
            }

            string message = Encoding.UTF8.GetString(buffer, 0, offset + result.Count);
            _logger.LogInformation("...Received message {message}...", message);

            return message;
        }

        return null;
    }
}
