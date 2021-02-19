using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName, description;
    public int mana, atk, hp;
    public Texture2D sprite { get { return _texture; } }

    Texture2D _texture = null;

    async void Awake()
    {
        _texture = await GetRemoteTexture("https://picsum.photos/");
    }

    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOp = www.SendWebRequest();

            while (asyncOp.isDone == false)
            {
                await Task.Delay(1000 / 30);
            }

            if (www.isNetworkError || www.isHttpError)
            {
                return null;
            }
            else
            {
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }

    void OnDestroy() => Dispose();
    public void Dispose() => Object.Destroy(_texture); // memory released, leak otherwise
}
