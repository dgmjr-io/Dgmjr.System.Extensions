namespace System.Net.Http;

public class HttpEndPoint(Uri uri) : EndPoint
{
    public Uri Uri { get; } = uri;

    public override string ToString() => Uri.ToString();

    public override bool Equals(object? obj) => obj is HttpEndPoint other && Uri.Equals(other.Uri);

    public override int GetHashCode() => Uri.GetHashCode();

    public static bool operator ==(HttpEndPoint left, HttpEndPoint right) => left.Equals(right);
    public static bool operator !=(HttpEndPoint left, HttpEndPoint right) => !(left == right);
    public static implicit operator Uri(HttpEndPoint endPoint) => endPoint.Uri;
    public static implicit operator HttpEndPoint(Uri uri) => new(uri);
    public static implicit operator HttpEndPoint(string uri) => new(new Uri(uri));
    public static implicit operator string(HttpEndPoint endPoint) => endPoint.Uri.ToString();
}
