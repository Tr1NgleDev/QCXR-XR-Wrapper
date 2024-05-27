using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class SkinHandler : MonoBehaviour
{
	public Material skin;
	public Material profilePicture;
	private string skinType;

	public GameObject rSlim;
	public GameObject rClassic;
	public GameObject lSlim;
	public GameObject lClassic;

	[FormerlySerializedAs("FigurineSlim")] public GameObject figurineSlim;
	[FormerlySerializedAs("FigurineClassic")] public GameObject figurineClassic;

	[Header("Debug")] public string FETCHINGUSERNAME;

	[ContextMenu("DEBUG FETCH")]
	public void DEBUGFETCH()
	{
		LoadSkin(FETCHINGUSERNAME);
	}

	public void LoadSkin(string username)
	{
		// LoadImage
		Task.Run(async () =>
		{
			UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://minotar.net/skin/" + username);
			request.SetRequestHeader("User-Agent", "QuestCraftPlusPlus/QuestCraft/" + Application.version + " (discord.gg/questcraft)");
			await request.SendWebRequest();
			
			if (request.result != UnityWebRequest.Result.Success)
				Debug.Log(request.error);
			else
			{
				Debug.Log("Loading skin");
				skin.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
				skin.mainTexture.filterMode = FilterMode.Point;
				profilePicture.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
				profilePicture.mainTexture.filterMode = FilterMode.Point;
			}
		});

		// SetSkinType
		Task.Run(async () =>
		{
			UnityWebRequest www = UnityWebRequest.Get("https://starlightskins.lunareclipse.studio/info/user/" + username);
			www.SetRequestHeader("User-Agent", "QuestCraftPlusPlus/QuestCraft/" + Application.version + " (discord.gg/questcraft)");
			await www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
				Debug.Log(www.error);
			else
			{
				JObject SkinType = JObject.Parse(www.downloadHandler.text);
				skinType = (string)SkinType["skinType"];
				Debug.Log(skinType);
			}

			if (skinType != "wide")
			{
				rSlim.SetActive(true);
				lSlim.SetActive(true);
				figurineSlim.SetActive(true);

				rClassic.SetActive(false);
				lClassic.SetActive(false);
				figurineClassic.SetActive(false);
			}
			else
			{
				rSlim.SetActive(false);
				lSlim.SetActive(false);
				figurineSlim.SetActive(false);

				rClassic.SetActive(true);
				lClassic.SetActive(true);
				figurineClassic.SetActive(true);
			}
		});
	}
}
